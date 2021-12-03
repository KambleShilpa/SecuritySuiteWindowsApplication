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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SecuritySuite.SearchPassword;
namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for EditDeleteDecrypt.xaml
    /// </summary>
    public partial class EditDeleteDecrypt : Page
    {
        public EditDeleteDecrypt()
        {
            InitializeComponent();
        }
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePassword update = new UpdatePassword();
            update.Show();
        }

        private void deleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            Delete delete = new Delete();
            delete.Show();
        }
    }
}
