using DoctorAppointment.Core.Contracts.Responses;
using DoctorAppointment.Core.Models;
using DoctorAppointment.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorAppointment.ConsoleUI
{
    /// <summary>
    /// UI component from representing appointment form
    /// </summary>
    public class AppointmentForm
    {
        private readonly ClinicManager r_ClinicService = ClinicManager.Instance;
        private readonly TimeSpan r_AppointmentTime = new TimeSpan(0, 0, 15);

        public AppointmentForm(string patientId)
        {
            PatientId = patientId;
        }

        public string AppointmentTime { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; private set; }

        public async Task<Appointment> GetAppointmentFormAsync()
        {
            await showFormAsync();
            return new Appointment
            {
                AppointmentTime = AppointmentTime,
                DoctorId = DoctorId,
                PatientId = PatientId
            };

        }

        private async Task showFormAsync()
        {

            Console.WriteLine(@"
Welcome to application form
First you need to pick a doctor
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

                Console.WriteLine(@"
Choose your doctor by typing the line number");

                do
                {
                    input = Console.ReadLine().ToUpper();
                } while (!validateDoctorChooseInput(input, doctors.Count));

                DoctorId = doctors[int.Parse(input) - 1].Id;
                AppointmentTime = r_AppointmentTime.ToString();

                Console.WriteLine(@"All done!
You are in the queue.");
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
            return input.Length <= 1 && (input == "Y" || input == "N");
        }

        private bool validateDoctorChooseInput(string input, int range)
        {
            int result;
            if (int.TryParse(input, out result))
            {
                if (result <= range && result >= 1)
                {
                    return true;
                }
            }

            Console.WriteLine("Invalid input. Try again");
            return false;
        }
    }
}
