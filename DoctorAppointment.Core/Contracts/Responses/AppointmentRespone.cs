﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorAppointment.Core.Contracts.Responses
{
    public class AppointmentRespone
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string AppointmentTime { get; set; }
    }
}
