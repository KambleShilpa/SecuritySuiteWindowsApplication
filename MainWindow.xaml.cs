using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this window?",
            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
            {
                // Do not close the window  
            }

        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
        private void registerButton(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            this.Close();
        }
    }
}
