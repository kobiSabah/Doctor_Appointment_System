using System.Collections.Generic;

namespace AppointmentSystem.WebServer.Models
{
    public class AuthenticationResult
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
    }
}
