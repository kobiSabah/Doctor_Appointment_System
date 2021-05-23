using System;

namespace DoctorAppointment.Core.Models
{
    public class DoctorEventArgs : EventArgs
    {
        public string DoctorId { get; set; }
    }
}
