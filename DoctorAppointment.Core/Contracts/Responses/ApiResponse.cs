﻿using System.Collections.Generic;

namespace DoctorAppointment.Core.Contracts.Responses
{
    public class ApiResponse<T> where T : class, new()
    {
        public ApiResponse()
        {
            Context = new T();
        }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public T Context { get; set; }
    }
}
