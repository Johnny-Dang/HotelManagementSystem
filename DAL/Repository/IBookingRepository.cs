using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repository
{
    public interface IBookingRepository
    {
        IEnumerable<BookingReservation> GetAllWithDetails();
        BookingReservation AddAndReturn(BookingReservation reservation);
        void Update(BookingReservation reservation);
        void SaveBooking(BookingReservation reservation, IEnumerable<BookingDetail> details);
        void UpdateDetail(BookingDetail detail);
        void DeleteDetail(BookingDetail detail);
        void AddDetail(BookingDetail detail);

        int GetNextBookingReservationId();
        void AddBookingReservation(BookingReservation reservation);
        void AddBookingDetails(List<BookingDetail> details);
    }
} 