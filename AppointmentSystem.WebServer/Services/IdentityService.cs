using AppointmentSystem.WebServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public class IdentityService : IIdentityService
    {
        private UserManager<User> m_UserManager;

        public IdentityService(UserManager<User> userManager)
        {
            m_UserManager = userManager;
        }


        public async Task<AuthenticationResult> RegisterAsync(string username, string firstName, string lastName, string email, string userType, string password)
        {
            User userResult = await m_UserManager.FindByEmailAsync(email);

            if (userResult != null)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new [] { "The user already exist" }
                };
            }
            else
            {
                IdentityResult result;
                User user;
                if (userType == "Doctor")
                {
                    user = new User
                    {
                        UserName = username,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        UserType = userType,
                        IsAvailable = true
                    };

                }
                else
                {
                    user = new User
                    {
                        UserName = username,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        UserType = userType
                    };
                }

                result = await m_UserManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Success = result.Succeeded,
                        ErrorMessage = result.Errors.Select(e => e.Description)
                    };
                }

                return new AuthenticationResult
                {
                    Success = true,
                    Id = user.Id,
                    UserType = user.UserType
                    
                };
            }
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            User user = await m_UserManager.FindByEmailAsync(email);
            const string ERROR_MESSAGE = "Wrong Email or Password";
            // The user doesn't exist 
            if (user == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new[] { ERROR_MESSAGE }
                };
            }

            // Check if the password is valid 
            bool isValidPassword = await m_UserManager.CheckPasswordAsync(user, password);

            if(!isValidPassword)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new[] { ERROR_MESSAGE }
                };
            }

            return new AuthenticationResult
            {
                Success = true,
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType
            };
        }
    }
}
