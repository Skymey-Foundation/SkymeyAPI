using System.Net;

namespace SkymeyLib.Models.Users
{
    public interface IUserResponse : IUserResponseJWT, IDisposable
    {
        public bool ResponseType { get; set; }
        public string Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
