using BLL.Services;
using DAL.Entities;
using System;
using System.Net.Mail;
using System.Windows;

namespace DangLeAnhTuanWPF
{
    public partial class AddEditCustomerWindow : Window
    {
        public Customer Customer { get; private set; }
        private readonly ICustomerService _customerService;

        public AddEditCustomerWindow(ICustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;
        }

        public void Initialize(Customer customer)
        {
            Customer = new Customer
            {
                CustomerId = customer.CustomerId,
                CustomerFullName = customer.CustomerFullName,
                Telephone = customer.Telephone,
                EmailAddress = customer.EmailAddress,
                CustomerBirthday = customer.CustomerBirthday,
                CustomerStatus = customer.CustomerStatus,
                Password = customer.Password
            };
            txtFullName.Text = customer.CustomerFullName;
            txtTelephone.Text = customer.Telephone;
            txtEmail.Text = customer.EmailAddress;
            dpBirthday.SelectedDate = customer.CustomerBirthday?.ToDateTime(TimeOnly.MinValue);
            txtStatus.Text = customer.CustomerStatus?.ToString() ?? "1";
            txtPassword.Password = customer.Password;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtTelephone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                !dpBirthday.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Email không đúng định dạng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_customerService.IsEmailTaken(txtEmail.Text.Trim(), Customer?.CustomerId))
            {
                MessageBox.Show("Email đã bị trùng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Customer == null)
                Customer = new Customer();

            Customer.CustomerFullName = txtFullName.Text.Trim();
            Customer.Telephone = txtTelephone.Text.Trim();
            Customer.EmailAddress = txtEmail.Text.Trim();
            Customer.CustomerBirthday = dpBirthday.SelectedDate.HasValue
                ? DateOnly.FromDateTime(dpBirthday.SelectedDate.Value)
                : null;
            Customer.CustomerStatus = byte.TryParse(txtStatus.Text, out var status) ? status : (byte)1;
            Customer.Password = txtPassword.Password;

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