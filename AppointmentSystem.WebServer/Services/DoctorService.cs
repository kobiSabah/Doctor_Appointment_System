using AppointmentSystem.WebServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Services
{
    public class DoctorService : IDoctorService
    {
        private UserManager<User> m_UserManager;

        public DoctorService(UserManager<User> userManager)
        {
            m_UserManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAllDoctors()
        {
            return await m_UserManager.Users.Where(u => u.UserType.Equals(Enum.GetName(typeof(eUserType), eUserType.Doctor))).ToListAsync();
        }

        public async Task<User> GetDoctorById(Guid doctorId)
        {
            User doctor = await m_UserManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(doctorId.ToString()));
            return doctor;
        }

        public async Task UpdateDoctorAvailability(Guid id, bool available)
        {
            User doctor = await GetDoctorById(id);
            doctor.IsAvailable = available;
            await m_UserManager.UpdateAsync(doctor);
        }
    }
}
