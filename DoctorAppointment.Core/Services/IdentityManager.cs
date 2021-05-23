using DoctorAppointment.Core.Contracts.Requests;
using DoctorAppointment.Core.Contracts.Responses;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Core.Services
{
    /// <summary>
    /// Class representation for login and register users 
    /// </summary>
    public class IdentityManager
    {
        /// <summary>
        /// Method for registration through the server  
        /// </summary>
        /// <param name="userToRegister">User to register</param>
        /// <returns></returns>
        public async Task<ApiResponse<LoginCredentials>> RegisterAsync(UserRegisterRequest userToRegister)
        {
            ApiResponse<LoginCredentials> apiResponse = new ApiResponse<LoginCredentials>();
            using (HttpClient httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(userToRegister);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("https://localhost:44350/api/v1/identity/register", data);
                string result = await response.Content.ReadAsStringAsync();
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if(response.IsSuccessStatusCode)
                {
                    apiResponse.Context = JsonConvert.DeserializeObject<LoginCredentials>(result);

                }
                else
                {
                    apiResponse.Errors = new[] { result };
                }

                return apiResponse;
            }
        }

        /// <summary>
        /// Login method 
        /// </summary>
        /// <param name="loginCredentials">User login data</param>
        /// <returns></returns>
        public async Task<ApiResponse<LoginCredentials>> LoginAsync(UserLoginRequest loginCredentials)
        {
            ApiResponse<LoginCredentials> apiResponse = new ApiResponse<LoginCredentials>();

            using (HttpClient httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(loginCredentials);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("https://localhost:44350/api/v1/identity/login", data);
                string result = await response.Content.ReadAsStringAsync();
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.Context = JsonConvert.DeserializeObject<LoginCredentials>(result);
                }
                else
                {
                    apiResponse.Errors = new[] { result };

                }

                return apiResponse;
            }
        }
    }
}
