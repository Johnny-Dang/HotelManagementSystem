using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IAuthService
    {
        string Login(string email, string password, out Customer customer);
    }
}
