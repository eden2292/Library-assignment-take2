using System;
using System.Windows;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Window
    {
        #region variables

        private readonly PramStore _pramStore;
        public String currentUserId;
        public String currentUserBooks;
        public int currentUserNoOfBooks;

        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";

        // take the current date time and add a month to it, then remove the time part.
        private string dueDate = DateTime.Now.AddMonths(1).ToShortDateString();

        #endregion variables



        public Checkout(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();
            currentUserId = _pramStore.CurrentUser.UserId;
            currentUserBooks = _pramStore.CurrentUser.UserBooks;
        }

        //clear textbox ready for user entry.
        private void txtCheckoutBookId_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCheckoutBookId.Clear();
        }

        //cancel operation and return to the user home page.
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }

        //method to checkout books.
        private void btnCheckoutBook_Click(object sender, RoutedEventArgs e)
        {
            //load xml documents
            XmlDocument xmlBookDoc = new XmlDocument();
            xmlBookDoc.Load(xmlBookFilePath);

            XmlDocument xmlUserDoc = new XmlDocument();
            xmlUserDoc.Load(xmlUserFilePath);

            //create list of nodes within the xml file
            XmlNodeList xmlBookNodeList = xmlBookDoc.DocumentElement.SelectNodes("/library/book");

            //booleans to that will change to true when the book is found and if it is not already checked out.
            bool bookFound = false;
            bool bookAvailable = false;

            foreach (XmlNode bookNode in xmlBookNodeList)
            {
                XmlNode bookId = bookNode.SelectSingleNode("bookId");
                XmlNode checkedOut = bookNode.SelectSingleNode("checkedOut");
                String title = Convert.ToString(bookNode.SelectSingleNode("title"));

                if (txtCheckoutBookId.Text == bookId.InnerText && checkedOut == null)
                {
                    //create a new element in the xml file to hold the date the book is due back.
                    XmlElement newElem = xmlBookDoc.CreateElement("checkedOut");
                    newElem.InnerText = dueDate;

                    bookNode.AppendChild(newElem);

                    if (bookNode.SelectSingleNode("reserved") != null)
                    {
                        XmlNode reserved = bookNode.SelectSingleNode("reserved");
                        bookNode.RemoveChild(reserved);
                    }

                    bookNode.OwnerDocument.Save(xmlBookFilePath);

                    XmlNodeList userNodeList = xmlUserDoc.DocumentElement.SelectNodes("/catalog/User");

                    foreach (XmlNode userNode in userNodeList)
                    {
                        if (_pramStore.CurrentUser.UserId == userNode.SelectSingleNode("UserID").InnerText)
                        {
                            XmlElement newBookElem = xmlUserDoc.CreateElement("BookTitle");
                            XmlElement dueDateElem = xmlUserDoc.CreateElement("DueDate");
                            newBookElem.InnerText = bookNode.SelectSingleNode("title").InnerText;
                            dueDateElem.InnerText = bookNode.SelectSingleNode("checkedOut").InnerText;

                            userNode.SelectSingleNode("CheckedOut").AppendChild(newBookElem);
                            userNode.SelectSingleNode("CheckedOut").AppendChild(dueDateElem);

                            if (userNode.SelectSingleNode("Reserved") != null)
                            {
                                XmlNode Reserved = userNode.SelectSingleNode("Reserved");
                                userNode.RemoveChild(Reserved);
                            }
                            userNode.OwnerDocument.Save(xmlUserFilePath);
                        }
                    }

                    MessageBox.Show($"Your due date is {dueDate}");
                    bookFound = true;
                    bookAvailable = true;
                }
                else if (txtCheckoutBookId.Text == bookId.InnerText && checkedOut != null)
                {
                    bookFound = true;
                }
            }

            if (!bookFound && !bookAvailable)
            {
                MessageBox.Show("This book is not recognised \n please contact the librarian");
            }
            else if (bookFound == true && !bookAvailable)
            {
                MessageBox.Show("This book cannot currently be checked out \n please contact the librarian");
            }
        }
    }
}