using System;

namespace DoctorAppointment.Core.Models
{
    public class AppointmentEventArgs : EventArgs
    {
        public string AppointmentId { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
    }
}
