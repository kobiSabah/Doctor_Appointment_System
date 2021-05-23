using AppointmentSystem.WebServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public class PatientService : IPatientService
    {
        private UserManager<User> m_UserManager;

        public PatientService(UserManager<User> userManager)
        {
            m_UserManager = userManager;

        }
        public async Task<IEnumerable<User>> GetAllPatients()
        {
            return await m_UserManager.Users.Where(u => u.UserType.Equals(Enum.GetName(typeof(eUserType), eUserType.Patieon))).ToListAsync();
        }

        public async Task<User> GetPatientById(Guid patientId)
        {
            User patient = await m_UserManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(patientId.ToString()));
            return patient;
        }
    }
}
