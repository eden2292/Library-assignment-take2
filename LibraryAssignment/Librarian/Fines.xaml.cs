using System;
using System.Data;
using System.Globalization;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

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

        #endregion variables

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
                selectFine = Decimal.Parse(row.Row.ItemArray[7].ToString()).ToString("C2");

                txtUserEmail.Text = selectEmail;

                if (selectFine != "£0.00")
                {
                    txtEmailTemplate.Text = $"Dear {selectName}, \n We are writing to inform you that you have books that are overdue. \n Please note that your current fine total is {selectFine}. " +
                        $"\n please endeavour to return these items to the library as soon as possible, or you will incur further charges. \n Regards, The library team";
                    btnSend.IsEnabled = true;
                }
                else if (selectFine == "£0.00")
                {
                    txtEmailTemplate.Text = "This user does not have a fine. Do not send notice.";
                    btnSend.IsEnabled = false;
                }
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