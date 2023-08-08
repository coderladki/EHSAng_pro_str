using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Server.Web.Api.DataObjects
{
    public class ApiResponse<T>
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public string Status { get; set; } = "ok";
        public T Result { get; set; }

        //public static implicit operator ApiResponse<T>(ApiResponse<List<IssueTypeDto>> v)
        //{
        //    throw new NotImplementedException();
        //}
        public ApiResponse()
        {
        }
        public ApiResponse(string status, string message,T result)
        {
            Status = status;
            Message = message;
            Result = result;
        }



    }
}
