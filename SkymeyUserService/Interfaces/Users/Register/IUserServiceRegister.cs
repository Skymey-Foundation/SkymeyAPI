using SkymeyLib.Models.Users.Login;
using SkymeyLib.Models.Users.Register;
using SkymeyUserService.Data;

namespace SkymeyUserService.Interfaces.Users.Register
{
    public interface IUserServiceRegister
    {
        Task<bool> IsValidUserInformation(RegisterModel registerModel, ApplicationContext _db);
    }
}
