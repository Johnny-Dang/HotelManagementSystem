using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        List<Customer> SearchCustomers(string keyword);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerId);
        void Update(Customer customer); // For CustomerWindow
        bool IsEmailTaken(string email, int? existingCustomerId = null);
    }
}