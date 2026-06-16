using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data.Dtos
{
    internal class SpecialityDtos
    {   
        public String? Name { get; set; }
        public String? Description { get; set; }
        public Guid id { get; set; }


        //Usa record el profesor.
        //internal record SpecialityDto(Guid Id,string Name, string Description)
    }
}
