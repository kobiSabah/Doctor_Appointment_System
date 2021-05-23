using AppointmentSystem.WebServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public class WaitingListService : IWaitingListService
    {
        private readonly ApplicationDbContext m_DbContext;

        public WaitingListService(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public async Task AddToWatingList(Guid doctorId, Guid patientId, string appointmentDuration)
        {
            Appointment appointment = new Appointment
            {
                DoctorId = doctorId,
                PatientId = patientId,
                AppointmentTime = appointmentDuration,
                Date = DateTime.Now.ToString()
            };

            await m_DbContext.WaitingList.AddAsync(appointment);

            await m_DbContext.SaveChangesAsync();
        }

        public async Task RemoveAppointment(Guid appointmentId)
        {
            Appointment appointment = await m_DbContext.WaitingList.FirstOrDefaultAsync(a => a.Id.Equals(appointmentId));

            if (appointment != null)
            {
                m_DbContext.WaitingList.Remove(appointment);
                await m_DbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Appointment>> GetAllUpcomeingAppointment(Guid doctorId)
        {
            return await m_DbContext.WaitingList.Where(a => a.DoctorId.Equals(doctorId)).ToListAsync();
        }

        public async Task<Appointment> GetUpcomeingAppointment(Guid doctorId)
        {
            return await m_DbContext.WaitingList.FirstOrDefaultAsync(a => a.DoctorId.Equals(doctorId));
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointments(Guid patientId)
        {
            return await m_DbContext.WaitingList.Where(a => a.PatientId.Equals(patientId)).ToListAsync();
        }
    }
}
