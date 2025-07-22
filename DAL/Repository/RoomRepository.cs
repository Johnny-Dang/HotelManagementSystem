using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public RoomRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomInformation> GetAllWithRoomType()
        {
            return _context.RoomInformations.Include(r => r.RoomType).ToList();
        }

        public RoomInformation GetById(int id)
        {
            return _context.RoomInformations.Include(r => r.RoomType).FirstOrDefault(r => r.RoomId == id);
        }

        public void Add(RoomInformation room)
        {
            _context.RoomInformations.Add(room);
            _context.SaveChanges();
        }

        public void Update(RoomInformation room)
        {
            _context.RoomInformations.Update(room);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var room = _context.RoomInformations.Find(id);
            if (room != null)
            {
                _context.RoomInformations.Remove(room);
                _context.SaveChanges();
            }
        }

        public IEnumerable<RoomInformation> Search(string keyword)
        {
            return _context.RoomInformations
                .Include(r => r.RoomType)
                .Where(r => r.RoomNumber.Contains(keyword) || r.RoomDetailDescription.Contains(keyword))
                .ToList();
        }

        public IEnumerable<RoomInformation> GetAllRooms()
        {
            return _context.RoomInformations
                .Include(r => r.RoomType)
                .ToList();
        }
    }
} 