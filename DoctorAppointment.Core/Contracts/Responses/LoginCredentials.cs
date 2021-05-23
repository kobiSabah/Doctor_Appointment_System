using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorAppointment.Core.Contracts.Responses
{
    public class LoginCredentials
    {
        public string Token { get; set; }
        public string UserType { get; set; }
    }
}
