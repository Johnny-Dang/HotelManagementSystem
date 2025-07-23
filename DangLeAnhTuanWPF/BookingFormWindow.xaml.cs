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

            if (StartDate >= EndDate)
            {
                MessageBox.Show("End date must be after start date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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