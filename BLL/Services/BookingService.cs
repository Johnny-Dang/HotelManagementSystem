using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public List<BookingReservation> GetBookingHistory(int customerId)
        {
            return _bookingRepository.GetByCustomerId(customerId).ToList();
        }

        public void SaveBooking(BookingReservation reservation, List<BookingDetail> details)
        {
            _bookingRepository.Add(reservation, details);

            // Cập nhật RoomStatus = 0 cho các phòng vừa đặt
            using (var context = new DAL.Entities.FuminiHotelManagementContext())
            {
                foreach (var detail in details)
                {
                    var room = context.RoomInformations.FirstOrDefault(r => r.RoomId == detail.RoomId);
                    if (room != null)
                    {
                        room.RoomStatus = 0;
                    }
                }
                context.SaveChanges();
            }
        }

        public List<BookingReservation> GetAllBookingsWithDetails()
        {
            return _bookingRepository.GetAllWithDetails().ToList();
        }

        public List<(string RoomNumber, int TotalBookings, decimal TotalRevenue)> GenerateBookingReport(DateOnly startDate, DateOnly endDate)
        {
            var report = _bookingRepository.GetAllWithDetails()
                .SelectMany(b => b.BookingDetails)
                .Where(d => d.StartDate >= startDate && d.EndDate <= endDate)
                .GroupBy(d => d.Room.RoomNumber)
                .Select(g => (
                    RoomNumber: g.Key,
                    TotalBookings: g.Count(),
                    TotalRevenue: g.Sum(d => d.ActualPrice ?? 0)
                ))
                .ToList();

            return report;
        }
    }
}