using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Contracts.V1.Requests
{
    public class UpdateDoctorAvailabiltyRequest
    {
        public Guid doctorId { get; set; }
        public bool isAvailable { get; set; }
    }
}
