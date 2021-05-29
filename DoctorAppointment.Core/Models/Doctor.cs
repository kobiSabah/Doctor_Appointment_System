using Newtonsoft.Json;

namespace DoctorAppointment.Core.Models
{
    /// <summary>
    /// Doctor class representation 
    /// </summary>
    public class Doctor : User
    {
        public Doctor()
        {

        }

        [JsonProperty("isAvaliable")]
        public bool IsAvailable { get; set; }

        public override string ToString()
        {
            return $"Dr {FirstName} {LastName}  Is available: {IsAvailable} ";
        }
    }
}
