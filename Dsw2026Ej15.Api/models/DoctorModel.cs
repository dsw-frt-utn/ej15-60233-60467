namespace Dsw2026Ej15.Api.models
{
    public record DoctorModel
    {
        public record Request(string? Name, string LicenseNumber, bool isActive, Guid SpecialityID);
        public record Response(string? Name, string LicenseNumber, bool isActive, Guid Id);

    }
   
}
