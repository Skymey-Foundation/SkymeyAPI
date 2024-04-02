using SkymeyLib.Models.Users.Login;
using System.Net;
using System.Text.Json.Serialization;

namespace SkymeyLib.Models.Users
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
