using DAL.Entities;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public interface IBookingService
    {
        List<BookingReservation> GetBookingHistory(int customerId);
        void SaveBooking(BookingReservation reservation, List<BookingDetail> details);
        List<BookingReservation> GetAllBookingsWithDetails();
        List<(string RoomNumber, int TotalBookings, decimal TotalRevenue)> GenerateBookingReport(DateOnly startDate, DateOnly endDate);
    }
}