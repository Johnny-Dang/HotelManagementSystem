using DAL.Entities;
using System;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface IBookingRepository
    {
        IEnumerable<BookingReservation> GetByCustomerId(int customerId);
        void Add(BookingReservation reservation, List<BookingDetail> details);
        IEnumerable<BookingReservation> GetAllWithDetails();
        void UpdateBookingStatus(int bookingReservationId, byte status);
        IEnumerable<BookingDetail> GetBookingDetailsByReservationId(int bookingReservationId);
        bool HasBookingDetailWithRoom(int roomId);
    }
}