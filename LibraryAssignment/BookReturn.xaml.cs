﻿using System;
using System.Windows;
using System.Xml;

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for BookReturn.xaml
    /// </summary>
    public partial class BookReturn : Window
    {
        private readonly PramStore _pramStore;
        public String currentUserId;
        public String currentUserBooks;
        public String title;
        private bool bookFound = false;

        private String xmlBookFilePath => "LibraryInventory.xml";
        private String xmlUserFilePath => "UserList.xml";
        private string newDate = DateTime.Now.AddMonths(1).ToShortDateString();

        public BookReturn(PramStore pramStore)
        {
            _pramStore = pramStore;
            InitializeComponent();
            currentUserId = _pramStore.CurrentUser.UserId;
            currentUserBooks = _pramStore.CurrentUser.UserBooks;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }

        private void txtReturn_GotFocus(object sender, RoutedEventArgs e)
        {
            txtReturn.Clear();
        }

        #region return

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlDocument userDocument = new XmlDocument();
            userDocument.Load(xmlUserFilePath);

            XmlNodeList bookNodes = xmlDocument.DocumentElement.SelectNodes("/library/book");
            XmlNodeList userNodes = userDocument.DocumentElement.SelectNodes("/catalog/User");

            foreach (XmlNode book in bookNodes)
            {
                XmlNode bookId = book.SelectSingleNode("bookId");

                if (txtReturn.Text == bookId.InnerText)
                {
                    XmlNode checkedOut = book.SelectSingleNode("checkedOut");
                    title = book.SelectSingleNode("title").InnerText;
                    book.RemoveChild(checkedOut);
                    MessageBox.Show("Successfully returned!");
                    bookFound = true;
                }
            }
            if (bookFound == false)
            {
                MessageBox.Show("An error has occured \n please contact the librarian");
            }

            foreach (XmlNode user in userNodes)
            {
                XmlNode bookTitle = user.SelectSingleNode("/catalog/User/CheckedOut/BookTitle");
                XmlNode dueDate = user.SelectSingleNode("/catalog/User/CheckedOut/DueDate");

                if (bookTitle.InnerText == title)
                {
                    bookTitle.ParentNode.RemoveChild(bookTitle);
                    dueDate.ParentNode.RemoveChild(dueDate);
                }
            }
            xmlDocument.Save(xmlBookFilePath);
            userDocument.Save(xmlUserFilePath);
        }

        #endregion return

        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlBookFilePath);

            XmlNodeList bookNodes = xmlDocument.DocumentElement.SelectNodes("/library/book");

            foreach (XmlNode book in bookNodes)
            {
                XmlNode bookId = book.SelectSingleNode("bookId");

                if (txtReturn.Text == bookId.InnerText)
                {
                    book.SelectSingleNode("checkedOut").InnerText = newDate;
                    MessageBox.Show($"Your new due date is \n {newDate}");
                    bookFound = true;
                }
                if (!bookFound)
                {
                    MessageBox.Show("An error has occured \n please contact the librarian");
                }
                xmlDocument.Save(xmlBookFilePath);
            }
        }
    }
}