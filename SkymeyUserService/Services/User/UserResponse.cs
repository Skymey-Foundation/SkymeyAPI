using SkymeyLib.Models.Users.Login;
using SkymeyUserService.Interfaces.Users.Register;
using System.Net;
using System.Text.Json.Serialization;

namespace SkymeyUserService.Services.User
{
    public class UserResponse : IUserResponse
    {
        [JsonPropertyName("ResponseType")]
        public bool ResponseType { get; set; }
        [JsonPropertyName("Response")]
        public string Response { get; set; }
        [JsonPropertyName("AuthenticatedResponses")]
        public required AuthenticatedResponse AuthenticatedResponses { get; set; }
        [JsonPropertyName("StatusCode")]
        public required HttpStatusCode StatusCode { get; set; }

        public void Dispose()
        {
            
        }
    }
}
