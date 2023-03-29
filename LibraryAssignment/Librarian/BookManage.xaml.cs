using System;
using System.Data;
using System.Windows;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for BookManage.xaml
    /// </summary>
    public partial class BookManage : Window
    {
        public BookManage()
        {
            InitializeComponent();

            DataSet dataset = new DataSet();
            dataset.ReadXml(xmlBookFilePath);
            dgLibraryInventory.ItemsSource = dataset.Tables[0].DefaultView;
        }

        #region variables

        private String xmlBookFilePath => "LibraryInventory.xml";

        private String selectTitle;
        private String selectAuthor;
        private String selectYear;
        private String selectPublisher;
        private String selectIsbn;
        private String selectCategory;
        private String selectId;
        private String selectValue;

        #endregion variables

        private void updateDatagrid()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml(xmlBookFilePath);
            dgLibraryInventory.ItemsSource = dataset.Tables[0].DefaultView;
            dgLibraryInventory.Items.Refresh();
        }

        private void btnManAdd_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocBooks = new XmlDocument();
            xmlDocBooks.Load(xmlBookFilePath);

            Random random = new Random();
            String id = Convert.ToString(random.Next(1000, 9999));

            XmlNode parentNode = xmlDocBooks.CreateElement("book");
            XmlNode titleNode = xmlDocBooks.CreateElement("title");
            XmlNode authorNode = xmlDocBooks.CreateElement("author");
            XmlNode yearNode = xmlDocBooks.CreateElement("year");
            XmlNode publisherNode = xmlDocBooks.CreateElement("publisher");
            XmlNode isbnNode = xmlDocBooks.CreateElement("isbn");
            XmlNode categoryNode = xmlDocBooks.CreateElement("category");
            XmlNode valueNode = xmlDocBooks.CreateElement("value");
            XmlNode bookIdNode = xmlDocBooks.CreateElement("bookId");

            titleNode.InnerText = txtManTitle.Text;
            authorNode.InnerText = txtManAuthor.Text;
            yearNode.InnerText = txtManYear.Text;
            publisherNode.InnerText = txtManPublisher.Text;
            isbnNode.InnerText = txtManIsbn.Text;
            categoryNode.InnerText = txtManCategory.Text;
            valueNode.InnerText = txtManValue.Text;
            bookIdNode.InnerText = id;

            parentNode.AppendChild(titleNode);
            parentNode.AppendChild(authorNode);
            parentNode.AppendChild(yearNode);
            parentNode.AppendChild(publisherNode);
            parentNode.AppendChild(isbnNode);
            parentNode.AppendChild(categoryNode);
            parentNode.AppendChild(valueNode);
            parentNode.AppendChild(bookIdNode);

            XmlNode ROOT = xmlDocBooks.SelectSingleNode("library");

            ROOT.AppendChild(parentNode);

            xmlDocBooks.Save(xmlBookFilePath);

            MessageBox.Show($"Success. \n Book ID is {id} \n Write in before continuing.");

            updateDatagrid();
        }

        private void dgLibraryInventory_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataRowView row = dgLibraryInventory.SelectedItem as DataRowView;

            if ((row != null))
            {
                selectTitle = row.Row.ItemArray[0].ToString();
                selectAuthor = row.Row.ItemArray[1].ToString();
                selectYear = row.Row.ItemArray[2].ToString();
                selectPublisher = row.Row.ItemArray[3].ToString();
                selectIsbn = row.Row.ItemArray[4].ToString();
                selectCategory = row.Row.ItemArray[5].ToString();
                selectValue = row.Row.ItemArray[6].ToString();
                selectId = row.Row.ItemArray[7].ToString();

                txtManTitle.Text = selectTitle;
                txtManAuthor.Text = selectAuthor;
                txtManYear.Text = selectYear;
                txtManPublisher.Text = selectPublisher;
                txtManIsbn.Text = selectIsbn;
                txtManCategory.Text = selectCategory;
                txtManValue.Text = selectValue;
            }
        }

        private void btnManUpdate_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocBooks = new XmlDocument();
            xmlDocBooks.Load(xmlBookFilePath);

            XmlNode oldBook = xmlDocBooks.SelectSingleNode("//book[title ='" + selectTitle + "']");

            oldBook.ChildNodes.Item(0).InnerText = txtManTitle.Text;
            oldBook.ChildNodes.Item(1).InnerText = txtManAuthor.Text;
            oldBook.ChildNodes.Item(2).InnerText = txtManYear.Text;
            oldBook.ChildNodes.Item(3).InnerText = txtManPublisher.Text;
            oldBook.ChildNodes.Item(4).InnerText = txtManIsbn.Text;
            oldBook.ChildNodes.Item(5).InnerText = txtManCategory.Text;
            oldBook.ChildNodes.Item(6).InnerText = txtManValue.Text;

            xmlDocBooks.Save(xmlBookFilePath);
            MessageBox.Show("Success!");

            updateDatagrid();
        }

        private void btnManRemove_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocBooks = new XmlDocument();
            xmlDocBooks.Load(xmlBookFilePath);

            XmlNode book = xmlDocBooks.SelectSingleNode("//book[bookId = '" + selectId + "']");

            book.ParentNode.RemoveChild(book);

            xmlDocBooks.Save(xmlBookFilePath);
            MessageBox.Show("Success!");

            updateDatagrid();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            StaffHome staffHome = new StaffHome();
            staffHome.Show();
        }
    }
}