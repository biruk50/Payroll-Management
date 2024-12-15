using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Payroll_Management
{
    public partial class Login : Window
    {
        public static int UserID { get; private set; }

        public string connectionString = ConfigurationManager.ConnectionStrings["Payroll_app.Properties.Settings.PAYROLLConnectionString"].ConnectionString;

        public Login()
        {
            InitializeComponent();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text.Trim();
            string password = Password.Password.Trim();
            
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();

                    string query = "SELECT id FROM User_Info WHERE UserName = @UserName AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                    {
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            UserID = Convert.ToInt32(result);
                            MessageBox.Show("Login Successful!", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Hide();
                            LandingPage landing_Page = new LandingPage();
                            landing_Page.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PasswordChangeLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.Show();
            this.Close();
        }

    }
}

