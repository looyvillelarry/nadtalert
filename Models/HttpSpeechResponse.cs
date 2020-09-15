using System.Net;

namespace NADT.Models
{
    public class HttpSpeechResponse
    {
        public HttpStatusCode Code { get; set; }
        public string Path { get; set; }
    }
}