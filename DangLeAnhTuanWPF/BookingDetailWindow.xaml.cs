using DAL.Entities;
using System.Collections.Generic;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class BookingDetailWindow : Window
    {
        public BookingDetailWindow()
        {
            InitializeComponent();
        }

        public void Initialize(IEnumerable<BookingDetail> bookingDetails)
        {
            dgBookingDetails.ItemsSource = bookingDetails;
        }
    }
}