using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Xml;
using System.Linq;
using System.Xml.Linq;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        /// <summary>
        /// Create a new readonly pram store. Readonly as this page does not need to edit the stored information.
        /// </summary>
        private readonly PramStore _pramStore;

        private String XmlUserfilePath => "UserList.xml";


        public Account(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();

            //pull information on the user from the pramstore to enter into the text boxes
            txtEmail.Text = _pramStore.CurrentUser.UserEmail;
            txtName.Text = _pramStore.CurrentUser.UserName;
            txtPhone.Text = _pramStore.CurrentUser.UserPhone;
            txtFines.Text = _pramStore.CurrentUser.UserFines;

            //load in the xml document as an xdoc to enable better filtering for the datagrid
            XDocument xDoc = XDocument.Load(XmlUserfilePath);
            //get the information to be included in the datagrid. 
            XElement User = xDoc.Root.Elements("User").Where(x => x.Element("UserID").Value == _pramStore.CurrentUser.UserId).SingleOrDefault();
            //find the checked out book nodes for this specific user and store them to the books variable
            var books = User.Element("CheckedOut").Elements("Book").ToList().Select(x => new { BookTitle = x.Element("BookTitle").Value, DueBackDate = x.Element("DueDate").Value }).ToList();
            //load the items from the books variable into the datagrid, then update it. 
            dgBookOnLoan.ItemsSource = books;
            dgBookOnLoan.Items.Refresh();

        }

        /// <summary>
        /// Return to the home page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }
    }
}