using AppointmentSystem.WebServer.Comfigurations;
using AppointmentSystem.WebServer.Contracts.V1.Responses;
using AppointmentSystem.WebServer.Models;
using AppointmentSystem.WebServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Controllers.V1
{
    /// <summary>
    /// Class for handles HTTP request about doctors 
    /// </summary>
    public class DoctorController : Controller
    {
        private IDoctorService m_DoctorService;

        public DoctorController(IDoctorService doctorService)
        {
            m_DoctorService = doctorService;
        }

        /// <summary>
        /// Get all the doctors in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Doctors.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<User> doctors = await m_DoctorService.GetAllDoctors();
            List<DoctorResponse> respone = new List<DoctorResponse>();

            foreach (User doctor in doctors)
            {
                respone.Add(generateRespone(doctor));
            }

            return Ok(respone);
        }

        /// <summary>
        /// Get doctor details by id 
        /// </summary>
        /// <param name="doctorId">Doctor Id</param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Doctors.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid doctorId)
        {
            User doctor = await m_DoctorService.GetDoctorById(doctorId);
            if (doctor != null)
            {
                DoctorResponse respone = generateRespone(doctor);
                return Ok(respone);
            }

            return BadRequest("Doctor was not found.");
        }

        /// <summary>
        /// Update doctor availability 
        /// </summary>
        /// <param name="doctorId">Doctor id</param>
        /// <param name="isAvailable">new Availability</param>
        /// <returns></returns>
        [HttpPatch(ApiRoutes.Doctors.Update)]
        public async Task<IActionResult> UpdateDoctorAvailability(Guid doctorId, bool isAvailable)
        {
            await m_DoctorService.UpdateDoctorAvailability(doctorId, isAvailable);
            
            return Ok();
        }

        /// <summary>
        /// Hiding user information like password and email 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private DoctorResponse generateRespone(User user)
        {
            if (user != null)
            {
                return new DoctorResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAvaliable = user.IsAvailable
                };
            }
            return new DoctorResponse();
        }
    }
}
