using LibraryAssignment.Librarian;
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

        private void btnFines_MouseEnter(object sender, MouseEventArgs e)
        {
            lblFinesInfo.Visibility = Visibility.Visible;
        }

        private void btnFines_MouseLeave(object sender, MouseEventArgs e)
        {
            lblFinesInfo.Visibility = Visibility.Hidden;
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

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void btnManageBooks_Click(object sender, RoutedEventArgs e)
        {
            Close();
            BookManage bookManage = new BookManage();
            bookManage.Show();
        }

        private void btnManageMembers_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MemberManage memberManage = new MemberManage();
            memberManage.Show();
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            Close();
            AllReports reports = new AllReports();
            reports.Show();
        }
    }
}