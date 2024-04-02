using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyLib.Models.Users.Login
{
    public interface IValidateEmail
    {
        Task<bool> CheckIfUnique(string email, CancellationToken cancellationToken);
    }
}
