using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IBookingService
    {
        IEnumerable<BookingReservation> GetAllBookingsWithDetails();

        void SaveBooking(BookingReservation reservation, List<BookingDetail> details);
    }
}
