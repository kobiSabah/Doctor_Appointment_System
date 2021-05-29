using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSystem.WebServer.Contracts.V1.Requests
{
    public class AddToWaitingListRequest
    {
        public Guid doctorId { get; set; }
        public Guid patientId { get; set; }
        public string appointmentDuration { get; set; }
    }
}
