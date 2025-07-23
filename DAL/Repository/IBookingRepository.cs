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
    }
}