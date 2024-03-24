using SkymeyLib.Interfaces.Users.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkymeyLib.Models.Users
{
    public class ValidateTokenInfo : IValidateToken
    {
        public ValidateTokenInfo() {
        
        }
        [JsonPropertyName("Token")]
        public string? Token { get; set; }
        [JsonPropertyName("RefreshToken")]
        public string? RefreshToken { get; set; }
    }
}
