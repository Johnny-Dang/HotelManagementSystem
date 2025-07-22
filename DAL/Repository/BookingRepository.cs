using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public BookingRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<BookingReservation> GetAllWithDetails()
        {
            return _context.BookingReservations
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .ToList();
        }

        public BookingReservation AddAndReturn(BookingReservation reservation)
        {
            _context.BookingReservations.Add(reservation);
            _context.SaveChanges();
            return reservation;
        }

        public void Update(BookingReservation reservation)
        {
            _context.BookingReservations.Update(reservation);
            _context.SaveChanges();
        }

        public void AddDetail(BookingDetail detail)
        {
            _context.BookingDetails.Add(detail);
            _context.SaveChanges();
        }

        public void UpdateDetail(BookingDetail detail)
        {
            _context.BookingDetails.Update(detail);
            _context.SaveChanges();
        }

        public void DeleteDetail(BookingDetail detail)
        {
            _context.BookingDetails.Remove(detail);
            _context.SaveChanges();

            var remainingDetails = _context.BookingDetails.Count(d => d.BookingReservationId == detail.BookingReservationId);
            if (remainingDetails == 0)
            {
                var reservation = _context.BookingReservations.Find(detail.BookingReservationId);
                if (reservation != null)
                {
                    _context.BookingReservations.Remove(reservation);
                    _context.SaveChanges();
                }
            }
        }

        public void SaveBooking(BookingReservation reservation, IEnumerable<BookingDetail> details)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //reservation.BookingReservationId = _context.BookingReservations.Max(d => d.BookingReservationId);
                    var obj = _context.BookingReservations.Add(reservation);
                    _context.SaveChanges();

                    foreach (var detail in details)
                    {
                        detail.BookingReservationId = reservation.BookingReservationId;
                        _context.BookingDetails.Add(detail);
                    }
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public int GetNextBookingReservationId()
        {
            return _context.BookingReservations.Any()
                ? _context.BookingReservations.Max(x => x.BookingReservationId) + 1
                : 1;
        }

        public void AddBookingReservation(BookingReservation reservation)
        {
            _context.BookingReservations.Add(reservation);
            _context.SaveChanges();
        }

        public void AddBookingDetails(List<BookingDetail> details)
        {
            _context.BookingDetails.AddRange(details);
            _context.SaveChanges();
        }
    }
} 