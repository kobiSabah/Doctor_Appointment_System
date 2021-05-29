using DoctorAppointment.Core.Contracts.Requests;
using System;

namespace DoctorAppointment.ConsoleUI
{
    /// <summary>
    /// UI component for users login
    /// </summary>
    public class UserLoginForm
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public void Show()
        {
            getUserEmail();
            getUserPassword();
        }

        private void getUserEmail()
        {
            Console.WriteLine(@"
Welcome to Login from !
Type your email address : ");

            Email = Console.ReadLine();

        }

        private void getUserPassword()
        {
            Console.WriteLine(@"
Type your password : ");

            Password = Console.ReadLine();
        }

        public UserLoginRequest GetRequest()
        {
            return new UserLoginRequest()
            {
                Email = Email,
                Password = Password
            };
        }

    }
}
