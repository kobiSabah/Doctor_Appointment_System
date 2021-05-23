
namespace AppointmentSystem.WebServer.Contracts.V1.Responses
{
    public class DoctorResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAvaliable { get; set; }
    }
}
