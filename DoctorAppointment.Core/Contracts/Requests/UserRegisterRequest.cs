namespace DoctorAppointment.Core.Contracts.Requests
{
    /// <summary>
    /// Register API request
    /// </summary>
    public class UserRegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
    }
}
