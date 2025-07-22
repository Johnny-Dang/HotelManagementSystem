using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Services
{
    public interface IRoomService
    {
        IEnumerable<RoomInformation> GetAllRooms();
        RoomInformation GetRoomById(int id);
        void AddRoom(RoomInformation room);
        void UpdateRoom(RoomInformation room);
        void DeleteRoom(int id);
        IEnumerable<RoomInformation> SearchRooms(string keyword);
    }
} 