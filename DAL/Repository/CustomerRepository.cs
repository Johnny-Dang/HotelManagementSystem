using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public CustomerRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public Customer GetByEmail(string email)
        {
            return _context.Customers
                .FirstOrDefault(c => c.EmailAddress == email);
        }

        public Customer GetByEmailAndPassword(string email, string password)
        {
            return _context.Customers
                .FirstOrDefault(c => c.EmailAddress == email && c.Password == password);
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            var existing = _context.Customers.Find(customer.CustomerId);
            if (existing != null)
            {
                existing.CustomerFullName = customer.CustomerFullName;
                existing.Telephone = customer.Telephone;
                existing.EmailAddress = customer.EmailAddress;
                existing.CustomerBirthday = customer.CustomerBirthday;
                existing.CustomerStatus = customer.CustomerStatus;
                existing.Password = customer.Password;
                _context.SaveChanges();
            }
        }

        public void Delete(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public bool IsEmailTaken(string email, int? existingCustomerId = null)
        {
            var customer = GetByEmail(email);
            return customer != null && (existingCustomerId == null || customer.CustomerId != existingCustomerId);
        }
    }
}