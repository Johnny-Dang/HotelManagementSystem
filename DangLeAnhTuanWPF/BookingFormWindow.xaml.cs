using DAL.Entities;
using System;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class BookingFormWindow : Window
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public RoomInformation Room { get; private set; }

        public BookingFormWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Initialize(RoomInformation room, DateOnly? startDate = null, DateOnly? endDate = null)
        {
            if (room == null)
            {
                MessageBox.Show("Room data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            Room = room;
            StartDate = startDate ?? DateOnly.FromDateTime(DateTime.Now);
            EndDate = endDate ?? DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            StartDatePicker.SelectedDate = StartDate.ToDateTime(TimeOnly.MinValue);
            EndDatePicker.SelectedDate = EndDate.ToDateTime(TimeOnly.MinValue);
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select both start and end dates.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StartDate = DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value);
            EndDate = DateOnly.FromDateTime(EndDatePicker.SelectedDate.Value);

            // Validation: StartDate phải cách ngày hiện tại ít nhất 1 ngày
            if (StartDate <= DateOnly.FromDateTime(DateTime.Now))
            {
                MessageBox.Show("Start date must be at least 1 day after today.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (StartDate >= EndDate)
            {
                MessageBox.Show("End date must be after start date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Room != null)
            {
                if (Room.RoomStatus == 0)
                {
                    MessageBox.Show("This room is already reserved. Please choose another room.", "Room Unavailable", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (Room.RoomStatus == 1)
                {
                    MessageBox.Show("Room is available. Proceeding with booking!", "Room Available", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (Room.RoomStatus == 2)
                {
                    MessageBox.Show("Room is not available. Please choose another room.", "Room Unavailable", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}