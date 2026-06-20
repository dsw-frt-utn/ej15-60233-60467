using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data.Dtos
{
    internal class DoctorDtos
    {
        public Guid id {  get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? LicenceNumber { get; set; } = string.Empty;//Para que servia el Init?
        public bool IsActive { get; set; }
        public Guid SpecialityId { get; set; } //El doctor deberia recibir un guid de su speciality? 
    }
}
