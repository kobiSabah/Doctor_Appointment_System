namespace AppointmentSystem.WebServer.Comfigurations
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
        }

        public static class Doctors
        {
            public const string Get = Base + "/doctors/{doctorId}";
            public const string Update = Base + "/doctors/{doctorId}";
            public const string GetAll = Base + "/doctors";

        }

        public static class Patients
        {
            public const string Get = Base + "/patients/{patientId}";
            public const string GetAll = Base + "/patients";

        }

        public static class WaitingList
        {
            public const string Add = Base + "/waitingList";
            public const string GetPatientAppointments = Base + "/waitingList/patient/{patientId}";
            public const string Delete = Base + "/waitingList/{appointmentId}";
            public const string Get = Base + "/waitingList/{doctorId}";
            public const string GetAll = Base + "/waitingList";

        }
    }
}
