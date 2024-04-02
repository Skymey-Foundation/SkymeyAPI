using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkymeyLib.Models.Users.Register
{
    public class RegisterModel
    {
        [JsonPropertyName("Email")]
        public string Email { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
