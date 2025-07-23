using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public RoomRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomInformation> GetAll()
        {
            return _context.RoomInformations
                .Include(r => r.RoomType)
                .ToList();
        }

        public IEnumerable<RoomInformation> GetAllWithRoomType()
        {
            return _context.RoomInformations
                .Include(r => r.RoomType)
                .ToList();
        }

        public RoomInformation GetById(int roomId)
        {
            return _context.RoomInformations
                .Include(r => r.RoomType)
                .FirstOrDefault(r => r.RoomId == roomId);
        }

        public IEnumerable<RoomInformation> Search(string keyword)
        {
            return _context.RoomInformations
                .Include(r => r.RoomType)
                .Where(r => r.RoomNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                            r.RoomDetailDescription.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
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

        public void Delete(int roomId)
        {
            var room = _context.RoomInformations.Find(roomId);
            if (room != null)
            {
                _context.RoomInformations.Remove(room);
                _context.SaveChanges();
            }
        }

        public IEnumerable<RoomType> GetRoomTypes()
        {
            return _context.RoomTypes.ToList();
        }
    }
}