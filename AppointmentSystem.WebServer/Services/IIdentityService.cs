using AppointmentSystem.WebServer.Models;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string username, string firstName, string lastName, string email, string userType, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}