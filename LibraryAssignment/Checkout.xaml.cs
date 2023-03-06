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
        public Checkout(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();
            currentUserId = _pramStore.CurrentUser.UserId;
            currentUserBooks = _pramStore.CurrentUser.UserBooks;
            currentUserNoOfBooks = _pramStore.CurrentUser.UserNoBooks;


        }

        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";
        private string dueDate = DateTime.Now.AddMonths(1).ToShortDateString();


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
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlDocument xmlDocUser = new XmlDocument();
            xmlDocUser.Load(xmlUserFilePath);

            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/library/book");

            bool bookFound = false;
            bool bookAvailable = false;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                XmlNode bookId = xmlNode.SelectSingleNode("bookId");
                XmlNode checkedOut = xmlNode.SelectSingleNode("checkedOut");
                String title = Convert.ToString(xmlNode.SelectSingleNode("title"));
                //XmlNode currentUser = xmlNode.SelectSingleNode("UserId");

                if (txtCheckoutBookId.Text == bookId.InnerText && checkedOut == null)
                {
                    XmlElement newElem = xmlDocument.CreateElement("checkedOut");
                    newElem.InnerText = dueDate;

                    xmlNode.AppendChild(newElem);
                    xmlNode.OwnerDocument.Save(xmlBookFilePath);

                    XmlNode oldCheckedOutBooks = xmlDocUser.SelectSingleNode("//User[CheckedOutBooks]");
                    oldCheckedOutBooks.ChildNodes.Item(4).InnerText += title;
                    xmlDocUser.Save(xmlUserFilePath);

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
                else if(bookFound == true && !bookAvailable)
                {
                    MessageBox.Show("This book cannot currently be checked out \n please contact the librarian");
                }
                
            
        }
    }
}
