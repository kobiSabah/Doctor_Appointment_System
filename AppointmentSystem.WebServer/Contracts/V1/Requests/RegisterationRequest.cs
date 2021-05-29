using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.WebServer.Contracts.V1.Requests
{
    public class RegisterationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
    }
}
