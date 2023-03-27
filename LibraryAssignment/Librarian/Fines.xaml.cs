﻿using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using System.Net.Mime;


namespace LibraryAssignment.Librarian
{
    /// <summary>
    /// Interaction logic for Fines.xaml
    /// </summary>
    public partial class Fines : Window
    {
        #region variables

        private String XmlUserfilePath => "UserList.xml";
        private String selectEmail;
        private String selectName;
        private String selectFine;

        private String today = DateTime.Now.ToShortDateString();

        #endregion
        public Fines()
        {
            InitializeComponent();

            DataSet dataset = new DataSet();
            dataset.ReadXml(XmlUserfilePath);
            dgUserFines.ItemsSource = dataset.Tables[0].DefaultView;
        }


        private void dgUserFines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = dgUserFines.SelectedItem as DataRowView;

            if (row != null)
            {
                selectEmail = row.Row.ItemArray[2].ToString();
                selectName = row.Row.ItemArray[1].ToString();
                selectFine = row.Row.ItemArray[5].ToString();

                txtUserEmail.Text = selectEmail;

                txtEmailTemplate.Text = $"Dear {selectName}, \n We are writing to inform you that you have books that are overdue. \n Please note that your current fine total is {selectFine}. " +
                    $"\n please endeavour to return these items to the library as soon as possible, or you will incur further charges. \n Regards, The library team";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            StaffHome home = new StaffHome();
            home.Show();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string to = txtUserEmail.Text;
            string from = "Librarian@library.gov.uk";
            string subject = "Notice of fines";
            string body = txtEmailTemplate.Text;
            MailMessage message = new MailMessage(from, to, subject, body);
            //this is where the code goes to send emails, however I am unable to do this without my own SMTP server.

            MessageBox.Show("email notice sent");
        }        
    }
}