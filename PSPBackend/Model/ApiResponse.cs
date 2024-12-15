using System.Net;

namespace PSPBackend.Model
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new();
        public object Data { get; set; }
    }
}
