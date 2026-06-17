using System;
using System.Collections.Generic;
using System.Text;
using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistenceInMemory
    {
        //Cargamos las specialities. 
        List<Speciality> GetSpecialities();
        //Metodo para obtener mediante Id
         Speciality? GetSpecialityById(Guid id);
        //Carga de doctores.
        List<Doctor> GetDoctors();
        //Agregar doctores
        bool AddDoctor(Doctor doctor); 
        //Obtener Doctor por Id
        Doctor? GetDoctorById(Guid id);
        //IEnumerable<Doctor> GetAllDoctors();
        //CONSULTAR.
        List<Doctor> DeleteById(Guid id);
    }
}
