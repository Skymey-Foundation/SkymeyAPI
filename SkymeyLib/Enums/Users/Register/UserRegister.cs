using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Enums.Users.Register
{
    public enum UserRegister
    {
        [StringValue("User already exist")]
        AlreadyExist,
        [StringValue("Ok")]
        Ok
    }
}
