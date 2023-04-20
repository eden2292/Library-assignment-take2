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

        #endregion variables

        public Search(PramStore pramStore)
        {
            InitializeComponent();
            _pramStore = pramStore;
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
            //filter results in the datagrid by information entered into the text box by the user
            foreach (DataColumn column in dv.Table.Columns)
            {
                sb.AppendFormat("[{0}] Like '%{1}%' OR ", column.ColumnName, txtSearchTitle.Text);
            }
            sb.Remove(sb.Length - 3, 3);
            dv.RowFilter = sb.ToString();
            dgResults.ItemsSource = dv;
            //refresh the datagrid to show the filtered results.
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

        //when the user selects a book from the grid, pull the necessary information from it for the reservation to be made
        private void dgResults_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //take the whole row from the selected cell
            DataRowView row = dgResults.SelectedItem as DataRowView;
            //locate the cells with title, author and ID information in and add them to a string variable.
            selectTitle = row.Row.ItemArray[0].ToString();
            selectAuthor = row.Row.ItemArray[1].ToString();
            selectId = row.Row.ItemArray[6].ToString();
        }
        #region reservation
        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlBookDoc = new XmlDocument();
            xmlBookDoc.Load(xmlBookFilePath);

            XmlDocument xmlUserDoc = new XmlDocument();
            xmlUserDoc.Load(xmlUserFilePath);

            XmlNodeList xmlUserNodeList = xmlUserDoc.DocumentElement.SelectNodes("/catalog/User");
            XmlNodeList xmlBookNodeList = xmlBookDoc.DocumentElement.SelectNodes("/library/book");
            //if statement to run the code provided the selection is not null - avoid null reference exceptions. 
            if (dgResults.SelectedItem != null)
            {
                foreach (XmlNode userNode in xmlUserNodeList)
                {
                    XmlNode user = userNode.SelectSingleNode("UserID");

                    if (currentUserId == user.InnerText)
                    {
                        //add reservation node to the user that contains the title of the book. 
                        XmlElement searchTitle = xmlUserDoc.CreateElement("Reserved");
                        searchTitle.InnerText = selectTitle;

                        userNode.AppendChild(searchTitle);
                        userNode.OwnerDocument.Save(xmlUserFilePath);
                        //show the book that the user has reserved. 
                        MessageBox.Show($"successfully reserved \n {selectTitle}");
                    }
                }

                foreach (XmlNode xmlBookNode in xmlBookNodeList)
                {
                    XmlNode book = xmlBookNode.SelectSingleNode("bookId");

                    if (book.InnerText == selectId)
                    {
                        //add a node to the reserved book that holds the user ID
                        XmlElement searchId = xmlBookDoc.CreateElement("reserved");
                        searchId.InnerText = currentUserId;

                        xmlBookNode.AppendChild(searchId);
                        xmlBookNode.OwnerDocument.Save(xmlBookFilePath);
                    }
                }
            }
            else if(dgResults.SelectedItem == null)
            {
                MessageBox.Show("no book selected");
            }
        }
        #endregion
    }
}