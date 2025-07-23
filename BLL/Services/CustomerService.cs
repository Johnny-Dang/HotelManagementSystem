using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll().ToList();
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            return _customerRepository.GetAll()
                .Where(c => c.CustomerFullName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                            c.EmailAddress.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void AddCustomer(Customer customer)
        {
            _customerRepository.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _customerRepository.Update(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            _customerRepository.Delete(customerId);
        }

        public void Update(Customer customer)
        {
            _customerRepository.Update(customer);
        }

        public bool IsEmailTaken(string email, int? existingCustomerId = null)
        {
            var existing = _customerRepository.GetByEmail(email);
            return existing != null && (existingCustomerId == null || existing.CustomerId != existingCustomerId);
        }
    }
}