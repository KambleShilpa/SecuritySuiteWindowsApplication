using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static SecuritySuite.Login;

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for Delete.xaml
    /// </summary>
    public partial class Delete : Window
    {
        public Delete()
        {
            InitializeComponent();
        }
        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            int id = ShowAllPassword.ID;
            SqlConnection cnn = new SqlConnection(LoginPage.connectionString);
            SqlCommand myCommand = new SqlCommand("delete Passwords where ID=@id", cnn);
            cnn.Open();
            myCommand.Parameters.AddWithValue("@id", id);
            myCommand.ExecuteNonQuery();
            cnn.Close();
            MessageBox.Show("Record Deleted Successfully!");
            this.Close();
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
