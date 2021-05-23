using AppointmentSystem.WebServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public class ScheduleService : IScheduleService
    {
        private ApplicationDbContext m_DbContext;
        private readonly IAppointmentService m_AppointmentService;

        public ScheduleService(ApplicationDbContext dbContext, IAppointmentService appointmentService)
        {
            m_DbContext = dbContext;
            m_AppointmentService = appointmentService;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments(Guid doctorId)
        {
            return await m_DbContext.Appointments.Where(a => a.DoctorId.Equals(doctorId) && !a.IsComplete).ToListAsync();
        }


        public async Task UpdateAppointment(Guid appointmentId)
        {
            var appointmentToUpdate = await m_AppointmentService.GetAppointment(appointmentId);
            if(appointmentToUpdate != null)
            {
                appointmentToUpdate.IsComplete = true;
                m_DbContext.Add(appointmentToUpdate);
                await m_DbContext.SaveChangesAsync();
            }
        }
    }
}
