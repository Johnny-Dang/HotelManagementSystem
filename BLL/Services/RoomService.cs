using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public List<RoomInformation> GetAllRooms()
        {
            return _roomRepository.GetAllWithRoomType().ToList();
        }

        public List<RoomInformation> SearchRooms(string keyword)
        {
            return _roomRepository.Search(keyword).ToList();
        }

        public void AddRoom(RoomInformation room)
        {
            _roomRepository.Add(room);
        }

        public void UpdateRoom(RoomInformation room)
        {
            _roomRepository.Update(room);
        }

        public void DeleteRoom(int roomId)
        {
            _roomRepository.Delete(roomId);
        }

        public List<RoomType> GetRoomTypes()
        {
            return _roomRepository.GetRoomTypes().ToList();
        }
    }
}