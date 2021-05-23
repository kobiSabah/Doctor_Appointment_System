namespace DoctorAppointment.Core.Models
{
    /// <summary>
    /// Doctor class representation 
    /// </summary>
    public class Doctor : User
    {
        public Doctor()
        {
            IsAvailable = true;
        }

        public bool IsAvailable { get; set; }

        public override string ToString()
        {
            return $"Dr {FirstName} {LastName}  Is available: {IsAvailable} ";
        }
    }
}
