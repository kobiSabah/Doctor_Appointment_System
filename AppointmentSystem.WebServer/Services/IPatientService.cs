using AppointmentSystem.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<User>> GetAllPatients();
        Task<User> GetPatientById(Guid id);
    }
}
