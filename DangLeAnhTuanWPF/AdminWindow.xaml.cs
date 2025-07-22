using BLL.Services;
using System.Windows;
using System.Windows.Controls;

namespace DangLeAnhTuanWPF
{
    public partial class AdminWindow : Window
    {
        private RoomService _roomService = new RoomService();
        private CustomerService _customerService = new CustomerService();
        private BookingService _bookingService = new BookingService();

        private DAL.Entities.Customer _selectedCustomer;
        private DAL.Entities.RoomInformation _selectedRoom;
        private DAL.Entities.BookingReservation _selectedBooking;

        public AdminWindow()
        {
            InitializeComponent();
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
            if (e.Source is TabControl tabControl)
            {
                if (tabControl.SelectedItem is TabItem tabItem)
                {
                    if (tabItem.Header.ToString() == "Room Management")
                    {
                        LoadRooms();
                    }
                    else if (tabItem.Header.ToString() == "Customer Management")
                    {
                        LoadCustomers();
                    }
                    else if (tabItem.Header.ToString() == "Booking Management")
                    {
                        LoadBookings();
                    }
                }
            }
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditCustomerWindow();
            if (addWindow.ShowDialog() == true)
            {
                _customerService.AddCustomer(addWindow.Customer);
                LoadCustomers();
            }
        }

        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCustomer = dgCustomers.SelectedItem as DAL.Entities.Customer;
        }

        private void dgRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRoom = dgRooms.SelectedItem as DAL.Entities.RoomInformation;
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer != null)
            {
                var editWindow = new AddEditCustomerWindow(_selectedCustomer);
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
                    _customerService.DeleteCustomer(_selectedCustomer.CustomerId);
                    LoadCustomers();
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
            if (string.IsNullOrEmpty(keyword))
                LoadCustomers();
            else
                dgCustomers.ItemsSource = _customerService.SearchCustomers(keyword);
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditRoomWindow();
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
                var editWindow = new AddEditRoomWindow(_selectedRoom);
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
                if (MessageBox.Show($"Bạn có chắc muốn xóa phòng '{_selectedRoom.RoomNumber}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _roomService.DeleteRoom(_selectedRoom.RoomId);
                    LoadRooms();
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
            if (string.IsNullOrEmpty(keyword))
                LoadRooms();
            else
                dgRooms.ItemsSource = _roomService.SearchRooms(keyword);
        }

        private void dgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedBooking = dgBookings.SelectedItem as DAL.Entities.BookingReservation;
        }

        private void ViewBookingDetail_Click(object sender, RoutedEventArgs e)
        {
            var booking = (sender as FrameworkElement)?.DataContext as DAL.Entities.BookingReservation;
            if (booking != null && booking.BookingDetails != null)
            {
                var detailWindow = new BookingDetailWindow(booking.BookingDetails);
                detailWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Không có thông tin chi tiết cho booking này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
} 