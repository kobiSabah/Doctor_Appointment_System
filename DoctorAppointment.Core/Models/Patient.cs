using System;

namespace DoctorAppointment.Core.Models
{
    /// <summary>
    /// Patient class representation 
    /// </summary>
    public class Patient : User
    {

        public Patient() : base()
        {
            ArrivalTime = DateTime.Now;
        }

        public DateTime ArrivalTime { get; private set; }
    }
}
