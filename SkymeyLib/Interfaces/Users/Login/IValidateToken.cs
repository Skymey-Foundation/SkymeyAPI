using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkymeyLib.Interfaces.Users.Login
{
    public interface IValidateToken
    {
        [JsonPropertyName("Token")]
        public string? Token { get; set; }
        [JsonPropertyName("RefreshToken")]
        public string? RefreshToken { get; set; }
    }
}
