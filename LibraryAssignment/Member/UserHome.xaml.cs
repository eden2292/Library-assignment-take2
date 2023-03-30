using System;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Globalization;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for UserHome.xaml
    /// </summary>
    public partial class UserHome : Window
    {
        private PramStore _pramStore;

        private string XmlUserfilePath => "UserList.xml";
        private string XmlBookFilePath => "LibraryInventory.xml";

        private decimal lateBookValue;

        public UserHome(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();

            XmlDocument xmlUserDoc = new XmlDocument();
            XmlDocument xmlBookDoc = new XmlDocument();

            xmlUserDoc.Load(XmlUserfilePath);
            xmlBookDoc.Load(XmlBookFilePath);

            XmlNodeList userNodes = xmlUserDoc.DocumentElement.SelectNodes("/catalog/User");
            XmlNodeList bookNodes = xmlBookDoc.DocumentElement.SelectNodes("/library/book");

            foreach (XmlNode xmlBookNode in bookNodes)
            {
                XmlNode title = xmlBookNode.SelectSingleNode("title");
                XmlNode checkedOut = xmlUserDoc.SelectSingleNode($"/catalog/User[UserID ='{_pramStore.CurrentUser.UserId}']/CheckedOut/Book[BookTitle ='{title.InnerText}']/BookTitle");

                if (checkedOut != null)
                {
                    if (title.InnerText == checkedOut.InnerText)
                    {
                        XmlNode value = xmlBookDoc.SelectSingleNode($"/library/book[title='{title.InnerText}']");
                        value = value.ChildNodes.Item(6);
                        lateBookValue = Convert.ToDecimal(value.InnerText);
                    }
                }
            }

            foreach (XmlNode xmlUserNode in userNodes)
            {
                XmlNode id = xmlUserNode.SelectSingleNode("UserID");
                XmlNode dueDate = xmlUserNode.SelectSingleNode($"/catalog/User[UserID ='{_pramStore.CurrentUser.UserId}']/CheckedOut/DueDate");

                if (dueDate != null)
                {
                    if (_pramStore.CurrentUser.UserId == id.InnerText && Convert.ToDateTime(dueDate.InnerText) < DateTime.Now)
                    {
                        DateTime due = Convert.ToDateTime(dueDate.InnerText);
                        DateTime today = DateTime.Now.Date;

                        TimeSpan daysLate = today.Subtract(due);

                        decimal days = Convert.ToDecimal(daysLate.TotalDays);
                        decimal minusGrace = days - 7;

                        decimal forCalc = lateBookValue / 100;

                        decimal fine = forCalc * (minusGrace / 7);

                        XmlNode Fines = xmlUserNode.SelectSingleNode($"/catalog/User[UserID ='{_pramStore.CurrentUser.UserId}']/Fines");

                        Fines.InnerText = Convert.ToString(fine);

                        xmlUserDoc.Save(XmlUserfilePath);

                        _pramStore.CurrentUser.UserFines = fine.ToString("C", CultureInfo.CurrentCulture);
                    }
                }
            }
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