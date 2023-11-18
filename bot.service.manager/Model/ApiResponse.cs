using System.Net;

namespace bot.service.manager.Model
{
    public class ApiResponse
    {
        public dynamic ResponseBody { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string HttpStatusMessage { get; set; }
        public ApiResponse()
        {

        }

        public static ApiResponse BuildResponse(dynamic data, HttpStatusCode httpStatusCode = HttpStatusCode.OK, string message = null)
        {
            ApiResponse apiResponse = new ApiResponse
            {
                HttpStatusCode = httpStatusCode,
                ResponseBody = data,
                HttpStatusMessage = message
            };
            return apiResponse;
        }

        public static ApiResponse BadRequest(dynamic data, string message = null)
        {
            ApiResponse apiResponse = new ApiResponse
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                ResponseBody = data,
                HttpStatusMessage = message
            };
            return apiResponse;
        }
    }
}
