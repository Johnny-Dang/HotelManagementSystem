using DAL.Entities;
using System;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class BookingFormWindow : Window
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public RoomInformation Room { get; }

        public BookingFormWindow(RoomInformation room, DateOnly? startDate = null, DateOnly? endDate = null)
        {
            InitializeComponent();
            Room = room;
            DataContext = this;

            StartDate = startDate ?? DateOnly.FromDateTime(DateTime.Now);
            EndDate = endDate ?? DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (StartDate == default || EndDate == default)
            {
                MessageBox.Show("Please select both start and end dates.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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