
namespace DoctorAppointment.Core.Contracts.Requests
{
    /// <summary>
    /// Change doctor availability API request 
    /// </summary>
    public class ChangeDoctorAvailabiltyRequest
    {
        public string DoctorId { get; set; }
        public bool IsAvailables { get; set; }
    }
}
