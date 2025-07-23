using BLL.Services;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DangLeAnhTuanWPF
{
    public partial class AdminWindow : Window
    {
        private readonly IRoomService _roomService;
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;
        private readonly IServiceProvider _serviceProvider;
        private Customer _selectedCustomer;
        private RoomInformation _selectedRoom;
        private BookingReservation _selectedBooking;

        public AdminWindow(IRoomService roomService, ICustomerService customerService, IBookingService bookingService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _roomService = roomService;
            _customerService = customerService;
            _bookingService = bookingService;
            _serviceProvider = serviceProvider;
            LoadCustomers();
            LoadRooms();
            LoadBookings();
        }

        private void LoadCustomers()
        {
            dgCustomers.ItemsSource = _customerService.GetAllCustomers();
        }

        private void LoadRooms()
        {
            dgRooms.ItemsSource = _roomService.GetAllRooms();
        }

        private void LoadBookings()
        {
            dgBookings.ItemsSource = _bookingService.GetAllBookingsWithDetails();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedItem is TabItem tabItem)
            {
                switch (tabItem.Header.ToString())
                {
                    case "Room Management":
                        LoadRooms();
                        break;
                    case "Customer Management":
                        LoadCustomers();
                        break;
                    case "Booking Management":
                        LoadBookings();
                        break;
                }
            }
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = _serviceProvider.GetService<AddEditCustomerWindow>();
            if (addWindow.ShowDialog() == true)
            {
                _customerService.AddCustomer(addWindow.Customer);
                LoadCustomers();
            }
        }

        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCustomer = dgCustomers.SelectedItem as Customer;
        }

        private void dgRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRoom = dgRooms.SelectedItem as RoomInformation;
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer != null)
            {
                var editWindow = _serviceProvider.GetService<AddEditCustomerWindow>();
                editWindow.Initialize(_selectedCustomer);
                if (editWindow.ShowDialog() == true)
                {
                    _customerService.UpdateCustomer(editWindow.Customer);
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer != null)
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa khách hàng '{_selectedCustomer.CustomerFullName}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _customerService.DeleteCustomer(_selectedCustomer.CustomerId);
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            var keyword = txtCustomerSearch.Text.Trim();
            dgCustomers.ItemsSource = string.IsNullOrEmpty(keyword)
                ? _customerService.GetAllCustomers()
                : _customerService.SearchCustomers(keyword);
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = _serviceProvider.GetService<AddEditRoomWindow>();
            if (addWindow.ShowDialog() == true)
            {
                _roomService.AddRoom(addWindow.Room);
                LoadRooms();
            }
        }

        private void EditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom != null)
            {
                var editWindow = _serviceProvider.GetService<AddEditRoomWindow>();
                editWindow.Initialize(_selectedRoom);
                if (editWindow.ShowDialog() == true)
                {
                    _roomService.UpdateRoom(editWindow.Room);
                    LoadRooms();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phòng để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom != null)
            {
                // Kiểm tra phòng có trong BookingDetail không
                if (_bookingService.HasBookingDetailWithRoom(_selectedRoom.RoomId))
                {
                    MessageBox.Show($"Phòng '{_selectedRoom.RoomNumber}' đã từng được đặt, không thể xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show($"Bạn có chắc muốn xóa phòng '{_selectedRoom.RoomNumber}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _roomService.DeleteRoom(_selectedRoom.RoomId);
                        LoadRooms();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phòng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SearchRoom_Click(object sender, RoutedEventArgs e)
        {
            var keyword = txtRoomSearch.Text.Trim();
            dgRooms.ItemsSource = string.IsNullOrEmpty(keyword)
                ? _roomService.GetAllRooms()
                : _roomService.SearchRooms(keyword);
        }

        private void dgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedBooking = dgBookings.SelectedItem as BookingReservation;
        }

        private void ViewBookingDetail_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as FrameworkElement)?.DataContext as BookingReservation;
            if (booking?.BookingDetails != null && booking.BookingDetails.Any())
            {
                var detailWindow = _serviceProvider.GetService<BookingDetailWindow>();
                detailWindow.Initialize(booking.BookingDetails, booking.BookingReservationId, _bookingService, booking.BookingStatus.Value); // truyền thêm BookingStatus
                if (detailWindow.ShowDialog() == true)
                {
                    LoadBookings();
                }
            }
            else
            {
                MessageBox.Show("Không có thông tin chi tiết cho booking này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (!dpStartDate.SelectedDate.HasValue || !dpEndDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và ngày kết thúc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var startDate = DateOnly.FromDateTime(dpStartDate.SelectedDate.Value);
            var endDate = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value);

            if (startDate > endDate)
            {
                MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var report = _bookingService.GenerateBookingReport(startDate, endDate)
                    .OrderByDescending(r => r.TotalRevenue)
                    .ToList();
                dgReport.ItemsSource = report.Select(r => new { r.RoomNumber, r.TotalBookings, r.TotalRevenue });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetCancelled_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as FrameworkElement)?.DataContext as BookingReservation;
            if (booking == null) return;
            if (booking.BookingStatus == 2)
            {
                MessageBox.Show("Booking đã ở trạng thái Cancelled!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (MessageBox.Show($"Bạn có chắc muốn hủy booking ID {booking.BookingReservationId}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _bookingService.UpdateBookingStatus(booking.BookingReservationId, 2);
                LoadBookings();
                MessageBox.Show("Đã cập nhật trạng thái Cancelled!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SetCompleted_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as FrameworkElement)?.DataContext as BookingReservation;
            if (booking == null) return;
            if (booking.BookingStatus == 3)
            {
                MessageBox.Show("Booking đã ở trạng thái Completed!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (MessageBox.Show($"Bạn có chắc muốn hoàn thành booking ID {booking.BookingReservationId}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _bookingService.UpdateBookingStatusAndRoom(booking.BookingReservationId, 3);
                LoadBookings();
                LoadRooms(); // cập nhật lại UI RoomManagement
                MessageBox.Show("Đã cập nhật trạng thái Completed và phòng đã được mở lại!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}