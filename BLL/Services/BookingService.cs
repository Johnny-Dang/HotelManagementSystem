using DAL.Entities;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService() : this(new BookingRepository(new FuminiHotelManagementContext()))
        {
        }

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public IEnumerable<BookingReservation> GetAllBookingsWithDetails() => _bookingRepository.GetAllWithDetails();

        public BookingReservation AddAndReturn(BookingReservation reservation) => _bookingRepository.AddAndReturn(reservation);

        public void Update(BookingReservation reservation) => _bookingRepository.Update(reservation);

        public void AddDetail(BookingDetail detail) => _bookingRepository.AddDetail(detail);

        public void UpdateDetail(BookingDetail detail) => _bookingRepository.UpdateDetail(detail);

        public void DeleteDetail(BookingDetail detail) => _bookingRepository.DeleteDetail(detail);

        public void SaveBooking(BookingReservation reservation, List<BookingDetail> details)
        {
            // Assign the next BookingReservationId
            reservation.BookingReservationId = _bookingRepository.GetNextBookingReservationId();

            // Assign BookingReservationId to all BookingDetails
            foreach (var detail in details)
            {
                detail.BookingReservationId = reservation.BookingReservationId;
            }

            // Save BookingReservation to database
            _bookingRepository.AddBookingReservation(reservation);

            // Save BookingDetails to database
            _bookingRepository.AddBookingDetails(details);
        }
    }
} 