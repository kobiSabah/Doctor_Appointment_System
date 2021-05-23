using Microsoft.AspNetCore.Identity;

namespace AppointmentSystem.WebServer.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public bool IsAvailable { get; set; }
    }
}
