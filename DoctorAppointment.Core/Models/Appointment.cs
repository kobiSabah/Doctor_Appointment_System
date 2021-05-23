using System;

namespace DoctorAppointment.Core.Models
{
    /// <summary>
    /// Appointment form for patients
    /// </summary>
    public class Appointment
    {
        public string Id { get; set; }
        public string AppointmentTime { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public DateTime Date { get; set; }
    }
}
