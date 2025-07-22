using DAL.Entities;
using System.Collections.Generic;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class BookingDetailWindow : Window
    {
        public BookingDetailWindow(IEnumerable<BookingDetail> details)
        {
            InitializeComponent();
            dgBookingDetails.ItemsSource = details;
        }
    }
} 