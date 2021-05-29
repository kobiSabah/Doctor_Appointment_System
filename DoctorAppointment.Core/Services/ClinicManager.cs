using DoctorAppointment.Core.Contracts.Responses;
using DoctorAppointment.Core.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DoctorAppointment.Core.Services
{
    /// <summary>
    /// Singleton class for managing all the clinic service
    /// </summary>
    public sealed class ClinicManager
    {
        private ScheduleManager scheduleManager;
        private DoctorManager doctorManager;
        private static ClinicManager s_Instance = null;
        private static readonly object creationLock = new object();

        private ClinicManager()
        {
            scheduleManager = new ScheduleManager();
            doctorManager = DoctorManager.Instance;
        }

        public static ClinicManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (creationLock)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new ClinicManager();
                        }
                    }
                }

                return s_Instance;
            }
        }

        /// <summary>
        /// Get all the doctors
        /// </summary>
        /// <param name="filterByAvailability">Optional parameter for filtering the doctor by a by availability default its false </param>
        /// <returns>List of doctor</returns>
        public async Task<ApiResponse<List<Doctor>>> GetAllDoctorsAsync(bool filterByAvailability = false)
        {
            ApiResponse<List<Doctor>> apiRespone = new ApiResponse<List<Doctor>>();

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://localhost:44350/api/v1/doctors");
                if (response.IsSuccessStatusCode)
                {
                    apiRespone.IsSuccess = true;
                    string result = await response.Content.ReadAsStringAsync();
                    apiRespone.Context = JsonConvert.DeserializeObject<List<Doctor>>(result);

                    if (apiRespone != null)
                    {
                        if (filterByAvailability)
                        {
                            apiRespone.Context = apiRespone.Context.FindAll(d => d.IsAvailable);
                        }
                    }
                }
                else
                {
                    apiRespone.IsSuccess = false;
                    apiRespone.Errors = new[] { " Something wrong " };
                }
            }

            return apiRespone;
        }

        /// <summary>
        /// Get doctor waiting list sort by arrival time
        /// </summary>
        /// <param name="doctorId">Doctor id</param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Patient>>> GetWatingListPatientsAsync(string doctorId)
        {
            ApiResponse<List<Patient>> apiResponse = new ApiResponse<List<Patient>>();
            using (HttpClient httpClient = new HttpClient())
            {
                string urlRequest = "https://localhost:44350/api/v1/waitingList?doctorId=" + doctorId;
                HttpResponseMessage response = await httpClient.GetAsync(urlRequest);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    List<Appointment> appointments = new List<Appointment>();
                    string result = await response.Content.ReadAsStringAsync();
                    appointments = JsonConvert.DeserializeObject<List<Appointment>>(result).OrderBy(a => a.Date).ToList();

                    foreach (Appointment appointment in appointments)
                    {
                        ApiResponse<Patient> getPatientApiResponse = await GetPatientByIdAsync(appointment.PatientId);
                        apiResponse.IsSuccess = getPatientApiResponse.IsSuccess;

                        if (getPatientApiResponse.IsSuccess)
                            apiResponse.Context.Add(getPatientApiResponse.Context);
                        else
                            apiResponse.Errors = getPatientApiResponse.Errors;
                    }
                }
                else
                    apiResponse.Errors = new[] { "Something wrong" };
            }

            return apiResponse;
        }

        /// <summary>
        /// Get patient details 
        /// </summary>
        /// <param name="patientId">Patient id</param>
        /// <returns></returns>
        public async Task<ApiResponse<Patient>> GetPatientByIdAsync(string patientId)
        {
            ApiResponse<Patient> apiResponse = new ApiResponse<Patient>();

            using (HttpClient httpClient = new HttpClient())
            {
                string urlRequest = "https://localhost:44350/api/v1/patients/" + patientId;
                HttpResponseMessage response = await httpClient.GetAsync(urlRequest);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccess = true;
                    string result = await response.Content.ReadAsStringAsync();
                    apiResponse.Context = JsonConvert.DeserializeObject<Patient>(result);
                }
                else
                {
                    apiResponse.Errors = new[] { "Something wrong" };
                }
            }

            return apiResponse;
        }

        /// <summary>
        /// Method for doctors that login and want to get patients 
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        public async Task StartWork(string doctorId)
        {
            ApiResponse<ConcurrentQueue<Appointment>> apiResponse = await scheduleManager.GetUpcomingAppointmentsAsync(doctorId);
            if (apiResponse.IsSuccess)
            {
                ConcurrentQueue<Appointment> upcommingApoointmnets = apiResponse.Context;
                if (upcommingApoointmnets != null)
                {
                    Appointment appointment;
                    upcommingApoointmnets.TryDequeue(out appointment);
                    await doctorManager.StartAppointmentAsync(appointment);
                }
            }
        }

        /// <summary>
        /// Get all patient appointments
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Appointment>>> GetPatientAppointmentsAsync(string patientId)
        {
            ApiResponse<List<Appointment>> apiResponse = new ApiResponse<List<Appointment>>();

            using (HttpClient httpClient = new HttpClient())
            {
                string urlRequest = "https://localhost:44350/api/v1/waitingList/patient/" + patientId;
                HttpResponseMessage response = await httpClient.GetAsync(urlRequest);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    apiResponse.Context = JsonConvert.DeserializeObject<List<Appointment>>(result);
                }
                else
                {
                    apiResponse.Errors = new[] { " Something wrong " };
                }
            }

            return apiResponse;
        }

        /// <summary>
        /// Cancel patient appointments
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public async Task CancelAppointment(string appointmentId)
        {
            await scheduleManager.RemoveFromWaitingList(appointmentId);
        }
    }
}
