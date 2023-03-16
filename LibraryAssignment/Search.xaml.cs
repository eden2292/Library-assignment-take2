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
        private readonly PramStore _pramStore;
        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";
        public String currentUserId;

        public Search()
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

        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlDocument xmlDocUser = new XmlDocument();
            xmlDocUser.Load(xmlUserFilePath);


        }
    }
}