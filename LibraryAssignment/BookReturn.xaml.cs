using System.Windows;


namespace LibraryAssignment
{
    /// <summary>
    /// Interaction logic for BookReturn.xaml
    /// </summary>
    public partial class BookReturn : Window
    { 
        private readonly PramStore _pramStore;
    
        public BookReturn(PramStore pramStore)
        {
        _pramStore = pramStore;
        InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
            UserHome home = new UserHome(_pramStore);
            home.Show();
        }

      
    }
}
