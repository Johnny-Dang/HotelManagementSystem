using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        public AuthService(ICustomerRepository customerRepository, string adminEmail, string adminPassword)
        {
            _customerRepository = customerRepository;
            _adminEmail = adminEmail;
            _adminPassword = adminPassword;
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
