using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace DangLeAnhTuanWPF
{
    public partial class BookingHistoryControl : UserControl
    {
        public BookingHistoryControl(Customer customer)
        {
            InitializeComponent();
            var context = new FuminiHotelManagementContext();
            // Lấy danh sách booking của customer, bao gồm cả BookingDetails nếu cần
            var bookings = context.BookingReservations
                .Where(b => b.CustomerId == customer.CustomerId)
                .Include(b => b.BookingDetails)
                .ToList();
            dgBookingHistory.ItemsSource = new ObservableCollection<BookingReservation>(bookings);
        }
    }
} 