using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        #region variables
        private readonly PramStore _pramStore;
        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";
        public String currentUserId;
        public String selectTitle;
        public String selectAuthor;
        public String selectId;
        #endregion

        public Search(PramStore _pramStore)
        {
            InitializeComponent();
            currentUserId = _pramStore.CurrentUser.UserId;
            DataSet dataSet = new DataSet();
            //Read the xml file into the dataset
            dataSet.ReadXml(xmlBookFilePath);
            //get the source for the data
            dgResults.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //creates an empty dataset to receive the XML data
            DataSet dataSet = new DataSet();
            //Reads the XML file into the dataset
            dataSet.ReadXml(xmlBookFilePath);
            //Sets the datasource for the datagrid to be the dataset           
            DataView dv = dataSet.Tables[0].DefaultView;
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn column in dv.Table.Columns)
            {
                sb.AppendFormat("[{0}] Like '%{1}%' OR ", column.ColumnName, txtSearchTitle.Text);
            }
            sb.Remove(sb.Length - 3, 3);
            dv.RowFilter = sb.ToString();
            dgResults.ItemsSource = dv;
            dgResults.Items.Refresh();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }

        private void txtSearchTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSearchTitle.Clear();
        }


        private void dgResults_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataRowView row = dgResults.SelectedItem as DataRowView;

            selectTitle = row.Row.ItemArray[0].ToString();
            selectAuthor = row.Row.ItemArray[1].ToString();
            selectId = row.Row.ItemArray[6].ToString();
        }

        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocBook = new XmlDocument();
            xmlDocBook.Load(xmlBookFilePath);

            XmlDocument xmlDocUser = new XmlDocument();
            xmlDocUser.Load(xmlUserFilePath);

            XmlNodeList xmlUserNodeList = xmlDocUser.DocumentElement.SelectNodes("/catalog/User");
            XmlNodeList xmlBookNodeList = xmlDocBook.DocumentElement.SelectNodes("/library/book");

            foreach(XmlNode xmlUserNode in xmlUserNodeList)
            {
                XmlNode user = xmlUserNode.SelectSingleNode("UserID");

                if(currentUserId == user.InnerText)
                {
                    XmlElement searchTitle = xmlDocUser.CreateElement("Reserved");
                    searchTitle.InnerText = selectTitle;

                    xmlUserNode.AppendChild(searchTitle);
                    xmlUserNode.OwnerDocument.Save(xmlUserFilePath);

                    MessageBox.Show($"successfully reserved \n {selectTitle}");
                }
            }

            foreach(XmlNode xmlBookNode in xmlBookNodeList)
            {
                XmlNode book = xmlBookNode.SelectSingleNode("bookId");

                if (book.InnerText == selectId)
                {
                    XmlElement searchId = xmlDocBook.CreateElement("reserved");
                    searchId.InnerText = currentUserId;

                    xmlBookNode.AppendChild(searchId);
                    xmlBookNode.OwnerDocument.Save(xmlBookFilePath);
                }
            }
        }
    }
}