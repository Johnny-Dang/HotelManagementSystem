using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Services
{
    public interface IRoomService
    {
        List<RoomInformation> GetAllRooms();
        List<RoomInformation> SearchRooms(string keyword);
        void AddRoom(RoomInformation room);
        void UpdateRoom(RoomInformation room);
        void DeleteRoom(int roomId);
        List<RoomType> GetRoomTypes();
    }
}