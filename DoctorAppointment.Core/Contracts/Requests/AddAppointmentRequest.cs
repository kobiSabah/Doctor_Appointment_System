namespace DoctorAppointment.Core.Contracts.Requests
{
    /// <summary>
    /// Adding an Appointment API request 
    /// </summary>
    public class AddAppointmentRequest
    {
        public string doctorId { get; set; }
        public string patientId { get; set; }
        public string appointmentDuration { get; set; }
    }
}
