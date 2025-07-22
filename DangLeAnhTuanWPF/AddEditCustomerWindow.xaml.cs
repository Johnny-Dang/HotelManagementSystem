using DAL.Entities;
using System;
using System.Windows;
using System.Net.Mail;

namespace DangLeAnhTuanWPF
{
    public partial class AddEditCustomerWindow : Window
    {
        public Customer Customer { get; private set; }

        public AddEditCustomerWindow(Customer customer = null)
        {
            InitializeComponent();
            if (customer != null)
            {
                // Luôn giữ CustomerId trong object Customer để phục vụ update
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
                dpBirthday.SelectedDate = customer.CustomerBirthday.HasValue
                    ? customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue)
                    : null;
                txtStatus.Text = customer.CustomerStatus?.ToString() ?? "1";
                txtPassword.Password = customer.Password;
            }
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

            // 1. Validate email format
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Email không đúng định dạng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Kiểm tra trùng email trong database
            var context = new DAL.Entities.FuminiHotelManagementContext();
            var repo = new DAL.Repository.CustomerRepository(context);
            var existed = repo.GetByEmail(txtEmail.Text.Trim());
            if (existed != null && (Customer == null || existed.CustomerId != Customer.CustomerId))
            {
                MessageBox.Show("Email đã bị trùng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Customer == null)
                Customer = new DAL.Entities.Customer();

            // Luôn giữ CustomerId nếu có
            // (nếu là edit thì CustomerId đã có, nếu là add thì CustomerId sẽ được sinh tự động)

            Customer.CustomerFullName = txtFullName.Text.Trim();
            Customer.Telephone = txtTelephone.Text.Trim();
            Customer.EmailAddress = txtEmail.Text.Trim();
            Customer.CustomerBirthday = dpBirthday.SelectedDate.HasValue
                ? DateOnly.FromDateTime(dpBirthday.SelectedDate.Value)
                : null;
            Customer.CustomerStatus = byte.TryParse(txtStatus.Text, out var status) ? status : (byte)1;
            Customer.Password = txtPassword.Password;

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
} 