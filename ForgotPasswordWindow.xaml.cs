using Payroll_Management;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace Payroll_Management
{
    public partial class ForgotPasswordWindow : Window
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["Payroll_app.Properties.Settings.PAYROLLConnectionString"].ConnectionString;
        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        private void btn_password_recovery(object sender, RoutedEventArgs e)
        {
            // Get user input
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string dob = DobTextBox.Text.Trim();

            // Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(dob))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate Date of Birth format
            if (!DateTime.TryParse(dob, out DateTime parsedDob))
            {
                MessageBox.Show("Invalid date format. Please use YYYY-MM-DD.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Payroll_app.Properties.Settings.PAYROLLConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();

                    string query = @"
                        SELECT Password 
                        FROM User_Info 
                        WHERE UserName = @UserName AND Email = @Email AND Phone_number = @Phone AND Date_of_birth = @Dob";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                    {
                        // Add query parameters
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Dob", parsedDob);

                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("No matching record found. Please contact the office's admin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            
                        }
                        else
                        {
                            string password = result.ToString();
                            MessageBox.Show($"Your password is: {password}", "Password Recovery", MessageBoxButton.OK, MessageBoxImage.Information);

                            Login login = new Login();
                            login.Show();

                            // Close the current ForgotPasswordWindow
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
