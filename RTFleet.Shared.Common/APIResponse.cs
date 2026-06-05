using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTFleet.Shared.Common
{
    public class APIResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public T Data { get; set; }
        public object Errors { get; set; }
        public DateTime TimeStamp { get; set; }

        public APIResponse(T data, bool isSuccess = true, string message = null, string code = "200", object errors = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Code = code;
            Data = data;
            Errors = errors;
            TimeStamp = DateTime.Now;
        }

        public static APIResponse<T> Success(T data, string message = null, string code = "200")
        {
            return new APIResponse<T>(data, true, message, code);
        }

        public static APIResponse<T> Failure(string message, string code = "400", object errors = null)
        {
            return new APIResponse<T>(default, false, message, code, errors);
        }
    }
}
