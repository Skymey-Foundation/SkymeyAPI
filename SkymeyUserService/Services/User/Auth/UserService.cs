using Microsoft.EntityFrameworkCore;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Data;
using SkymeyUserService.Interfaces.Users.Auth;

namespace SkymeyUserService.Services.User.Auth
{
    public class UserService : IUserService
    {
        public string GetUserDetails()
        {
            return "ok";
        }

        public async Task<bool> IsValidUserInformation(LoginModel model, ApplicationContext _db)
        {
            if (await _db.USR_001.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefaultAsync() != null)
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
