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
using System.Windows.Shapes;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Window
    {
        public Checkout()
        {
            InitializeComponent();
        }

        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";
        private string dueDate = DateTime.Now.AddMonths(1).ToShortDateString();


        private void txtCheckoutBookId_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCheckoutBookId.Clear();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void btnCheckoutBook_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);
            XmlDocument xmlDocUser = new XmlDocument();
            xmlDocUser.Load(xmlUserFilePath);
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/library/book");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                XmlNode bookId = xmlNode.SelectSingleNode("bookId");

                if (txtCheckoutBookId.Text == bookId.InnerText)
                {
                    XmlNode newElem = xmlDocument.CreateNode("element", "checkedOut", "");
                    newElem.InnerText = dueDate;
                    xmlNode.AppendChild(newElem);
                    xmlNode.OwnerDocument.Save(xmlBookFilePath);
                    MessageBox.Show($"Your due date is {dueDate}");
                }
            }
        }
    }
}
