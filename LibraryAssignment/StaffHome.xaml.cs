using System.Windows;
using System.Windows.Input;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for StaffHome.xaml
    /// </summary>
    public partial class StaffHome : Window
    {
        public StaffHome()
        {
            InitializeComponent();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        #region Label_Controls

        private void btnManageBooks_MouseEnter(object sender, MouseEventArgs e)
        {
            lblManageBooksInfo.Visibility = Visibility.Visible;
        }

        private void btnManageBooks_MouseLeave(object sender, MouseEventArgs e)
        {
            lblManageBooksInfo.Visibility = Visibility.Hidden;
        }

        private void btnManageMembers_MouseEnter(object sender, MouseEventArgs e)
        {
            lblManageMembersInfo.Visibility = Visibility.Visible;
        }

        private void btnManageMembers_MouseLeave(object sender, MouseEventArgs e)
        {
            lblManageMembersInfo.Visibility = Visibility.Hidden;
        }

        private void btnReports_MouseEnter(object sender, MouseEventArgs e)
        {
            lblReportsInfo.Visibility = Visibility.Visible;
        }

        private void btnReports_MouseLeave(object sender, MouseEventArgs e)
        {
            lblReportsInfo.Visibility = Visibility.Hidden;
        }

        #endregion Label_Controls

        private void btnManageBooks_Click(object sender, RoutedEventArgs e)
        {
            Close();
            BookManage manage = new BookManage();
            manage.Show();
        }
    }
}