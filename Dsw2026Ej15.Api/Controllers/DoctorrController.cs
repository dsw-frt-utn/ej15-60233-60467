using Dsw2026Ej15.Api.models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [Route("[/api/doctors]")]//     [Route("[controller]")]Le saca el sufijo Controller y lo usa como ruta en el endpoint. 
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
            else { return Created(); }// Era Ok Antes: Devuelvo un 201. Este ok puede devolverse como string o , en algunos casos Json.

            var speciality = _persistence.GetSpecialityById(request.SpecialityID);

            if (speciality == null)
            {
                return BadRequest("La especialidad no existe");

            }
            else { return Created(); }
        }
    }
}
