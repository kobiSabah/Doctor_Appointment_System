using AppointmentSystem.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public interface IWaitingListService
    {
        Task<Appointment> GetUpcomeingAppointment(Guid doctorId);
        Task RemoveAppointment(Guid appointmentId);

        Task<IEnumerable<Appointment>> GetAllUpcomeingAppointment(Guid doctorId);

        Task AddToWatingList(Guid doctorId, Guid patientId, string time);

        Task<IEnumerable<Appointment>> GetPatientAppointments(Guid patientId);
    }
}
