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
        #region variables

        private readonly PramStore _pramStore;
        public String currentUserId;
        public String currentUserBooks;
        public String title;
        //boolean that switches if book is found in the xml file. 
        private bool bookFound = false;
        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";

        //datetime variable that is used to generate new return date. 
        private string newDate = DateTime.Now.AddMonths(1).ToShortDateString();

        #endregion variables

        public BookReturn(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();
            currentUserId = _pramStore.CurrentUser.UserId;
            currentUserBooks = _pramStore.CurrentUser.UserBooks;
        }

        //Cancel the chosen operation and return to the user home page
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }

        //clear text box ready for user input
        private void txtReturn_GotFocus(object sender, RoutedEventArgs e)
        {
            txtReturn.Clear();
        }

        #region return

        //Method to return books.
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            //load xml files.
            XmlDocument xmlBookDoc = new XmlDocument();
            xmlBookDoc.Load(xmlBookFilePath);

            XmlDocument xmlUserDoc = new XmlDocument();
            xmlUserDoc.Load(xmlUserFilePath);

            //create list of nodes from xml files.
            XmlNodeList bookNodes = xmlBookDoc.DocumentElement.SelectNodes("/library/book");
            XmlNodeList userNodes = xmlUserDoc.DocumentElement.SelectNodes("/catalog/User");

            //loop through to check against each book ID node and check if it matches the one entered by the user.
            foreach (XmlNode bookNode in bookNodes)
            {
                XmlNode bookId = bookNode.SelectSingleNode("bookId");

                //if they match, remove the "checkedout"" node and grab the title. Display a message box to inform the user their command has been successful.
                if (txtReturn.Text == bookId.InnerText)
                {
                    XmlNode checkedOut = bookNode.SelectSingleNode("checkedOut");
                    title = bookNode.SelectSingleNode("title").InnerText;
                    bookNode.RemoveChild(checkedOut);
                    MessageBox.Show("Successfully returned!");
                    bookFound = true;
                }
            }

            //if the bookfound boolean never returns true, show an error message.
            if (bookFound == false)
            {
                MessageBox.Show("An error has occured \n please contact the librarian");
            }

            //loop through each user in the xml node list and check if the title of a book they have checked out matches the previously gathered title
            foreach (XmlNode userNode in userNodes)
            {
                XmlNode book = userNode.SelectSingleNode($"/catalog/User['{_pramStore.CurrentUser.UserId}']/CheckedOut/Book");

                //remove the nodes underneath "checkedout" - note removal of the checked out node itself will break the pramstore.
                if (book != null && book.SelectSingleNode("BookTitle").InnerText == title)
                {
                    book.ParentNode.RemoveChild(book);
                }
            }
            //Save the files
            xmlBookDoc.Save(xmlBookFilePath);
            xmlUserDoc.Save(xmlUserFilePath);
        }

        #endregion return

        #region renew

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
                    //instead of removing nodes when found, update the innertext of the checked out node to read one month from the renewal date.
                    book.SelectSingleNode("checkedOut").InnerText = newDate;
                    MessageBox.Show($"Your new due date is \n {newDate}");
                    bookFound = true;
                }

                xmlDocument.Save(xmlBookFilePath);

            }


            if (bookFound != true)
            {
                MessageBox.Show("An error has occured \n please contact the librarian");
            }
        }

        #endregion renew
    }
}