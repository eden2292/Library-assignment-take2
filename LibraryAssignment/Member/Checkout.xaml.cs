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
            //loop through each book in the xml file and find book id, checked out and title nodes. 
            foreach (XmlNode bookNode in xmlBookNodeList)
            {
                XmlNode bookId = bookNode.SelectSingleNode("bookId");
                XmlNode checkedOut = bookNode.SelectSingleNode("checkedOut");
                String title = Convert.ToString(bookNode.SelectSingleNode("title"));
                XmlNode reserved = bookNode.SelectSingleNode("reserved");
                //if the entered ID matches the ID in the xml file, and checked out node returns null, then the user is able to take the book out. 
                if (txtCheckoutBookId.Text == bookId.InnerText && checkedOut == null)
                {
                    //create a new element in the xml file to hold the date the book is due back.
                    XmlElement newElem = xmlBookDoc.CreateElement("checkedOut");
                    //add the dueDate variable as the innertext of the checked out element. 
                    newElem.InnerText = dueDate;
                    //add the new element to the node. 
                    bookNode.AppendChild(newElem);
                    //remove the reserved node, if it exists. 
                    if (bookNode.SelectSingleNode("reserved") != null)
                    {
                        XmlNode reservedremove = bookNode.SelectSingleNode("reserved");
                        bookNode.RemoveChild(reservedremove);
                    }
                    //save the xml file with the amended information. 
                    bookNode.OwnerDocument.Save(xmlBookFilePath);
                    //create a new list of the nodes in the user XML file. 
                    XmlNodeList userNodeList = xmlUserDoc.DocumentElement.SelectNodes("/catalog/User");
                    
                    foreach (XmlNode userNode in userNodeList)
                    {
                        //compare the user ID from the global parameters against the ID node of each user in the xml file to find the one that matches. 
                        if (_pramStore.CurrentUser.UserId == userNode.SelectSingleNode("UserID").InnerText)
                        {
                            //create the information that the user needs, book title and due date, wrapped inside a new book node within their checked out section. 
                            XmlElement bookElem = xmlUserDoc.CreateElement("Book");
                            XmlElement newBookElem = xmlUserDoc.CreateElement("BookTitle");
                            XmlElement dueDateElem = xmlUserDoc.CreateElement("DueDate");
                            //take the title and checked out date from the previously selected book and add the information from those nodes to the new elements. 
                            newBookElem.InnerText = bookNode.SelectSingleNode("title").InnerText;
                            dueDateElem.InnerText = bookNode.SelectSingleNode("checkedOut").InnerText;

                            bookElem.AppendChild(newBookElem);
                            bookElem.AppendChild(dueDateElem);
                            //add these to the checked out node. 
                            XmlNode parent = xmlUserDoc.SelectSingleNode($"/catalog/User['{_pramStore.CurrentUser.UserId}']/CheckedOut");

                            parent.AppendChild(bookElem);
                            //remove reserved node if it exists. 
                            if (userNode.SelectSingleNode("Reserved") != null)
                            {
                                XmlNode Reserved = userNode.SelectSingleNode("Reserved");
                                userNode.RemoveChild(Reserved);
                            }
                            userNode.OwnerDocument.Save(xmlUserFilePath);
                        }
                    }
                    //show a message box with the due date to the user. 
                    MessageBox.Show($"Your due date is {dueDate}");
                    //switch both booleans to true if this code has executed. 
                    bookFound = true;
                    bookAvailable = true;
                }
                //run this code if the book has been checked out. 
                else if (txtCheckoutBookId.Text == bookId.InnerText && checkedOut != null)
                {
                    bookFound = true;
                }
            }
            //if neither boolean switches to true, error displays as the book does not exist in the system. 
            if (!bookFound && !bookAvailable)
            {
                MessageBox.Show("This book is not recognised \n please contact the librarian");
            }
            //if the book is found but not available, show an error box that says it cannot be checked out. 
            else if (bookFound == true && !bookAvailable)
            {
                MessageBox.Show("This book cannot currently be checked out \n please contact the librarian");
            }
        }
    }
}