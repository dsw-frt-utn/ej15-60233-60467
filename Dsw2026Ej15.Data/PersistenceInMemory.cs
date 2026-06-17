using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistenceInMemory
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

        public List<Doctor> DeleteById(Guid id)
        {
           doctores.RemoveAll(x=>x.Id == id );
            return doctores;
        }
        public Speciality? GetSpecialityById(Guid id)
        {

            return specialities.SingleOrDefault(x => x.Id == id);
        }

        public Doctor? GetDoctorById(Guid id)
        {
            return doctores.SingleOrDefault(x => x.Id == id);
        }
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
            var doctoresdtos = CargarDatosDeDoctores<DoctorDtos>("doctores");
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
            var specialitieData = CargarDatosDeEspecialidades<Speciality>("especialidades");

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
        private List<T>? CargarDatosDeEspecialidades<T>(string file) //Preguntarle al profesor como funciona, como declarar el path.
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "specialities.json");
            //En clase el combine , en el parametro string va "sources","specialities"
            string jsonContent = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(jsonContent, options); //Aqui el profesor puso un parametro para que no importe si esta en mayuscula o minuscula.
            
                
        
        }
        #endregion
        #region //InicializarDatos
        public void InicializarDatos()
        {
            InicializarDoctores();
            InicializarSpecialities();
        }
        private List<T>? CargarDatosDeDoctores<T>(string file)
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "doctores.json");
            //En clase el combine , en el parametro string va "sources","specialities"
            string jsonContent = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(jsonContent, options);
        }
        #endregion


    }
}