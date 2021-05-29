using DoctorAppointment.Core.Contracts.Responses;
using DoctorAppointment.Core.Models;
using DoctorAppointment.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorAppointment.ConsoleUI
{
    /// <summary>
    /// UI component for doctor homepage
    /// </summary>
    public class DoctorHomePage
    {
        private readonly ClinicManager r_ClinicManager;
        private readonly string r_Id;
        public DoctorHomePage(string id)
        {
            r_ClinicManager = ClinicManager.Instance;
            r_Id = id;
        }

        public async Task menu()
        {
            string input = string.Empty;

            while (input != "2")
            {
                Console.WriteLine(@"
Welcome Doctor
What would you like to do ? 
1. Show waiting list.
2. Exit.");

                input = Console.ReadLine();

                if (input == "1")
                {
                    ApiResponse<List<Patient>> apiResponse = await r_ClinicManager.GetWatingListPatientsAsync(r_Id);

                    if (apiResponse.IsSuccess)
                    {
                        List<Patient> patients = apiResponse.Context;
                        Console.WriteLine(@"
    --------------------
        Waiting List
    --------------------");
                        int index = 1;
                        if (patients.Count > 0)
                        {
                            foreach (Patient patient in patients)
                            {
                                Console.WriteLine($"{index}. {patient.FirstName} {patient.LastName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Appointments.");
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
                else if (input != "2")
                {
                    Console.WriteLine("invalid input. Try again.");
                }

            }
        }
    }
}
