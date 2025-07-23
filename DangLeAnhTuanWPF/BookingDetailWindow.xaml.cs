using BLL.Services;
using DAL.Entities;
using System.Collections.Generic;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class BookingDetailWindow : Window
    {
        private int _bookingReservationId;
        private IBookingService _bookingService;
        private int _bookingStatus;
        public BookingDetailWindow()
        {
            InitializeComponent();
        }

        public void Initialize(IEnumerable<BookingDetail> bookingDetails, int bookingReservationId, IBookingService bookingService, byte bookingStatus = 0)
        {
            dgBookingDetails.ItemsSource = bookingDetails;
            _bookingReservationId = bookingReservationId;
            _bookingService = bookingService;
            _bookingStatus = bookingStatus;
            btnConfirm.Visibility = bookingService == null ? Visibility.Collapsed : Visibility.Visible;
            btnCancelled.Visibility = (bookingService == null || _bookingStatus != 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void btnConfirm_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_bookingService != null && _bookingReservationId > 0)
            {
                _bookingService.UpdateBookingStatus(_bookingReservationId, 1);
                MessageBox.Show("Booking confirmed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnCancelled_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_bookingService != null && _bookingReservationId > 0)
            {
                if (MessageBox.Show("Bạn có chắc muốn Cancelled booking này không?", "Xác nhận", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    _bookingService.UpdateBookingStatusAndRoom(_bookingReservationId, 2); // 2 = Cancelled, đồng thời set RoomStatus = 1
                    MessageBox.Show("Booking đã bị Cancelled và các phòng đã được mở lại!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }
    }
}