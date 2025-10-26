using System.Net;

namespace kebabCards.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Errors = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public object Result { get; set; }
        public string ReturnMessage { get; set; }
    }
}
