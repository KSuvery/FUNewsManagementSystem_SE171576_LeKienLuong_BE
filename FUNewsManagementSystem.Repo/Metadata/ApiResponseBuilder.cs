using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repo.Metadata
{
    public class ApiResponseBuilder
    {
        // This method is used to build a response object for single data
        public static ApiResponse<T> BuildResponse<T>(int statusCode, string message, T data)
        {
            return new ApiResponse<T>
            {
                Message = message,
                StatusCode = statusCode,
                Data = data,
            };
        }
    }
}
