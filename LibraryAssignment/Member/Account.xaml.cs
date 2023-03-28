using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Xml;

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
        private String XmlBookFilePath => "LibraryInventory.xml";

        private double lateBookValue;

        public Account(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();

            //create a data set that holds the information from the xml file to be displayed in a human readable format.
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(XmlUserfilePath);
            dgBookOnLoan.ItemsSource = dataSet.Tables[0].DefaultView;

            //pull information on the user from the pramstore to enter into the text boxes
            txtEmail.Text = _pramStore.CurrentUser.UserEmail;
            txtName.Text = _pramStore.CurrentUser.UserName;
            txtPhone.Text = _pramStore.CurrentUser.UserPhone;
            txtFines.Text = _pramStore.CurrentUser.UserFines;

            XmlDocument xmlUserDoc = new XmlDocument();
            XmlDocument xmlBookDoc = new XmlDocument(); 

            xmlUserDoc.Load(XmlUserfilePath);
            xmlBookDoc.Load(XmlBookFilePath);

            XmlNodeList userNodes = xmlUserDoc.DocumentElement.SelectNodes("/catalog/User");
            XmlNodeList bookNodes = xmlBookDoc.DocumentElement.SelectNodes("/library/book");

            foreach(XmlNode xmlBookNode in bookNodes)
            {
                XmlNode title = xmlBookNode.SelectSingleNode("title");
                XmlNode checkedOut = xmlUserDoc.SelectSingleNode($"/catalog/User[UserID ='{_pramStore.CurrentUser.UserId}']/CheckedOut/BookTitle");

                if (title.InnerText == checkedOut.InnerText)
                {
                    XmlNode value = xmlBookDoc.SelectSingleNode($"/library/book[title='{title.InnerText}']");
                    value = value.ChildNodes.Item(6);
                    lateBookValue = Convert.ToDouble(value.InnerText);

                }
            }
            
            foreach(XmlNode xmlUserNode in userNodes)
            {
                XmlNode id = xmlUserNode.SelectSingleNode("UserID");
                XmlNode dueDate = xmlUserNode.SelectSingleNode($"/catalog/User[UserID ='{_pramStore.CurrentUser.UserId}']/CheckedOut/DueDate");

                if(dueDate != null)
                {
                    if (_pramStore.CurrentUser.UserId == id.InnerText && Convert.ToDateTime(dueDate.InnerText) < DateTime.Now)
                    {
                        DateTime due = Convert.ToDateTime(dueDate.InnerText);
                        DateTime today = DateTime.Now.Date;

                        TimeSpan daysLate = today.Subtract(due);

                        double days = daysLate.TotalDays;
                        double minusGrace = days - 7;

                        double forCalc = lateBookValue / 100;

                        double fine = forCalc * (minusGrace / 7);

                        XmlNode parentNode = xmlUserNode.SelectSingleNode($"/catalog/User[UserID ='{_pramStore.CurrentUser.UserId}']");
                        XmlNode Fines = xmlUserDoc.CreateElement("Fines");

                        Fines.InnerText = Convert.ToString(fine);
                        
                        parentNode.AppendChild(Fines);

                        xmlUserDoc.Save(XmlUserfilePath);

                    }
                }
            }

            //Sets the datasource for the datagrid to be the dataset - tables[1] to display the child nodes of the checked out node (one layer deeper)          
            DataView dv = dataSet.Tables[1].DefaultView;
            StringBuilder sb = new StringBuilder();

            //display information in the datagrid, filtered by the name of the user to only display their books.
            foreach (DataColumn column in dv.Table.Columns)
            {
                sb.AppendFormat("[{0}] Like '%{1}%' OR ", column.ColumnName, _pramStore.CurrentUser.UserId);
            }
            sb.Remove(sb.Length - 3, 3);
            dgBookOnLoan.ItemsSource = dv;
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