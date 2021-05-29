using DoctorAppointment.Core.Contracts.Requests;
using DoctorAppointment.Core.Contracts.Responses;
using DoctorAppointment.Core.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.Core.Services
{
    /// <summary>
    /// Class for managing doctors waiting list  
    /// </summary>
    public class ScheduleManager
    {
        private readonly DoctorManager r_DoctorService = DoctorManager.Instance;
        private ConcurrentQueue<Appointment> m_UpcommingAppointments = new ConcurrentQueue<Appointment>();
        public delegate void PatientNotifyEventHandler(object source, AppointmentEventArgs args);
        public event PatientNotifyEventHandler AppointmentStarted;

        public ScheduleManager()
        {
            r_DoctorService.DoctorAvailable += OnDoctorAvailable;
            r_DoctorService.AppointmentStarted += OnAppointmentStarted;
        }

        /// <summary>
        /// Handel user request to add appointment
        /// </summary>
        /// <param name="appointmentToAdd"> The appointment details </param>
        /// <returns></returns>
        public async Task AddAppointmentAsync(Appointment appointmentToAdd)
        {
            ApiResponse<object> result = await CheckDoctorAvailabilityAsync(appointmentToAdd.DoctorId);
            if (result.IsSuccess)
            {
                if (!(bool)(result.Context))
                {
                    await addToWaitingListAsync(appointmentToAdd);
                }
                else
                {
                    await r_DoctorService.StartAppointmentAsync(appointmentToAdd);
                }
            }

        }

        /// <summary>
        /// Notify to the listeners that the appointment is about to start 
        /// </summary>
        /// <param name="patientId">patient id</param>
        protected async virtual void OnAppointmentStarted(object source, AppointmentEventArgs args)
        {
            await RemoveFromWaitingList(args.AppointmentId);

            if (AppointmentStarted != null)
                AppointmentStarted(this, new AppointmentEventArgs() { PatientId = args.PatientId });
        }

        /// <summary>
        /// Get all the appointments from the waiting list 
        /// </summary>
        /// <param name="doctorId">Doctor id</param>
        /// <returns></returns>
        public async Task<ApiResponse<ConcurrentQueue<Appointment>>> GetUpcomingAppointmentsAsync(string doctorId)
        {
            ApiResponse<ConcurrentQueue<Appointment>> apiResponse = new ApiResponse<ConcurrentQueue<Appointment>>();

            using (HttpClient httpClient = new HttpClient())
            {
                string urlRequest = "https://localhost:44350/api/v1/waitingList?doctorId=" + doctorId;
                HttpResponseMessage response = await httpClient.GetAsync(urlRequest);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.Context = JsonConvert.DeserializeObject<ConcurrentQueue<Appointment>>(result);
                }
                else
                {
                    apiResponse.Errors = new[] { result };
                }
            }

            return apiResponse;
        }

        /// <summary>
        /// Method for get the next appointment from the queue when the doctor is available
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private async void OnDoctorAvailable(object source, DoctorEventArgs e)
        {
            if (m_UpcommingAppointments.IsEmpty)
            {
                ApiResponse<ConcurrentQueue<Appointment>> response = await GetUpcomingAppointmentsAsync(e.DoctorId);
                if (response.IsSuccess)
                {
                    m_UpcommingAppointments = response.Context;
                }
            }

            if (!m_UpcommingAppointments.IsEmpty)
            {
                m_UpcommingAppointments.TryDequeue(out Appointment appointment);
                await r_DoctorService.StartAppointmentAsync(appointment);
            }
        }

        /// <summary>
        /// Check the server if the doctor is available for appointment
        /// </summary>
        /// <param name="doctorId">Doctor id</param>
        /// <returns></returns>
        private async Task<ApiResponse<object>> CheckDoctorAvailabilityAsync(string doctorId)
        {
            ApiResponse<object> apiResponse = new ApiResponse<object>();

            Doctor doctor;
            using (HttpClient httpClient = new HttpClient())
            {
                string urlRequest = "https://localhost:44350/api/v1/doctors/" + doctorId;
                HttpResponseMessage response = await httpClient.GetAsync(urlRequest);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    doctor = JsonConvert.DeserializeObject<Doctor>(result);
                    apiResponse.Context = doctor.IsAvailable;
                }
                else
                {
                    apiResponse.Errors = new[] { "Something Wrong" };
                }
            }

            return apiResponse;
        }

        /// <summary>
        /// Add appointment to waiting list when the doctor is not available 
        /// </summary>
        /// <param name="appointmentToAdd"></param>
        /// <returns></returns>
        private async Task<ApiResponse<object>> addToWaitingListAsync(Appointment appointmentToAdd)
        {
            ApiResponse<object> apiResponse = new ApiResponse<object>();

            AddAppointmentRequest request = new AddAppointmentRequest
            {
                appointmentDuration = appointmentToAdd.AppointmentTime,
                doctorId = appointmentToAdd.DoctorId,
                patientId = appointmentToAdd.PatientId
            };

            using (HttpClient httpClient = new HttpClient())
            {
                string urlRequest = "https://localhost:44350/api/v1/waitingList";
                string json = JsonConvert.SerializeObject(request);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(urlRequest, data);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    apiResponse.Errors = new[] { "Something wrong" };
                }

                return apiResponse;
            }
        }

        /// <summary>
        /// Cancel/ Remove patient appointment
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<object>> RemoveFromWaitingList(string appointmentId)
        {
            ApiResponse<object> apiResponse = new ApiResponse<object>();

            using (HttpClient httpClient = new HttpClient())
            {
                string urlRequest = "https://localhost:44350/api/v1/waitingList/" + appointmentId;
                HttpResponseMessage response = await httpClient.DeleteAsync(urlRequest);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    apiResponse.Errors = new[] { "Something wrong" };
                }
            }

            return apiResponse;
        }
    }
}
