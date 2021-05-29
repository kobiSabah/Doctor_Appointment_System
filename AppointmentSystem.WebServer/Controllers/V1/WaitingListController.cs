using AppointmentSystem.WebServer.Comfigurations;
using AppointmentSystem.WebServer.Contracts.V1.Requests;
using AppointmentSystem.WebServer.Models;
using AppointmentSystem.WebServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Controllers.V1
{
    /// <summary>
    /// Controller class for handles waiting list informations requests 
    /// </summary>
    public class WaitingListController : Controller
    {
        private IWaitingListService waitingListService;

        public WaitingListController(IWaitingListService waitingListService)
        {
            this.waitingListService = waitingListService;
        }

        /// <summary>
        /// Remove patient appointment from the waiting list
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        [HttpDelete(ApiRoutes.WaitingList.Delete)]
        public async Task<IActionResult> RemoveAppointment(Guid appointmentId)
        {
            await waitingListService.RemoveAppointment(appointmentId);
            return Ok();
        }

        /// <summary>
        /// Get all doctor upcoming appointments 
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.WaitingList.GetAll)]
        public async Task<IActionResult> GetAllAppointments(Guid doctorId)
        {
            IEnumerable<Appointment> appointments = await waitingListService.GetAllUpcomeingAppointment(doctorId);
            return Ok(appointments);
        }

        /// <summary>
        /// Insert new appointment in the waiting list
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="patientId"></param>
        /// <param name="appointmentDuration"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.WaitingList.Add)]
        public async Task<IActionResult> AddToWaitingList([FromBody]AddToWaitingListRequest request)
        {
            await waitingListService.AddToWatingList(request.doctorId, request.patientId, request.appointmentDuration);
            return Ok();
        }

        /// <summary>
        /// Get all appointments scheduled by the patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.WaitingList.GetPatientAppointments)]
        public async Task<IActionResult> GetPatientAppointment(Guid patientId)
        {
            IEnumerable<Appointment> appointments = await waitingListService.GetPatientAppointments(patientId);
            return Ok(appointments);
        }
    }
}
