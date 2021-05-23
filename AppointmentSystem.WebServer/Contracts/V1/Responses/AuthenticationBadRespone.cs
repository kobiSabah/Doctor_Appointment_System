using System.Collections.Generic;

namespace AppointmentSystem.WebServer.Contracts.V1.Responses
{
    public class AuthenticationBadRespone
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
