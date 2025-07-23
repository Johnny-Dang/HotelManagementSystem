using BLL.Services;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;

        public LoginWindow(IAuthService authService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _authService = authService;
            _serviceProvider = serviceProvider;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text.Trim();
            var password = txtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ email và mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = _authService.Login(email, password, out var customer);

            if (result == "Admin")
            {
                MessageBox.Show("Đăng nhập Admin thành công!");
                var adminWindow = _serviceProvider.GetService<AdminWindow>();
                adminWindow.Show();
                this.Close();
            }
            else if (result == "Customer")
            {
                MessageBox.Show("Đăng nhập Customer thành công!");
                var customerWindow = _serviceProvider.GetService<CustomerWindow>();
                customerWindow.Initialize(customer);
                customerWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng!", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}