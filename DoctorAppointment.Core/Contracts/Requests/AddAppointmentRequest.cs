namespace DoctorAppointment.Core.Contracts.Requests
{
    /// <summary>
    /// Adding an Appointment API request 
    /// </summary>
    public class AddAppointmentRequest
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string AppointmentTime { get; set; }
    }
}
