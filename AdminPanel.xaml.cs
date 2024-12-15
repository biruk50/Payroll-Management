using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace Payroll_Management
{
    public partial class AdminPanel : Window
    {
        // Connection string from App.config
        public string connectionString = ConfigurationManager.ConnectionStrings["Payroll_app.Properties.Settings.PAYROLLConnectionString"].ConnectionString;

        public AdminPanel()
        {
            InitializeComponent();
            LoadDataGrid(); // Load data when the window opens
        }

        // Load data into DataGrid
        private void LoadDataGrid()
        {
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();
                    string query = "SELECT * FROM User_Info";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnect);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridEntries.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Populate fields when DataGrid row is selected
        private void dataGridEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridEntries.SelectedItem is DataRowView row)
            {
                txtName.Text = row["Name"].ToString();
                txtEmail.Text = row["Email"].ToString();
                txtUserName.Text = row["UserName"].ToString();
                txtPassword.Password = row["Password"].ToString();
                txtJobTitle.Text = row["JobTitle"].ToString();
                txtPhoneNumber.Text = row["Phone_number"].ToString();
                dpDOB.SelectedDate = row["Date_of_birth"] as DateTime?;
                dpHiredDate.SelectedDate = row["Hired_date"] as DateTime?;
                txtPAN.Text = row["PAN_number"].ToString();
                txtBank.Text = row["Bank"].ToString();
                txtBankAccount.Text = row["Bank_account"].ToString();
            }
        }

        // Save a new record
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();
                    string query = @"INSERT INTO User_Info (Name, Email, UserName, Password, JobTitle, Phone_number, Date_of_birth, Hired_date, PAN_number, Bank, Bank_account)
                                     VALUES (@Name, @Email, @UserName, @Password, @JobTitle, @Phone_number, @Date_of_birth, @Hired_date, @PAN_number, @Bank, @Bank_account)";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Password);
                        cmd.Parameters.AddWithValue("@JobTitle", txtJobTitle.Text);
                        cmd.Parameters.AddWithValue("@Phone_number", txtPhoneNumber.Text);
                        cmd.Parameters.AddWithValue("@Date_of_birth", dpDOB.SelectedDate);
                        cmd.Parameters.AddWithValue("@Hired_date", dpHiredDate.SelectedDate);
                        cmd.Parameters.AddWithValue("@PAN_number", txtPAN.Text);
                        cmd.Parameters.AddWithValue("@Bank", txtBank.Text);
                        cmd.Parameters.AddWithValue("@Bank_account", txtBankAccount.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadDataGrid(); // Refresh DataGrid
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Update an existing record
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEntries.SelectedItem is DataRowView row)
            {
                int id = Convert.ToInt32(row["id"]);
                try
                {
                    using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                    {
                        sqlConnect.Open();
                        string query = @"UPDATE User_Info 
                                         SET Name=@Name, Email=@Email, UserName=@UserName, Password=@Password, 
                                             JobTitle=@JobTitle, Phone_number=@Phone_number, Date_of_birth=@Date_of_birth, 
                                             Hired_date=@Hired_date, PAN_number=@PAN_number, Bank=@Bank, Bank_account=@Bank_account
                                         WHERE id=@id";
                        using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@Name", txtName.Text);
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                            cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Password);
                            cmd.Parameters.AddWithValue("@JobTitle", txtJobTitle.Text);
                            cmd.Parameters.AddWithValue("@Phone_number", txtPhoneNumber.Text);
                            cmd.Parameters.AddWithValue("@Date_of_birth", dpDOB.SelectedDate);
                            cmd.Parameters.AddWithValue("@Hired_date", dpHiredDate.SelectedDate);
                            cmd.Parameters.AddWithValue("@PAN_number", txtPAN.Text);
                            cmd.Parameters.AddWithValue("@Bank", txtBank.Text);
                            cmd.Parameters.AddWithValue("@Bank_account", txtBankAccount.Text);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadDataGrid(); // Refresh DataGrid
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Delete a record
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEntries.SelectedItem is DataRowView row)
            {
                int id = Convert.ToInt32(row["id"]);
                try
                {
                    using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                    {
                        sqlConnect.Open();
                        string query = "DELETE FROM User_Info WHERE id=@id";
                        using (SqlCommand cmd = new SqlCommand(query, sqlConnect))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadDataGrid(); // Refresh DataGrid
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
