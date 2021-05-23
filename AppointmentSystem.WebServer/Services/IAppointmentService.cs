using AppointmentSystem.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public interface IAppointmentService
    {
        Task AddAppointment(Guid doctorId, Guid patientId);
        Task CancelAppointment(Guid appointmentId);
        Task<Appointment> GetAppointment(Guid appointmentId);
        Task<IEnumerable<Appointment>> GetAllAppointments();
    }
}
