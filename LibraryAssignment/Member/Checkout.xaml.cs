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
        private readonly PramStore _pramStore;
        public String currentUserId;
        public String currentUserBooks;
        public int currentUserNoOfBooks;

        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";
        private string dueDate = DateTime.Now.AddMonths(1).ToShortDateString();

        public Checkout(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();
            currentUserId = _pramStore.CurrentUser.UserId;
            currentUserBooks = _pramStore.CurrentUser.UserBooks;
        }

        private void txtCheckoutBookId_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCheckoutBookId.Clear();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }

        private void btnCheckoutBook_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocBook = new XmlDocument();
            xmlDocBook.Load(xmlBookFilePath);

            XmlDocument xmlDocUser = new XmlDocument();
            xmlDocUser.Load(xmlUserFilePath);

            XmlNodeList xmlNodeList = xmlDocBook.DocumentElement.SelectNodes("/library/book");

            bool bookFound = false;
            bool bookAvailable = false;

            foreach (XmlNode xmlBookNode in xmlNodeList)
            {
                XmlNode bookId = xmlBookNode.SelectSingleNode("bookId");
                XmlNode checkedOut = xmlBookNode.SelectSingleNode("checkedOut");
                String title = Convert.ToString(xmlBookNode.SelectSingleNode("title"));
                //XmlNode currentUser = xmlNode.SelectSingleNode("UserId");

                if (txtCheckoutBookId.Text == bookId.InnerText && checkedOut == null)
                {
                    XmlElement newElem = xmlDocBook.CreateElement("checkedOut");
                    newElem.InnerText = dueDate;

                    xmlBookNode.AppendChild(newElem);

                    if(xmlBookNode.SelectSingleNode("reserved") != null)
                    {
                        XmlNode reserved = xmlBookNode.SelectSingleNode("reserved");
                        xmlBookNode.RemoveChild(reserved);
                    }

                    xmlBookNode.OwnerDocument.Save(xmlBookFilePath);

                    XmlNodeList userNodeList = xmlDocUser.DocumentElement.SelectNodes("/catalog/User");

                    foreach (XmlNode xmlUserNode in userNodeList)
                    {
                        if (_pramStore.CurrentUser.UserId == xmlUserNode.SelectSingleNode("UserID").InnerText)
                        {
                            XmlElement newBookElem = xmlDocUser.CreateElement("BookTitle");
                            XmlElement dueDateElem = xmlDocUser.CreateElement("DueDate");
                            newBookElem.InnerText = xmlBookNode.SelectSingleNode("title").InnerText;
                            dueDateElem.InnerText = xmlBookNode.SelectSingleNode("checkedOut").InnerText;

                            xmlUserNode.SelectSingleNode("CheckedOut").AppendChild(newBookElem);
                            xmlUserNode.SelectSingleNode("CheckedOut").AppendChild(dueDateElem);

                            if (xmlUserNode.SelectSingleNode("Reserved") != null)
                            {
                                XmlNode Reserved = xmlUserNode.SelectSingleNode("Reserved");
                                xmlUserNode.RemoveChild(Reserved);
                            }
                            xmlUserNode.OwnerDocument.Save(xmlUserFilePath);
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