using AppointmentSystem.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<User>> GetAllDoctors();
        Task<User> GetDoctorById(Guid id);
        Task UpdateDoctorAvailability(Guid id, bool available);
    }
}
