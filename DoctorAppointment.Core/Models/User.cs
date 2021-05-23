using System;

namespace DoctorAppointment.Core.Models
{
    /// <summary>
    /// Base class for application users 
    /// </summary>
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
