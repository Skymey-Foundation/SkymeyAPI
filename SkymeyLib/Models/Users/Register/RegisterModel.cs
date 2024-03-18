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
        [Required]
        [JsonPropertyName("Email")]
        public required string Email { get; set; }
        [Required]
        [JsonPropertyName("Password")]
        public required string Password { get; set; }
    }
}
