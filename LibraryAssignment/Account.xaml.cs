using System;
using System.Windows;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        private readonly PramStore _pramStore;

        public Account(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();

            txtEmail.Text = _pramStore.CurrentUser.UserEmail;
            txtName.Text = _pramStore.CurrentUser.UserName;
            txtPhone.Text = _pramStore.CurrentUser.UserPhone;
        }

        private String xmlUserFilePath => "UserList.xml";
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlUserFilePath);
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/User");

        }
    }
}