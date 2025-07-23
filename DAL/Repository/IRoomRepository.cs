using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface IRoomRepository
    {
        IEnumerable<RoomInformation> GetAll();
        IEnumerable<RoomInformation> GetAllWithRoomType();
        RoomInformation GetById(int roomId);
        IEnumerable<RoomInformation> Search(string keyword);
        void Add(RoomInformation room);
        void Update(RoomInformation room);
        void Delete(int roomId);
        IEnumerable<RoomType> GetRoomTypes();
    }
}