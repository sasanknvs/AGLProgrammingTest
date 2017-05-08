using System.Net;

namespace AGL.Models.EntityModels
{
    public class Error
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
