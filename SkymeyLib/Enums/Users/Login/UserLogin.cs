using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Enums.Users
{
    public enum UserLogin
    {
        [StringValue("/api/SkymeyUserService/Login")]
        LoginUrl,
        [StringValue("Please pass the valid Email and Password")]
        LoginAndPassword,
        [StringValue("Ok")]
        Ok,
        [StringValue("Unauthorized")]
        Unauthorized
    }
}
