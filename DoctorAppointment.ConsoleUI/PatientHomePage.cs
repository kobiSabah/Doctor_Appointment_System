using DoctorAppointment.Core.Contracts.Responses;
using DoctorAppointment.Core.Models;
using DoctorAppointment.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorAppointment.ConsoleUI
{
    /// <summary>
    /// UI component for patient homepage
    /// </summary>
    public class PatientHomePage
    {
        private readonly ClinicManager r_ClinicService = ClinicManager.Instance;
        private readonly ScheduleManager r_ScheduleService = new ScheduleManager();
        private string m_PatientId;

        public PatientHomePage(string patientId)
        {
            r_ScheduleService.AppointmentStarted += OnAppointmentStart;
            m_PatientId = patientId;
        }

        public async Task show()
        {
            await showOptions();
        }

        private async Task showOptions()
        {
            string input = string.Empty;
            while (input != "4")
            {
                Console.WriteLine(@"
-------------------------------------
                Menu
-------------------------------------
1. Show a list of doctors.
2. Make an appointment.
3. Cancel an appointment.
4. Exit.");

                input = Console.ReadLine();
                if (validateUserChoice(input))
                {
                    switch (input)
                    {
                        case "1":
                            await showDoctors();
                            break;

                        case "2":
                            await createAppointment();
                            break;

                        case "3":
                            await cancleAppointment();
                            break;
                        case "4":
                            exit();
                            break;
                        default:
                            Console.WriteLine("Invalid input. Try again");
                            break;
                    }
                }
            }
        }

        private bool validateUserChoice(string input)
        {
            int userChoice;
            bool isvalid = int.TryParse(input, out userChoice);

            if (isvalid)
            {
                return userChoice > 0 && userChoice <= 3;
            }

            return false;
        }

        private async Task createAppointment()
        {
            AppointmentForm newAppointment = new AppointmentForm(m_PatientId);

            await r_ScheduleService.AddAppointmentAsync(await newAppointment.GetAppointmentFormAsync());
        }

        private async Task cancleAppointment()
        {
            Appointment appointmetToCancel = await getAllAppointmentsAsync();

            if (appointmetToCancel != null)
            {
                ApiResponse<object> apiResponse = await r_ScheduleService.RemoveFromWaitingList(appointmetToCancel.Id);

                if (!apiResponse.IsSuccess)
                {
                    foreach (string error in apiResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
        }

        private async Task<Appointment> getAllAppointmentsAsync()
        {
            ApiResponse<List<Appointment>> apiResponse = await r_ClinicService.GetPatientAppointmentsAsync(m_PatientId);

            if (apiResponse.IsSuccess)
            {
                List<Appointment> appointments = apiResponse.Context;
                if (appointments.Count > 0)
                {
                    Console.WriteLine("Choose appointment to cancel");
                    int index = 1;
                    foreach (Appointment appointment in appointments)
                    {
                        Console.WriteLine($"{index}. {appointment.Date}");
                    }

                    int input = int.Parse(Console.ReadLine());

                    return appointments[input - 1];
                }
                else
                {
                    Console.WriteLine("No appointments");
                    return null;
                }
            }
            else
            {
                foreach (string error in apiResponse.Errors)
                {
                    Console.WriteLine(error);
                }
                return null;
            }
        }

        private void exit()
        {
            Console.WriteLine(@"Logging out...");
        }

        private async Task showDoctors()
        {
            Console.WriteLine(@"
Would you like to filter the doctors by availability Y/N ?");

            string input = string.Empty;
            do
            {
                input = Console.ReadLine().ToUpper();
            } while (!validateFilterinput(input));

            ApiResponse<List<Doctor>> apiResponse = input == "Y" ? await r_ClinicService.GetAllDoctorsAsync(true) : await r_ClinicService.GetAllDoctorsAsync(false);
            if (apiResponse.IsSuccess)
            {
                List<Doctor> doctors = apiResponse.Context;
                int index = 1;
                foreach (Doctor doctor in doctors)
                {
                    Console.WriteLine($"{index}. {doctor}");
                    index++;
                }
            }
            else
            {
                foreach (string error in apiResponse.Errors)
                {
                    Console.WriteLine(error);
                }
            }
        }

        private bool validateFilterinput(string input)
        {
            if (input.Length > 1)
                return false;

            if (input == "Y" || input == "N")
                return true;

            return false;
        }

        private void OnAppointmentStart(object source, AppointmentEventArgs e)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            if (e.PatientId.Equals(m_PatientId))
                Console.WriteLine($"Your Meeting is about to start.");

            Console.ForegroundColor = currentColor;
        }
    }
}
