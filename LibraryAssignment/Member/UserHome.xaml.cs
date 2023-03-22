using System.Windows;
using System.Windows.Input;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for UserHome.xaml
    /// </summary>
    public partial class UserHome : Window
    {
        private PramStore _pramStore;

        public UserHome(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();


        }

        //Controls to Show and hide labels with further information on presented options.

        #region Label controls

        private void btnCheckout_MouseEnter(object sender, MouseEventArgs e)
        {
            lblCheckout.Visibility = Visibility.Visible;
        }

        private void btnCheckout_MouseLeave(object sender, MouseEventArgs e)
        {
            lblCheckout.Visibility = Visibility.Hidden;
        }

        private void btnReturn_MouseEnter(object sender, MouseEventArgs e)
        {
            lblReturn.Visibility = Visibility.Visible;
        }

        private void btnReturn_MouseLeave(object sender, MouseEventArgs e)
        {
            lblReturn.Visibility = Visibility.Hidden;
        }

        private void btnSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            lblSearch.Visibility = Visibility.Visible;
        }

        private void btnSearch_MouseLeave(object sender, MouseEventArgs e)
        {
            lblSearch.Visibility = Visibility.Hidden;
        }

        private void btnAccount_MouseEnter(object sender, MouseEventArgs e)
        {
            lblAccount.Visibility = Visibility.Visible;
        }

        private void btnAccount_MouseLeave(object sender, MouseEventArgs e)
        {
            lblAccount.Visibility = Visibility.Hidden;
        }

        #endregion Label controls

        //Controls to switch to a new page when the user selects an option

        #region userPageControls

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Checkout checkout = new Checkout(_pramStore);
            checkout.Show();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
            BookReturn bookreturn = new BookReturn(_pramStore);
            bookreturn.Show();
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Account account = new Account(_pramStore);
            account.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Search search = new Search(_pramStore);
            search.Show();
        }

        #endregion userPageControls

        //log out and return to log in page. 
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow main = new MainWindow();
            main.Show();
        }
    }
}