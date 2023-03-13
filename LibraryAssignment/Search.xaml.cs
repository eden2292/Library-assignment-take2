using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        private readonly PramStore _pramStore;
        private String xmlBookFilePath => "LibraryInventory.xml";
        public Search()
        {
            InitializeComponent();

            DataSet dataSet = new DataSet();
            //Read the xml file into the dataset
            dataSet.ReadXml(xmlBookFilePath);
            //get the source for the data
            dgResults.ItemsSource = dataSet.Tables[0].DefaultView;
        }

      

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            


            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }
    }
}
