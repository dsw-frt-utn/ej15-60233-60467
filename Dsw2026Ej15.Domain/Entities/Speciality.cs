using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Speciality:BaseEntity
    {
        public String? Name {  get; set; }
        public String? Description { get; set; }
        public Speciality(String name, String description,Guid? id = null):base(id)
        {
            Name = name;
            Description = description;
            
        }
    }
}
