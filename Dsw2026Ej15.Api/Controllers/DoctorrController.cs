using Dsw2026Ej15.Api.models;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                return BadRequest();
            }
           // Era Ok Antes: Devuelvo un 201. Este ok puede devolverse como string o , en algunos casos Json.

            var speciality = _persistence.GetSpecialityById(request.SpecialityID);

            if (speciality == null)
            {
                return BadRequest("La especialidad no existe");

            }
            return Created(); 
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctor(DoctorModel.Response response)
        {

            var doctores = _persistence.GetDoctors();
            var activos = doctores.Where(p => p.IsActive == true);
            if (!activos.Any() )
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
        [HttpGet]
        [Route("api/doctors/{id}")]
        public async Task<IActionResult> GetDoctorByID(Guid id)
        {
            var idDoc = _persistence.GetDoctorById(id);
            if (idDoc == null || !idDoc.IsActive) {
                return NotFound("El doctor no esta activo o no existe");
            }
            var doctores= new DoctorModel.Response(idDoc.Name, idDoc.LicenseNumber, idDoc.IsActive,idDoc.Id);
            return Ok(doctores);
        }
        [HttpDelete]
        [Route("api/doctors/{id}")]
        public async Task<IActionResult> DeleteDoctorByID(Guid id)
        {
            var deleteados = _persistence.DeleteById(id);
            var doctoredelete = deleteados.Where(p => p.IsActive == true);
            if (doctoredelete == null || !doctoredelete.Any() )
            {
                return NotFound("El doctor no esta activo o no existe");
            }
            return NoContent();
        }
    }

    }

