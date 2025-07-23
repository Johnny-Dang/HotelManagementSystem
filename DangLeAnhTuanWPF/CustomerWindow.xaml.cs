using BLL.Services;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DangLeAnhTuanWPF
{
    public partial class CustomerWindow : Window
    {
        private Customer _customer;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly ICustomerService _customerService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ObservableCollection<BookingDetail> _cart = new ObservableCollection<BookingDetail>();
        private BookingReservation _currentReservation = null;

        public CustomerWindow(IRoomService roomService, IBookingService bookingService, ICustomerService customerService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _roomService = roomService;
            _bookingService = bookingService;
            _customerService = customerService;
            _serviceProvider = serviceProvider;
            dgCart.ItemsSource = _cart;
        }

        public void Initialize(Customer customer)
        {
            if (customer == null)
            {
                MessageBox.Show("Customer data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            _customer = customer;
            LoadRooms();
            LoadBookingHistory();
            LoadProfile();
        }

        private void LoadRooms()
        {
            var rooms = _roomService.GetAllRooms();
            dgRooms.ItemsSource = new ObservableCollection<RoomInformation>(rooms);
        }

        private void LoadBookingHistory()
        {
            var history = _bookingService.GetBookingHistory(_customer.CustomerId);
            dgBookingHistory.ItemsSource = new ObservableCollection<BookingReservation>(history);
        }

        private void LoadProfile()
        {
            txtFullName.Text = _customer.CustomerFullName ?? string.Empty;
            txtEmail.Text = _customer.EmailAddress ?? string.Empty;
            txtTelephone.Text = _customer.Telephone ?? string.Empty;
            dpBirthday.SelectedDate = _customer.CustomerBirthday?.ToDateTime(TimeOnly.MinValue);
            txtPassword.Clear(); // Password is not loaded for security
        }

        private void BookingRoom_Click(object sender, RoutedEventArgs e)
        {
            var room = (sender as FrameworkElement)?.DataContext as RoomInformation;
            if (room == null) return;

            var bookingForm = _serviceProvider.GetService<BookingFormWindow>();
            bookingForm.Initialize(room);
            if (bookingForm.ShowDialog() == true)
            {
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

                int days = (bookingForm.EndDate.ToDateTime(TimeOnly.MinValue) -
                           bookingForm.StartDate.ToDateTime(TimeOnly.MinValue)).Days;

                var detail = new BookingDetail
                {
                    RoomId = room.RoomId,
                    Room = room,
                    StartDate = bookingForm.StartDate,
                    EndDate = bookingForm.EndDate,
                    ActualPrice = room.RoomPricePerDay * days
                };

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
                LoadBookingHistory();
                LoadRooms();
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

            var bookingForm = _serviceProvider.GetService<BookingFormWindow>();
            bookingForm.Initialize(detail.Room, detail.StartDate, detail.EndDate);
            if (bookingForm.ShowDialog() == true)
            {
                int days = (bookingForm.EndDate.ToDateTime(TimeOnly.MinValue) -
                           bookingForm.StartDate.ToDateTime(TimeOnly.MinValue)).Days;

                _currentReservation.TotalPrice -= detail.ActualPrice ?? 0;
                detail.StartDate = bookingForm.StartDate;
                detail.EndDate = bookingForm.EndDate;
                detail.ActualPrice = detail.Room.RoomPricePerDay * days;
                _currentReservation.TotalPrice += detail.ActualPrice ?? 0;

                dgCart.Items.Refresh();
            }
        }

        private void DeleteCart_Click(object sender, RoutedEventArgs e)
        {
            var detail = (sender as FrameworkElement)?.DataContext as BookingDetail;
            if (detail == null) return;

            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _cart.Remove(detail);
                _currentReservation.BookingDetails.Remove(detail);
                _currentReservation.TotalPrice -= detail.ActualPrice ?? 0;

                if (!_cart.Any())
                {
                    _currentReservation = null;
                }

                dgCart.Items.Refresh();
            }
        }

        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtTelephone.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var customer = new Customer
            {
                CustomerId = _customer.CustomerId,
                CustomerFullName = txtFullName.Text,
                EmailAddress = txtEmail.Text,
                Telephone = txtTelephone.Text,
                CustomerBirthday = dpBirthday.SelectedDate.HasValue ? DateOnly.FromDateTime(dpBirthday.SelectedDate.Value) : null,
                Password = string.IsNullOrEmpty(txtPassword.Password) ? _customer.Password : txtPassword.Password,
                CustomerStatus = _customer.CustomerStatus
            };

            try
            {
                _customerService.Update(customer);
                _customer = customer;
                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewBookingDetail_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as FrameworkElement)?.DataContext as BookingReservation;
            if (booking?.BookingDetails != null && booking.BookingDetails.Any())
            {
                var detailWindow = _serviceProvider.GetService<BookingDetailWindow>();
                detailWindow.Initialize(booking.BookingDetails, booking.BookingReservationId, null); // null vì customer không cần xác nhận
                detailWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Không có thông tin chi tiết cho booking này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}