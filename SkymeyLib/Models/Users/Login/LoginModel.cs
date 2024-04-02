using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation;

namespace SkymeyLib.Models.Users.Login
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
