using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Models;

namespace ECommerce.src.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, string role);
    }
}