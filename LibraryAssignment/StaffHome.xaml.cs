using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            Hide();
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
        #endregion










    }
}
