using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        IEnumerable<Customer> SearchCustomers(string keyword);
    }
} 