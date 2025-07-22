using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class AddEditRoomWindow : Window
    {
        public RoomInformation Room { get; private set; }
        private List<RoomType> _roomTypes;

        public AddEditRoomWindow(RoomInformation room = null)
        {
            InitializeComponent();
            var context = new FuminiHotelManagementContext();
            _roomTypes = context.RoomTypes.ToList();
            cbRoomType.ItemsSource = _roomTypes;

            if (room != null)
            {
                txtRoomNumber.Text = room.RoomNumber;
                txtDescription.Text = room.RoomDetailDescription;
                txtMaxCapacity.Text = room.RoomMaxCapacity?.ToString();
                cbRoomType.SelectedValue = room.RoomTypeId;
                txtStatus.Text = room.RoomStatus?.ToString();
                txtPrice.Text = room.RoomPricePerDay?.ToString();
                Room = room;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text) ||
                string.IsNullOrWhiteSpace(txtMaxCapacity.Text) ||
                cbRoomType.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtStatus.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Room == null)
                Room = new RoomInformation();

            Room.RoomNumber = txtRoomNumber.Text.Trim();
            Room.RoomDetailDescription = txtDescription.Text.Trim();
            Room.RoomMaxCapacity = int.TryParse(txtMaxCapacity.Text, out var cap) ? cap : 1;
            Room.RoomTypeId = (int)cbRoomType.SelectedValue;
            Room.RoomStatus = byte.TryParse(txtStatus.Text, out var status) ? status : (byte)1;
            Room.RoomPricePerDay = decimal.TryParse(txtPrice.Text, out var price) ? price : 0;

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
} 