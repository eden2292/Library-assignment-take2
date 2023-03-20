using System;
using System.Windows;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private PramStore _pramStore;

        public MainWindow()
        {        
            //create a new instance of Pramstore to hold user information added to CurrentUser.cs
            _pramStore = new PramStore();
            InitializeComponent();
        }

        #region variables

        //a string to hold the locations of xmlfiles. This is the only place a file name needs to be changed (n.b. on each page)
        private String xmlUserFilePath => "UserList.xml";
        private String xmlBookFilePath => "LibraryInventory.xml";

        private bool AccessSuccess;
        private bool ReservationAvailable;

        #endregion

        //code to run when "Log In" button is clicked. 
        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {
            //accessing files
            //create a new instance of an xml document for the user information
            XmlDocument xmlDocUser = new XmlDocument();
            //load document into this instance
            xmlDocUser.Load(xmlUserFilePath);
            //build a list of nodes from what is available in the xml file. 
            XmlNodeList xmlUserNodeList = xmlDocUser.DocumentElement.SelectNodes("/catalog/User");

            //repeat above steps for books. 
            XmlDocument xmlDocBooks = new XmlDocument();
            xmlDocBooks.Load(xmlBookFilePath);
            XmlNodeList xmlBookNodeList = xmlDocBooks.DocumentElement.SelectNodes("/library/book");

            //loop through each node in the XML file and check against "User ID"
            foreach (XmlNode xmlUserNode in xmlUserNodeList)
            {
                //find each instance of the "UserID" node and hold it in the XmlNode "Id"
                XmlNode id = xmlUserNode.SelectSingleNode("UserID");

                //check the held "User Id node" against the information entered in the text box "txtUserId". 
                if (txtUserId.Text == id.InnerText)
                {
                    //find any instances of "Tag" in the users information - only one per user. 
                    XmlNode tag = xmlUserNode.SelectSingleNode("Tag");

                    //store user information in the global parameters (PramStore.cs)
                    _pramStore.CurrentUser = new UserDetails
                    {
                        UserName = xmlUserNode.SelectSingleNode("Name").InnerText,
                        UserPhone = xmlUserNode.SelectSingleNode("Telephone").InnerText,
                        UserEmail = xmlUserNode.SelectSingleNode("Email").InnerText,
                        UserBooks = xmlUserNode.SelectSingleNode("CheckedOut").InnerText,
                        UserId = id.InnerText,
                        UserTag = tag.InnerText,
                    };


                    //check if the user has any reserved books
                    if (xmlUserNode.SelectSingleNode("Reserved") != null)
                    {
                        String reserved = xmlUserNode.SelectSingleNode("Reserved").InnerText;

                        foreach(XmlNode xmlBookNode in xmlBookNodeList)
                        {
                            if(reserved == xmlDocBooks.SelectSingleNode("//title").InnerText)
                            {
                                XmlNode checkedOut = xmlBookNode.SelectSingleNode("checkedOut");

                                if(checkedOut == null)
                                {
                                    ReservationAvailable = true;
                                }
                            }
                        }
                    }
                    //switch to landing page based on the "tag" node.
                    if (tag.InnerText == "Staff")
                    {
                        Hide();
                        StaffHome staffHome = new StaffHome();
                        staffHome.Show();
                    }
                    else if (tag.InnerText == "Member")
                    {
                        if(ReservationAvailable == true)
                        {
                            MessageBox.Show("Your reserved book is available");
                        }
                        Hide();
                        UserHome userHome = new UserHome(_pramStore);
                        userHome.Show();
                        AccessSuccess = true;
                    }
                }
            }

            //If previously instantiated bool has not changed value to "true", a message box shows.
            if (AccessSuccess != true)
            {
                MessageBox.Show("User ID does not match our records");
            }
        }

        private void txtUserId_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUserId.Clear();
        }
    }
}