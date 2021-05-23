namespace DoctorAppointment.Core.Contracts.Requests
{
    /// <summary>
    /// Remove appointment API request
    /// </summary>
    public class RemoveAppointmentRequest
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
    }
}
