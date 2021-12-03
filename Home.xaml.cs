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

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }
        private void encryption_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("Encryption.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this window?",
            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
            {
            }
        }

        private void Hide_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("Hide.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void Unhide_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("Unhide.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void decryption_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("Decryption.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void help_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("Help.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void Contact_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("Contact.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void listOfFiles_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("ListOfFilesEnDecrypted.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void lock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("Lock.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void unlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("Unlock.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void logout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Do you want to logout?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MainWindow mn = new MainWindow();
                mn.Show();
                this.Close();
            }
        }

        private void password_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainContentFrame.Navigate(new System.Uri("PasswordManager.xaml",
             UriKind.RelativeOrAbsolute));
        }
    }
}
