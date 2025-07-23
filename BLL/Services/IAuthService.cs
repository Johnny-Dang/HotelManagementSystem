using DAL.Entities;

namespace BLL.Services
{
    public interface IAuthService
    {
        string Login(string email, string password, out Customer customer);
    }
}