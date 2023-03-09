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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            _pramStore = new PramStore();
            InitializeComponent();
        }

        private String xmlUserFilePath => "UserList.xml";
        private bool AccessSuccess;

        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {
            //Access XML file with user information in. 
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlUserFilePath);
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/User");

            //loop through each node in the XML file and check against "User ID"
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                XmlNode id = xmlNode.SelectSingleNode("UserID");
                //If the User ID matches a node in the XML file, check the tag node. 
                if (txtUserId.Text == id.InnerText)
                {
                    XmlNode tag = xmlNode.SelectSingleNode("Tag");

                    _pramStore.CurrentUser = new UserDetails
                    {
                        UserName = xmlNode.SelectSingleNode("Name").InnerText,
                        UserPhone = xmlNode.SelectSingleNode("Telephone").InnerText,
                        UserEmail = xmlNode.SelectSingleNode("Email").InnerText,
                        UserBooks = xmlNode.SelectSingleNode("CheckedOut").InnerText,
                        UserId = id.InnerText,
                        UserTag = tag.InnerText,
                    };

                    //switch to landing page based on the "tag" node. 
                    if (tag.InnerText == "Staff")
                    {
                        Hide();
                        StaffHome staffHome = new StaffHome();
                        staffHome.Show();
                    }
                    else if (tag.InnerText == "Member")
                    {
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