using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Data.Interfaces;
using Dsw2026Ej15.Domain.Entities;
using System.Text.Json;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        #region //atributos de clase
        private readonly List<Doctor> doctores = new List<Doctor>();
        private readonly List<Speciality> specialities = new List<Speciality>();
        #endregion
        #region //constructor
        public PersistenceInMemory() //Evitamos inicializar desde Program.
        {
            InicializarDatos();
        }
        #endregion
        #region //Getters.
        public List<Doctor> GetDoctors() => doctores;
        //public List<Doctor> GetDoctorsById(Guid id) => doctores. Hace Falta? 
        public List<Speciality> GetSpecialities() => specialities; //Es lo mismo que LoadSpecialities? 
        #endregion 
        #region //añadir doctor
        public bool AddDoctor(Doctor doctor) //bool?
        {
            try
            {
                doctores.Add(doctor);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region //InicializarEntidades
        private void InicializarDoctores()
        {
            var doctoresdtos = CargarDatosDeArchivo<DoctorDtos>("doctores");
            if (doctoresdtos != null)
            {
                foreach (var data in doctoresdtos)
                {
                    var specialitie = specialities.Find(s => s.Id == data.SpecialityId);
                    if (specialitie != null)
                    {
                        Doctor d = new Doctor(data.Name, data.LicenceNumber, data.IsActive, specialitie, data.id);
                    }
                }
            }

        }
        private void InicializarSpecialities()
        {
            var specialitieData = CargarDatosDeArchivo<Speciality>("especialidades");

            if (specialitieData != null)
            {
                foreach (var data in specialitieData)
                {
                    Speciality s = new Speciality(data.Name, data.Description, data.Id);
                    specialities.Add(s);
                }
            }

        }
        #endregion
        #region //LectorJson
        //Funcion de carga de datos Json
        private List<T>? CargarDatosDeArchivo<T>(string file) //Preguntarle al profesor como funciona, como declarar el path.
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\Sources", $"{file}.json");
            string jsonContent = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<List<T>>(jsonContent);
        }
        #endregion
        #region //InicializarDatos
        public void InicializarDatos()
        {
            InicializarDoctores();
            InicializarSpecialities();
        }
        #endregion
    }
}
