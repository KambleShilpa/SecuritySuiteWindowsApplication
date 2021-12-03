using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }
        private void registerTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Registration register = new Registration();
            register.Show();
            this.Close();
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
        private void changePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginEmailID.Text.Length == 0)
            {
                errormessage.Text = "Enter an Email ID.";
                loginEmailID.Focus();
            }
            else
            {
                string userID = loginEmailID.Text;
                string password = loginCurrPassword.Password;
                string Confpassword = loginConfirmPassword.Password;


                SqlConnection con = new SqlConnection(LoginPage_Class.ConString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Registration_Table where Email_ID= '" + userID + "'  and PASSWORD='" + password + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    SqlCommand cmd1 = new SqlCommand("Update Registration_Table set Password = '" + Confpassword + "' where Email_ID = '" + userID + "'", con);
                    cmd1.ExecuteNonQuery();
                    Login login = new Login();
                    login.Show();
                    this.Close();
                }
                else
                {
                    errormessage.Text = "Sorry! Please enter existing EmailId/Password.";
                }
                con.Close();
            }
        }
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        public void Reset()
        {
            loginEmailID.Text = "";
            loginCurrPassword.Password = "";
            loginNewPassword.Password = "";
            loginConfirmPassword.Password = "";
            errormessage.Text = "";
        }
    }
}
