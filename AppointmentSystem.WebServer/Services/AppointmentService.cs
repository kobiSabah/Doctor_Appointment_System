using AppointmentSystem.WebServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext m_DbContext;

        public AppointmentService(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public async Task AddAppointment(Guid doctorId, Guid patientId)
        {
            Appointment appointment = new Appointment
            {
                DoctorId = doctorId,
                PatientId = patientId,
                AppointmentTime = new TimeSpan(0, 0, 5).ToString()
            };

            await m_DbContext.Appointments.AddAsync(appointment);
            await m_DbContext.SaveChangesAsync();
        }

        public async Task CancelAppointment(Guid appointmentId)
        {
            Appointment appointment = await GetAppointment(appointmentId);

            if (appointment != null)
            {
                m_DbContext.Appointments.Remove(appointment);
                await m_DbContext.SaveChangesAsync();
            }
        }

        public async Task<Appointment> GetAppointment(Guid appointmentId)
        {
            return await m_DbContext.Appointments.FirstOrDefaultAsync(a => a.Id.Equals(appointmentId));
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments()
        {
            return await m_DbContext.Appointments.ToListAsync();
        }
    }
}
