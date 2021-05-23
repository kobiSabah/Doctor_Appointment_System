using AppointmentSystem.WebServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.WebServer
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options)
            :base(options)
        {

        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Appointment> WaitingList { get; set; }
    }
}
