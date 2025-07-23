using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public BookingRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<BookingReservation> GetByCustomerId(int customerId)
        {
            return _context.BookingReservations
                .Include(br => br.BookingDetails)
                .ThenInclude(bd => bd.Room)
                .ThenInclude(r => r.RoomType)
                .Where(br => br.CustomerId == customerId)
                .ToList();
        }

        public void Add(BookingReservation reservation, List<BookingDetail> details)
        {
            // Tự sinh BookingReservationId nếu database không tự động tăng
            int maxId = 1;
            if (_context.BookingReservations.Any())
            {
                maxId = _context.BookingReservations.Max(b => b.BookingReservationId) + 1;
            }
            reservation.BookingReservationId = maxId;
            // Gán BookingReservationId cho từng BookingDetail
            foreach (var detail in details)
            {
                detail.BookingReservationId = reservation.BookingReservationId;
            }
            reservation.BookingDetails = details;
            _context.BookingReservations.Add(reservation);
            _context.SaveChanges();
        }

        public IEnumerable<BookingReservation> GetAllWithDetails()
        {
            return _context.BookingReservations
                .Include(br => br.Customer)
                .Include(br => br.BookingDetails)
                .ThenInclude(bd => bd.Room)
                .ThenInclude(r => r.RoomType)
                .ToList();
        }

        public void UpdateBookingStatus(int bookingReservationId, byte status)
        {
            var booking = _context.BookingReservations.FirstOrDefault(b => b.BookingReservationId == bookingReservationId);
            if (booking != null)
            {
                booking.BookingStatus = status;
                _context.SaveChanges();
            }
        }

        public IEnumerable<BookingDetail> GetBookingDetailsByReservationId(int bookingReservationId)
        {
            return _context.BookingDetails
                .Include(bd => bd.Room)
                .ThenInclude(r => r.RoomType)
                .Where(bd => bd.BookingReservationId == bookingReservationId)
                .ToList();
        }

        public bool HasBookingDetailWithRoom(int roomId)
        {
            return _context.BookingDetails.Any(bd => bd.RoomId == roomId);
        }
    }
}