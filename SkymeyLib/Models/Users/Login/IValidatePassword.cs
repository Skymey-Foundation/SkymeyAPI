using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Models.Users.Login
{
    public interface IValidatePassword
    {
        Task<bool> CheckIfUnique2(string password, CancellationToken cancellationToken);
    }
}
