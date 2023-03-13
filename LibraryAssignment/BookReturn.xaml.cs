using System;
using System.Windows;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for BookReturn.xaml
    /// </summary>
    public partial class BookReturn : Window
    {
        private readonly PramStore _pramStore;
        public String currentUserId;
        public String currentUserBooks;
        public String title;

        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";
        private string newDate = DateTime.Now.AddMonths(1).ToShortDateString();

        public BookReturn(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();
            currentUserId = _pramStore.CurrentUser.UserId;
            currentUserBooks = _pramStore.CurrentUser.UserBooks;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }

        private void txtReturn_GotFocus(object sender, RoutedEventArgs e)
        {
            txtReturn.Clear();
        }

        #region return
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlDocument userDocument = new XmlDocument();
            userDocument.Load(xmlUserFilePath);

            XmlNodeList bookNodes = xmlDocument.DocumentElement.SelectNodes("/library/book");
            XmlNodeList userNodes = userDocument.DocumentElement.SelectNodes("/catalog/User");


            //bool bookFound = false;

            foreach (XmlNode book in bookNodes)
            {

                XmlNode bookId = book.SelectSingleNode("bookId");

                if (txtReturn.Text == bookId.InnerText)
                {

                    XmlNode checkedOut = book.SelectSingleNode("checkedOut");
                    title = book.SelectSingleNode("title").InnerText;
                    book.RemoveChild(checkedOut);

                }
            }

            foreach (XmlNode user in userNodes)
            {
                XmlNode bookTitle = user.SelectSingleNode("/catalog/User/CheckedOut/BookTitle");

                if (bookTitle.InnerText == title)
                {
                    bookTitle.ParentNode.RemoveChild(bookTitle);
                }

            }
            xmlDocument.Save(xmlBookFilePath);
            userDocument.Save(xmlUserFilePath);
        }


        #endregion

        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlNodeList bookNodes = xmlDocument.DocumentElement.SelectNodes("/library/book");

            foreach (XmlNode book in bookNodes)
            {

                XmlNode bookId = book.SelectSingleNode("bookId");

                if (txtReturn.Text == bookId.InnerText)
                {

                    XmlNode checkedOut = book.SelectSingleNode("checkedOut");
                    //title = book.SelectSingleNode("title").InnerText;
                    XmlNode dueDate("<checkedOut>" + newDate + "</checkedOut>");
                    book.ReplaceChild(dueDate, checkedOut);

                }
            }
        }
    }
}