using AppointmentSystem.WebServer.Comfigurations;
using AppointmentSystem.WebServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Controllers.V1
{
    public class ScheduleController : Controller
    {
        private IScheduleService m_ScheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            m_ScheduleService = scheduleService;
        }

        [HttpGet(ApiRoutes.Schedule.GetAll)]
        public async Task<IActionResult> GetAllAppointments(Guid doctorId)
        {
            var appointments = await m_ScheduleService.GetAllAppointments(doctorId);

            return Ok(appointments);
        }

        [HttpPatch(ApiRoutes.Schedule.Update)]
        public async Task<IActionResult> UpdateAppointment(Guid appointmentId)
        {
            await m_ScheduleService.UpdateAppointment(appointmentId);

            return Ok();
        }

    }
}
