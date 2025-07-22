using DAL.Entities;
using DAL.Repository;
using System.Collections.Generic;

namespace BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService() : this(new RoomRepository(new FuminiHotelManagementContext()))
        {
        }

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IEnumerable<RoomInformation> GetAllRooms() => _roomRepository.GetAllRooms();

        public RoomInformation GetRoomById(int id) => _roomRepository.GetById(id);

        public void AddRoom(RoomInformation room) => _roomRepository.Add(room);

        public void UpdateRoom(RoomInformation room) => _roomRepository.Update(room);

        public void DeleteRoom(int id) => _roomRepository.Delete(id);

        public IEnumerable<RoomInformation> SearchRooms(string keyword) => _roomRepository.Search(keyword);

    }
} 