using System;
using System.Collections.Generic;
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
        }

        #region variables

        private String xmlBookFilePath => "LibraryInventory.xml";
        private List<BookDetails> books = new List<BookDetails>();

        #endregion variables

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
            XmlNode bookIdNode = xmlDocBooks.CreateElement("bookId");

            titleNode.InnerText = txtManTitle.Text;
            authorNode.InnerText = txtManAuthor.Text;
            yearNode.InnerText = txtManYear.Text;
            publisherNode.InnerText = txtManPublisher.Text;
            isbnNode.InnerText = txtManIsbn.Text;
            categoryNode.InnerText = txtManCategory.Text;
            bookIdNode.InnerText = id;

            parentNode.AppendChild(titleNode);
            parentNode.AppendChild(authorNode);
            parentNode.AppendChild(yearNode);
            parentNode.AppendChild(publisherNode);
            parentNode.AppendChild(isbnNode);
            parentNode.AppendChild(categoryNode);
            parentNode.AppendChild(bookIdNode);

            XmlNode ROOT = xmlDocBooks.SelectSingleNode("library");

            ROOT.AppendChild(parentNode);

            xmlDocBooks.Save(xmlBookFilePath);

            MessageBox.Show($"Success. Book ID is\n {id}");
        }
    }
}