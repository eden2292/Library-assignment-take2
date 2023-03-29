using System;
using System.Data;
using System.Windows;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for MemberManage.xaml
    /// </summary>
    public partial class MemberManage : Window
    {
        public MemberManage()
        {
            InitializeComponent();

            DataSet dataset = new DataSet();
            dataset.ReadXml(xmlUserFilePath);
            dgUserInformation.ItemsSource = dataset.Tables[0].DefaultView;
        }

        #region variables
        private String xmlUserFilePath => "UserList.xml";

        private String selectName;
        private String selectEmail;
        private String selectTelephone;
        private String selectFines;
        private String selectTag;

        #endregion variables

        private void updateDatagrid()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml(xmlUserFilePath);
            dgUserInformation.ItemsSource = dataset.Tables[0].DefaultView;
            dgUserInformation.Items.Refresh();
        }

        private void btnMemAdd_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocUser = new XmlDocument();
            xmlDocUser.Load(xmlUserFilePath);

            Random random = new Random();
            String id = Convert.ToString(random.Next(10000000, 99999999));

            XmlNode parentNode = xmlDocUser.CreateElement("User");
            XmlNode userIdNode = xmlDocUser.CreateElement("UserID");
            XmlNode nameNode = xmlDocUser.CreateElement("Name");
            XmlNode emailNode = xmlDocUser.CreateElement("Email");
            XmlNode telephoneNode = xmlDocUser.CreateElement("Telephone");
            XmlNode checkedoutNode = xmlDocUser.CreateElement("CheckedOut");
            XmlNode finesNode = xmlDocUser.CreateElement("Fines");
            XmlNode tagNode = xmlDocUser.CreateElement("Tag");

            userIdNode.InnerText = id;
            nameNode.InnerText = txtManName.Text;
            emailNode.InnerText = txtManEmail.Text;
            telephoneNode.InnerText = txtManTelephone.Text;
            checkedoutNode.InnerText = "";
            finesNode.InnerText = "";
            tagNode.InnerText = txtManTag.Text;

            parentNode.AppendChild(userIdNode);
            parentNode.AppendChild(nameNode);
            parentNode.AppendChild(emailNode);
            parentNode.AppendChild(telephoneNode);
            parentNode.AppendChild(checkedoutNode);
            parentNode.AppendChild(finesNode);
            parentNode.AppendChild(tagNode);

            XmlNode ROOT = xmlDocUser.SelectSingleNode("catalog");

            ROOT.AppendChild(parentNode);

            xmlDocUser.Save(xmlUserFilePath);

            MessageBox.Show($"Success! \n user ID is {id} \n Write in before continuing");

            updateDatagrid();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            StaffHome staffHome = new StaffHome();
            staffHome.Show();
        }

        private void dgUserInformation_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataRowView row = dgUserInformation.SelectedItem as DataRowView;

            if (row != null)
            {
                selectName = row.Row.ItemArray[1].ToString();
                selectEmail = row.Row.ItemArray[2].ToString();
                selectTelephone = row.Row.ItemArray[3].ToString();
                selectTag = row.Row.ItemArray[6].ToString();
                selectFines = row.Row.ItemArray[7].ToString();

                txtManName.Text = selectName;
                txtManEmail.Text = selectEmail;
                txtManTelephone.Text = selectTelephone;
                txtManFines.Text = selectFines;
                txtManTag.Text = selectTag;

            }
        }

        private void btnMemUpdate_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocUser = new XmlDocument();
            xmlDocUser.Load(xmlUserFilePath);

            XmlNode oldUser = xmlDocUser.SelectSingleNode("//User[Name = '" + selectName + "']");
            XmlNode fines = xmlDocUser.SelectSingleNode("//User[Name = '" + selectName + "']/Fines");
            oldUser.ChildNodes.Item(1).InnerText = txtManName.Text;
            oldUser.ChildNodes.Item(2).InnerText = txtManEmail.Text;
            oldUser.ChildNodes.Item(3).InnerText = txtManTelephone.Text;
            oldUser.ChildNodes.Item(5).InnerText = txtManFines.Text;
            oldUser.ChildNodes.Item(6).InnerText = txtManTag.Text;
            oldUser.ChildNodes.Item(7).InnerText = txtManFines.Text;

            xmlDocUser.Save(xmlUserFilePath);
            MessageBox.Show("Success!");

            updateDatagrid();
        }

        private void btnMemRemove_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocUser = new XmlDocument();
            xmlDocUser.Load(xmlUserFilePath);

            XmlNode user = xmlDocUser.SelectSingleNode("//User[Name = '" + selectName + "']");

            user.ParentNode.RemoveChild(user);

            xmlDocUser.Save(xmlUserFilePath);
            MessageBox.Show("Success!");

            updateDatagrid();
        }
    }
}