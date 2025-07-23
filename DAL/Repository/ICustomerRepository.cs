using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer GetByEmail(string email);
        Customer GetByEmailAndPassword(string email, string password); // Added for login
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int customerId);
        bool IsEmailTaken(string email, int? existingCustomerId = null);
    }
}