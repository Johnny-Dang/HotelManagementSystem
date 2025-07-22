using DAL.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using BLL.Services;
using System;
using System.Linq;

namespace DangLeAnhTuanWPF
{
    public partial class CustomerWindow : Window
    {
        private readonly Customer _customer;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly ObservableCollection<BookingDetail> _cart = new ObservableCollection<BookingDetail>();
        private BookingReservation _currentReservation = null;

        public CustomerWindow(Customer customer, IRoomService roomService, IBookingService bookingService)
        {
            InitializeComponent();
            _customer = customer;
            _roomService = roomService;
            _bookingService = bookingService;
            LoadRooms();
            dgCart.ItemsSource = _cart;
        }

        private void LoadRooms()
        {
            var rooms = _roomService.GetAllRooms();
            dgRooms.ItemsSource = new ObservableCollection<RoomInformation>(rooms);
        }

        private void BookingRoom_Click(object sender, RoutedEventArgs e)
        {
            var room = (sender as FrameworkElement)?.DataContext as RoomInformation;
            if (room == null) return;

            var bookingForm = new BookingFormWindow(room);
            if (bookingForm.ShowDialog() == true)
            {
                // If no reservation exists, create a new one
                if (_currentReservation == null)
                {
                    _currentReservation = new BookingReservation
                    {
                        BookingDate = DateOnly.FromDateTime(DateTime.Now),
                        TotalPrice = 0,
                        BookingStatus = 0,
                        CustomerId = _customer.CustomerId,
                        Customer = _customer,
                        BookingDetails = new System.Collections.Generic.List<BookingDetail>()
                    };
                }

                // Calculate number of days for ActualPrice
                int days = (bookingForm.EndDate.ToDateTime(TimeOnly.MinValue) -
                           bookingForm.StartDate.ToDateTime(TimeOnly.MinValue)).Days;

                // Create BookingDetail
                var detail = new BookingDetail
                {
                    RoomId = room.RoomId,
                    Room = room,
                    StartDate = bookingForm.StartDate,
                    EndDate = bookingForm.EndDate,
                    ActualPrice = room.RoomPricePerDay * days
                };

                // Add to cart and update reservation
                _cart.Add(detail);
                _currentReservation.TotalPrice += detail.ActualPrice ?? 0;
                _currentReservation.BookingDetails.Add(detail);
            }
        }

        private void ConfirmBooking_Click(object sender, RoutedEventArgs e)
        {
            if (_currentReservation == null || !_cart.Any())
            {
                MessageBox.Show("Cart is empty!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _bookingService.SaveBooking(_currentReservation, _cart.ToList());
                MessageBox.Show("Booking successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _cart.Clear();
                _currentReservation = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCart_Click(object sender, RoutedEventArgs e)
        {
            var detail = (sender as FrameworkElement)?.DataContext as BookingDetail;
            if (detail == null) return;

            var room = detail.Room;
            var bookingForm = new BookingFormWindow(room, detail.StartDate, detail.EndDate);
            if (bookingForm.ShowDialog() == true)
            {
                // Calculate number of days
                int days = (bookingForm.EndDate.ToDateTime(TimeOnly.MinValue) -
                           bookingForm.StartDate.ToDateTime(TimeOnly.MinValue)).Days;

                // Update the existing BookingDetail
                _currentReservation.TotalPrice -= detail.ActualPrice ?? 0;
                detail.StartDate = bookingForm.StartDate;
                detail.EndDate = bookingForm.EndDate;
                detail.ActualPrice = room.RoomPricePerDay * days;
                _currentReservation.TotalPrice += detail.ActualPrice ?? 0;

                // Refresh the cart display
                dgCart.Items.Refresh();
            }
        }

        private void DeleteCart_Click(object sender, RoutedEventArgs e)
        {
            var detail = (sender as FrameworkElement)?.DataContext as BookingDetail;
            if (detail == null) return;

            // Remove from cart and update TotalPrice
            _cart.Remove(detail);
            _currentReservation.BookingDetails.Remove(detail);
            _currentReservation.TotalPrice -= detail.ActualPrice ?? 0;

            // If cart is empty, reset reservation
            if (!_cart.Any())
            {
                _currentReservation = null;
            }

            // Refresh the cart display
            dgCart.Items.Refresh();
        }
    }
}