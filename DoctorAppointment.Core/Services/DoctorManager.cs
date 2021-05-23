using DoctorAppointment.Core.Contracts.Requests;
using DoctorAppointment.Core.Contracts.Responses;
using DoctorAppointment.Core.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoctorAppointment.Core.Services
{
    /// <summary>
    /// Class for managing all the doctor service 
    /// </summary>
    public class DoctorManager
    {
        public delegate void DoctorAvailableEventHandler(object source, DoctorEventArgs args);
        public event DoctorAvailableEventHandler DoctorAvailable;

        public delegate void PatientNotifyEventHandler(object source, AppointmentEventArgs args);
        public event PatientNotifyEventHandler AppointmentStarted;

        private static DoctorManager s_Instance = null;
        private static readonly object creationLock = new object();

        private DoctorManager()
        {
        }

        public static DoctorManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (creationLock)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new DoctorManager();
                        }
                    }
                }

                return s_Instance;
            }
        }

        /// <summary>
        /// Method for simulate appointment
        /// </summary>
        /// <param name="appointmentToStart">Appointment details</param>
        /// <returns></returns>
        public async Task StartAppointmentAsync(Appointment appointmentToStart)
        {
            ApiResponse<Doctor> response = await FindDoctorByIdAsync(appointmentToStart.DoctorId);
            if(response.IsSuccess)
            {
                Doctor doctor = response.Context;
                if (doctor.IsAvailable)
                {
                    await UpdateDoctorAvailabilityAsync(doctor.Id, false);
                    OnAppointmentStarted(appointmentToStart);
                    await Task.Run(() =>
                    {
                        Thread.Sleep(TimeSpan.Parse(appointmentToStart.AppointmentTime));
                    });

                    await UpdateDoctorAvailabilityAsync(doctor.Id, true);
                    OnDoctorAvailable(appointmentToStart.DoctorId);
                }
            }
        }

        /// <summary>
        /// Update the server about availability
        /// </summary>
        /// <param name="doctorId">Doctor id</param>
        /// <param name="isAvailable">Is available</param>
        /// <returns></returns>
        private async Task UpdateDoctorAvailabilityAsync(string doctorId, bool isAvailable)
        {
            ChangeDoctorAvailabiltyRequest request = new ChangeDoctorAvailabiltyRequest
            {
                DoctorId = doctorId,
                IsAvailables = isAvailable
            };

            ApiResponse<StringBuilder> apiResponse = new ApiResponse<StringBuilder>();

            using (HttpClient httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(request);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                string urlRequest = "https://localhost:44350/api/v1/doctors/" + doctorId;
                HttpResponseMessage response = await httpClient.PutAsync(urlRequest, data);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if(!response.IsSuccessStatusCode)
                {
                    apiResponse.Errors = new[] { "Something wrong" };
                }
            }
        }

        /// <summary>
        /// Find doctor by there id
        /// </summary>
        /// <param name="doctorId">Doctor id</param>
        /// <returns></returns>
        public async Task<ApiResponse<Doctor>> FindDoctorByIdAsync(string doctorId)
        {
            ApiResponse<Doctor> apiResponse = new ApiResponse<Doctor>() ;

            using (HttpClient httpClient = new HttpClient())
            {
                string urlRequest = "https://localhost:44350/api/v1/doctors/" + doctorId;
                HttpResponseMessage response = await httpClient.GetAsync(urlRequest);
                apiResponse.IsSuccess = response.IsSuccessStatusCode;

                if(response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    apiResponse.Context = JsonConvert.DeserializeObject<Doctor>(result);
                }
                else
                {
                    apiResponse.Errors = new[] { "Something wrong" };
                }
            }

            return apiResponse;
        }

        /// <summary>
        /// Notify all the listener when the doctor is available
        /// </summary>
        /// <param name="doctorId"></param>
        protected virtual void OnDoctorAvailable(string doctorId)
        {
            if (DoctorAvailable != null)
                DoctorAvailable(this, new DoctorEventArgs() { DoctorId = doctorId });
        }

        /// <summary>
        /// Notify to the patient that his appointment is coming 
        /// </summary>
        /// <param name="patientId">Patient id</param>
        protected virtual void OnAppointmentStarted(Appointment appointment)
        {
            if (AppointmentStarted != null)
                AppointmentStarted(this, new AppointmentEventArgs() 
                { 
                    AppointmentId = appointment.Id,
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId 
                });
        }
    }
}
