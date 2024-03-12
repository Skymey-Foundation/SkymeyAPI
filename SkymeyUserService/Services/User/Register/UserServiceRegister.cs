using Microsoft.EntityFrameworkCore;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Auth;
using SkymeyUserService.Interfaces.Users.Register;

namespace SkymeyUserService.Services.User.Auth
{
    public class UserServiceRegister : IUserServiceRegister
    {
        public async Task <bool> IsValidUserInformation(RegisterModel registerModel, ApplicationContext _db)
        {
            if (await _db.USR_001.Where(x => x.Email == registerModel.Email).FirstOrDefaultAsync() == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
