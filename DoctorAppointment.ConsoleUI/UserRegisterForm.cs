using DoctorAppointment.Core.Contracts.Requests;
using System;

namespace DoctorAppointment.ConsoleUI
{
    /// <summary>
    /// UI component for registration
    /// </summary>
    public class UserRegisterForm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }

        public void Show()
        {
            getUserFullName();
            getUserType();
            getEmail();
            getUsername();
            getUserPassword();
        }

        private void getUserFullName()
        {
            Console.WriteLine(@"
Welcome to doctor register form !
Type your full name in English:");
            string input = Console.ReadLine();

            string[] fullname = input.Split(' ');
            FirstName = fullname[0];
            LastName = fullname[1];

        }

        private void getUsername()
        {
            Console.WriteLine(@"
Type user username : ");

            UserName = Console.ReadLine();
        }
        private void getEmail()
        {
            Console.WriteLine(@"
Type your email address : ");

            string input = Console.ReadLine();
            Email = input;
        }

        private void getUserPassword()
        {
            Console.WriteLine(@"
Type your password:");
            Password = Console.ReadLine();
        }

        private void getUserType()
        {
            Console.WriteLine(@"
What kind of user you are ?
1. Doctor.
2. Patient");

            string input = Console.ReadLine();
            UserType = input == "1" ? "Doctor" : "Patient";
        }

        public UserRegisterRequest GetRequest()
        {
            return new UserRegisterRequest
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                UserType = UserType,
                Username = UserName
            };
        }
    }
}
