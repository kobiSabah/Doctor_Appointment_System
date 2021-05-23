using AppointmentSystem.WebServer.Comfigurations;
using AppointmentSystem.WebServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Controllers.V1
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService m_AppointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            m_AppointmentService = appointmentService;
        }
        [HttpPost(ApiRoutes.Appointment.Add)]
        public async Task<IActionResult> AddAppointment(Guid doctorId,Guid patientId)
        {
            await m_AppointmentService.AddAppointment(doctorId, patientId);
            return Ok();
        }

        [HttpDelete(ApiRoutes.Appointment.Delete)]
        public async Task<IActionResult> CancelAppointment(Guid appointmentId)
        {
            await m_AppointmentService.CancelAppointment(appointmentId);
            return Ok();
        }

        [HttpGet(ApiRoutes.Appointment.Get)]
        public async Task<IActionResult> GetAppointment(Guid appointmentId)
        {
            var appointment = await m_AppointmentService.GetAppointment(appointmentId);

            return Ok(appointment);
        }

        [HttpGet(ApiRoutes.Appointment.GetAll)]
        public async Task<IActionResult> GetAllAppointment()
        {
            var appointments = await m_AppointmentService.GetAllAppointments();

            return Ok(appointments);
        }
    }
}
