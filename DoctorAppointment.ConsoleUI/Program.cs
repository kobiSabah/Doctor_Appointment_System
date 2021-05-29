using DoctorAppointment.Core.Contracts.Responses;
using DoctorAppointment.Core.Services;
using System;
using System.Threading.Tasks;

namespace DoctorAppointment.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            IdentityManager identity = new IdentityManager();
            ClinicManager clinic = ClinicManager.Instance;

            LoginCredentials loginCredentials = null;

            string input = GetUserinput();

            switch (input)
            {
                case "1":
                    loginCredentials = await userRegister(identity);
                    break;
                case "2":
                    loginCredentials = await userLogin(identity);
                    break;
                default:
                    break;
            }



            if (loginCredentials.UserType == "Doctor")
            {
                await clinic.StartWork(loginCredentials.Token);
                DoctorHomePage homepage = new DoctorHomePage(loginCredentials.Token);

                await homepage.menu();
            }
            else
            {
                PatientHomePage homepage = new PatientHomePage(loginCredentials.Token);
                await homepage.show();
            }
        }

        private static async Task<LoginCredentials> userLogin(IdentityManager identity)
        {
            LoginCredentials loginCredentials = new LoginCredentials();
            UserLoginForm loginFrom = new UserLoginForm();
            ApiResponse<LoginCredentials> apiLoginResponse;
            do
            {
                loginFrom.Show();
                apiLoginResponse = await identity.LoginAsync(loginFrom.GetRequest());

                if (apiLoginResponse.IsSuccess)
                {
                    loginCredentials = apiLoginResponse.Context;
                }
                else
                {
                    foreach (string error in apiLoginResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            } while (!apiLoginResponse.IsSuccess);

            return loginCredentials;
        }

        private static async Task<LoginCredentials> userRegister(IdentityManager identity)
        {
            UserRegisterForm registerForm = new UserRegisterForm();
            LoginCredentials loginCredentials = null;
            ApiResponse<LoginCredentials> apiRegisterResponse;
            do
            {
                registerForm.Show();
                apiRegisterResponse = await identity.RegisterAsync(registerForm.GetRequest());
                if (apiRegisterResponse.IsSuccess)
                {
                    loginCredentials = apiRegisterResponse.Context;
                }
                else
                {
                    foreach (string error in apiRegisterResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            } while (!apiRegisterResponse.IsSuccess);

            return loginCredentials;
        }

        private static string GetUserinput()
        {
            string input = string.Empty;
            Console.WriteLine(
@"
Welcome User
1. Register.
2  Login.
");
            do
            {
                input = Console.ReadLine();
            } while (input != "1" && input != "2");

            return input;
        }

    }
}
