using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace Payroll_Management
{
    public partial class UserDetailPanel : Window
    {
        // Connection string to access the database
        public string connectionString = ConfigurationManager.ConnectionStrings["Payroll_app.Properties.Settings.PAYROLLConnectionString"].ConnectionString;

        public UserDetailPanel()
        {
            InitializeComponent();
            LoadUserDetails();
        }

        // Load user details into the form fields
        private void LoadUserDetails()
        {
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();
                    string query = "SELECT * FROM User_Info WHERE id = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                    {
                        cmd.Parameters.AddWithValue("@UserID", Login.UserID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate fields
                                txtID.Text = reader["id"].ToString();
                                txtName.Text = reader["Name"].ToString();
                                txtEmail.Text = reader["Email"].ToString();
                                txtUserName.Text = reader["UserName"].ToString();
                                txtPhoneNumber.Text = reader["Phone_number"].ToString();
                                dpDOB.SelectedDate = reader["Date_of_birth"] != DBNull.Value ? Convert.ToDateTime(reader["Date_of_birth"]) : (DateTime?)null;
                                txtHiredDate.Text = reader["Hired_date"].ToString();
                                txtJobTitle.Text = reader["JobTitle"].ToString();
                                txtPAN.Text = reader["PAN_number"].ToString();
                                txtBank.Text = reader["Bank"].ToString();
                                txtBankAccount.Text = reader["Bank_account"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("User not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Update button click event
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();
                    string query = @"
                        UPDATE User_Info 
                        SET 
                            Name = @Name, 
                            Email = @Email, 
                            UserName = @UserName, 
                            Phone_number = @PhoneNumber, 
                            Date_of_birth = @DOB, 
                            PAN_number = @PAN, 
                            Bank = @Bank, 
                            Bank_account = @BankAccount
                        WHERE id = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
                        cmd.Parameters.AddWithValue("@DOB", dpDOB.SelectedDate.HasValue ? dpDOB.SelectedDate.Value : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PAN", txtPAN.Text);
                        cmd.Parameters.AddWithValue("@Bank", txtBank.Text);
                        cmd.Parameters.AddWithValue("@BankAccount", txtBankAccount.Text);
                        cmd.Parameters.AddWithValue("@UserID", Login.UserID);

                        // Execute update
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User details updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("No rows updated. Please try again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
