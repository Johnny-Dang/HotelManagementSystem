using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repository
{
    public interface IRoomRepository
    {
        IEnumerable<RoomInformation> GetAllWithRoomType();
        RoomInformation GetById(int id);
        void Add(RoomInformation room);
        void Update(RoomInformation room);
        void Delete(int id);
        IEnumerable<RoomInformation> Search(string keyword);

        IEnumerable<RoomInformation> GetAllRooms();
    }
} 