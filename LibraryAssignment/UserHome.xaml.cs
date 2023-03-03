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
    /// Interaction logic for UserHome.xaml
    /// </summary>
    public partial class UserHome : Window
    {
        public UserHome()
        {
            InitializeComponent();
        }

        
        //Code that I was trying out. Might work....
        #region ExperimentalCode
        //private void btnCheckout_MouseEnter((Button) sender, MouseEventArgs e)
        //{
        //    Button button = new Button();

        //    if (button.Content == "Checkout")
        //    {
        //        lblInfo.Content = "Checkout your selected books";
        //    }
        //    else if(button.Content == "Return")
        //    {
        //        lblInfo.Content = "Return books you have read";
        //    }
        //    else if (button.Content == "Search")
        //    {
        //        lblInfo.Content = "Search for and reserve books";
        //    }
        //    else if (button.Content == "Account")
        //    {
        //        lblInfo.Content = "View account information, including fines";
        //    }
        //    lblInfo.Visibility = Visibility.Visible;
        //}
        #endregion

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

        #endregion


        //Controls to switch to a new page when the user selects an option
        #region userPageControls

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Checkout checkout = new Checkout();
            checkout.Show();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            BookReturn bookreturn = new BookReturn();
            bookreturn.Show();
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Account account = new Account();
            account.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Search search = new Search();
            search.Show();
        }

        #endregion


    }
}
