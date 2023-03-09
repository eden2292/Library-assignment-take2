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

        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";

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

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlNodeList bookNodes = xmlDocument.DocumentElement.SelectNodes("/library/book");

            //bool bookFound = false;

            foreach(XmlNode book in bookNodes)
            {
                
                XmlNode bookId = book.SelectSingleNode("bookId");

                if(txtReturn.Text == bookId.InnerText)
                {
                   
                    XmlNode checkedOut = book.SelectSingleNode("checkedOut");
                    book.RemoveChild(checkedOut);
                    xmlDocument.Save(xmlBookFilePath);
 
                }
}

        }
    }
}
