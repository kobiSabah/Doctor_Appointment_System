
namespace DoctorAppointment.Core.Contracts.Requests
{
    /// <summary>
    /// Login API request 
    /// </summary>
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
