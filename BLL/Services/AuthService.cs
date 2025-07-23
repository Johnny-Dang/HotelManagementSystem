using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Configuration;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        public AuthService(ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _adminEmail = configuration["Admin:Email"];
            _adminPassword = configuration["Admin:Password"];
        }

        public string Login(string email, string password, out Customer customer)
        {
            customer = null;
            if (email == _adminEmail && password == _adminPassword)
                return "Admin";

            customer = _customerRepository.GetByEmailAndPassword(email, password);
            if (customer != null)
                return "Customer";

            return null;
        }
    }
}