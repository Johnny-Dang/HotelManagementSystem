using BLL.Services;
using DAL.Entities;
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.AddSingleton<FuminiHotelManagementContext>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var roomService = _serviceProvider.GetService<IRoomService>();
            var bookingService = _serviceProvider.GetService<IBookingService>();
            var customer = new Customer { CustomerId = 1 }; // Replace with actual customer data
            var customerWindow = new CustomerWindow(customer, roomService, bookingService);
            customerWindow.Show();
        }
    }
}