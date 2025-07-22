using DAL.Entities;
using DAL.Repository;
using System.Collections.Generic;

namespace BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService() : this(new CustomerRepository(new FuminiHotelManagementContext()))
        {
        }

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAllCustomers() => _customerRepository.GetAll();

        public Customer GetCustomerById(int id) => _customerRepository.GetById(id);

        public void AddCustomer(Customer customer) => _customerRepository.Add(customer);

        public void UpdateCustomer(Customer customer) => _customerRepository.Update(customer);

        public void DeleteCustomer(int id) => _customerRepository.Delete(id);

        public IEnumerable<Customer> SearchCustomers(string keyword) => _customerRepository.Search(keyword);
    }
} 