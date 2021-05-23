using System;

namespace AppointmentSystem.WebServer.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string AppointmentTime { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public string Date { get; set; }
    }
}
