using BLL.Services;
using DAL.Entities;
using DAL.Repository;
using DangLeAnhTuanWPF.Helpers;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService(
                new CustomerRepository(new FuminiHotelManagementContext()),
                AppConfig.AdminEmail,
                AppConfig.AdminPassword
            );
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
                var adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }
            else if (result == "Customer")
            {
                MessageBox.Show("Đăng nhập Customer thành công!");
                var customerWindow = new CustomerWindow(customer);
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
