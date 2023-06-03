using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser  user);


    }
}
