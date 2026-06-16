using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Doctor:BaseEntity
    {

        public  string? Name { get; init; }
        public  string? LicenceNumber {  get; init; } //Para que servia el Init?
        public bool IsActive {  get; init; }
        Speciality? Speciality { get; init; }

        public Doctor(string name, string licenceNumber,bool isActive, Speciality speciality,Guid? id = null): base(id)
        {
            Name = name;
            LicenceNumber = licenceNumber;
            IsActive = isActive;
            Speciality = speciality;
            
        }

    }
}
