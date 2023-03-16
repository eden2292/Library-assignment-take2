using System;
using System.Data;
using System.Text;
using System.Windows;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        private readonly PramStore _pramStore;

        private String xmlUserFilePath => "UserList.xml";

        public Account(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();

            DataSet dataSet = new DataSet();
            dataSet.ReadXml(xmlUserFilePath);
            dgBookOnLoan.ItemsSource=dataSet.Tables[0].DefaultView;

            txtEmail.Text = _pramStore.CurrentUser.UserEmail;
            txtName.Text = _pramStore.CurrentUser.UserName;
            txtPhone.Text = _pramStore.CurrentUser.UserPhone;

            //creates an empty dataset to receive the XML data
            //Sets the datasource for the datagrid to be the dataset           
            DataView dv = dataSet.Tables[1].DefaultView;
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn column in dv.Table.Columns)
            {
                sb.AppendFormat("[{0}] Like '%{1}%' OR ", column.ColumnName, txtName.Text);
            }
            sb.Remove(sb.Length - 3, 3);
            dgBookOnLoan.ItemsSource = dv;
            dgBookOnLoan.Items.Refresh();

        }



        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }


        private void dgBookOnLoan_Initialized(object sender, EventArgs e)
        {
           
        }
    }
}