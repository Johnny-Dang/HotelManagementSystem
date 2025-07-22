using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public CustomerRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAll() => _context.Customers.ToList();

        public Customer GetById(int id) => _context.Customers.Find(id);

        public Customer GetByEmail(string email) => _context.Customers.FirstOrDefault(c => c.EmailAddress == email);

        public Customer GetByEmailAndPassword(string email, string password) =>
            _context.Customers.FirstOrDefault(c => c.EmailAddress == email && c.Password == password && c.CustomerStatus == 1);

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            var tracked = _context.Customers.Local.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (tracked != null)
            {
                _context.Entry(tracked).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Customer> Search(string keyword)
        {
            return _context.Customers
                .Where(c => c.CustomerFullName.Contains(keyword) || c.EmailAddress.Contains(keyword))
                .ToList();
        }
    }
} 