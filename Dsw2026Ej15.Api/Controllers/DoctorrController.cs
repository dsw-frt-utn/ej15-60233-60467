using Dsw2026Ej15.Api.models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [Route("/api/doctors")]//     [Route("[controller]")]Le saca el sufijo Controller y lo usa como ruta en el endpoint. 
    public class DoctorsController : ControllerBase
    {
        private readonly IPersistenceInMemory _persistence;
        public DoctorsController(IPersistenceInMemory persistence)
        {
            _persistence = persistence;

            //Vamos a usar 200 y 400 protocolo HTTP.

        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorModel.Request request)
        {
            if (request == null) return BadRequest("Body mal formado");
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("Campos Vacios. Completar e intentar de nuevo"); //Aqui tira excepciones throw new ValidationException("")
            }
            // Era Ok Antes: Devuelvo un 201. Este ok puede devolverse como string o , en algunos casos Json.

            var speciality = _persistence.GetSpecialityById(request.SpecialityID);

            if (speciality == null)
            {
                throw new ValidationException("Campo de especialidad no coincide"); 

            }
            var doctor = new Doctor(request.Name, request.LicenseNumber, request.isActive, speciality);

            if (_persistence.AddDoctor(doctor) == true)
            {
                foreach (var d in _persistence.GetDoctors())
                {
                    Console.WriteLine($"{d.Name}, {d.IsActive}");
                }
                return Created($"/api/doctors/{doctor.Id}", new DoctorModel.Response(
        doctor.Name,
        doctor.LicenseNumber,
        doctor.IsActive,
        doctor.Id
    ));

            }
            return BadRequest("no se esta cargando el doctor, corroborar");

        }
        [HttpGet]
        public async Task<IActionResult> GetDoctor()
        {

            var doctores = _persistence.GetDoctors();
            var activos = doctores.Where(p => p.IsActive == true);
            if (!activos.Any())
            {
                return BadRequest("No hay medicos activos");
            }
            var resultado = activos.Select(d => new DoctorModel.Response(
                d.Name,
                d.LicenseNumber,
                d.IsActive,
                d.Id));

            return Ok(resultado);


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorByID(Guid id)
        {
            var idDoc = _persistence.GetDoctorById(id);
            if (idDoc == null || !idDoc.IsActive)
            {
                return NotFound("El doctor no esta activo o no existe");
            }
            var doctores = new DoctorModel.Response(idDoc.Name, idDoc.LicenseNumber, idDoc.IsActive, idDoc.Id);
            return Ok(doctores);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorByID(Guid id)
        {
            var doctor = _persistence.GetDoctors().FirstOrDefault(p => p.Id == id);
            if (doctor == null || !doctor.IsActive)
            {
                return NotFound("El doctor no esta activo o no existe");
            }
            _persistence.DeleteById(id);
            foreach (var d in _persistence.GetDoctors())
            {
                Console.WriteLine($"eliminado: {d.IsActive}");
            }

            return NoContent();
        }
    }

}

// RECORDATORIO - ERROR DE ID EN RESPUESTA DEL ENDPOINT
//
// Tuvimos un bug donde el DELETE siempre devolvía 404.
// El problema no estaba en el DeleteById ni en la lógica de baja,
// sino en el record Response del DoctorModel.
//
// El Response devolvía el SpecialityID en lugar del Id del doctor:
//
//    ANTES (MAL):
//    public record Response(string Name, string LicenseNumber, bool isActive, Guid SpecialityID);
//
//    DESPUÉS (BIEN):
//    public record Response(string Name, string LicenseNumber, bool isActive, Guid Id);
//
// Entonces cuando hacía el GET para ver los doctores, el "id" que
// me mostraba Swagger era el id de la especialidad del doctor, no el suyo propio.
// Al usar ese id en el DELETE, obviamente no encontraba ningún doctor con ese Guid
// y devolvía 404.
//
// LECCIÓN: Cuando un endpoint devuelve 404 inesperado, verificar primero
// que los IDs que estamos usando realmente corresponden a la entidad correcta.
// No asumir que el id que muestra la respuesta es el que necesitamos.