using AppointmentSystem.WebServer.Comfigurations;
using AppointmentSystem.WebServer.Contracts.V1.Requests;
using AppointmentSystem.WebServer.Contracts.V1.Responses;
using AppointmentSystem.WebServer.Models;
using AppointmentSystem.WebServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Controllers.V1
{
    /// <summary>
    /// Controller class for handles identity requests like login and register 
    /// </summary>
    public class IdentityController : Controller
    {
        private readonly IIdentityService m_IdentityService;

        public IdentityController(IIdentityService identityService)
        {
            m_IdentityService = identityService;
        }

        /// <summary>
        /// Register a user in database  
        /// </summary>
        /// <param name="registerationRequest"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterationRequest registerationRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationBadRespone
                {
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            AuthenticationResult result = await m_IdentityService.RegisterAsync(
                registerationRequest.Username,
                registerationRequest.FirstName, 
                registerationRequest.LastName,
                registerationRequest.Email,
                registerationRequest.UserType,
                registerationRequest.Password
                );

            if(!result.Success)
            {
                return BadRequest(new AuthenticationBadRespone
                {
                    Errors = result.ErrorMessage
                });
            }

            return Ok(new AuthenticationSuccessRespone
            {
                Token = result.Id,
                UserType = result.UserType
            });
        }

        /// <summary>
        /// Login user 
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            AuthenticationResult result = await m_IdentityService.LoginAsync(loginRequest.Email, loginRequest.Password);

            if (!result.Success)
            {
                return BadRequest(new AuthenticationBadRespone
                {
                    Errors = result.ErrorMessage
                });
            }

            return Ok(new AuthenticationSuccessRespone
            {
                Token = result.Id,
                UserType = result.UserType
            });
        }
    }
}
