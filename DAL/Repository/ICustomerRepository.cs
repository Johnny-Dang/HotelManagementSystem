using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repository
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer GetById(int id);
        Customer GetByEmail(string email);
        Customer GetByEmailAndPassword(string email, string password);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        IEnumerable<Customer> Search(string keyword);
    }
} 