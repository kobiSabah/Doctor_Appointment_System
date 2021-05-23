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
    /// Controller class for handles patients informations requests 
    /// </summary>
    public class PatientController : Controller
    {
        private IPatientService m_PatientService;

        public PatientController(IPatientService patientService)
        {
            m_PatientService = patientService;
        }

        /// <summary>
        /// Get all patients 
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Patients.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<User> doctors = await m_PatientService.GetAllPatients();

            return Ok(doctors);
        }

        /// <summary>
        /// Get patients information
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Patients.Get)]
        public async Task<IActionResult> Get(Guid patientId)
        {
            User patient = await m_PatientService.GetPatientById(patientId);

            if(patient != null)
            {
                return Ok(new PatientRespone
                {
                    Id = patient.Id,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                });

            }
            return BadRequest("Patient not found");
        }
    }
}
