using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Payroll_Management
{
    public partial class LandingPage : Window
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        private void btn_Admin_Click(object sender, RoutedEventArgs e)
        {
            if (Login.UserID == 1) // Check if the UserID is admin
            {
                // Create an instance of the AdminPanel window
                AdminPanel adminPanelWindow = new AdminPanel();

                // Set the content of the Frame to AdminPanel's content
                RightPanelFrame.Content = adminPanelWindow.Content;
            }
            else
            {
                MessageBox.Show("Access Denied: You do not have admin privileges.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_UserDetails_Click(object sender, RoutedEventArgs e)
        {
            UserDetailPanel userDetailPanelWindow = new UserDetailPanel();
            RightPanelFrame.Content = userDetailPanelWindow.Content;
        }
    }
}
