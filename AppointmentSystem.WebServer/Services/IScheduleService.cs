using AppointmentSystem.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<Appointment>> GetAllAppointments(Guid doctorId);

        Task UpdateAppointment(Guid appointmentId);
    }
}
