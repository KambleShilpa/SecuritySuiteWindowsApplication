using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            loginUserID.Focus();
        }
        public static class LoginPage
        {
            public static string username { get; set; }
            public static string email_id { get; set; }
            public static string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            //other members, fields, methods
            public static string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        public static class LoginPage_Class
        {
            public static string username { get; set; }
            public static string email_id { get; set; }
            public static string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            //other members, fields, methods
            public static string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        public void Reset()
        {
            loginUserID.Text = "";
            loginPassword.Password = "";
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginUserID.Text.Length == 0)
            {
                errormessage.Text = "Enter an UserID.";
                loginUserID.Focus();
            }
            else
            {
                string userID = loginUserID.Text;
                string password = loginPassword.Password;

                SqlConnection con = new SqlConnection(LoginPage_Class.ConString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Registration_Table where USER_NAME='" + userID + "'  and PASSWORD='" + password + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    LoginPage.username = loginUserID.Text;
                    SqlCommand cmd2 = new SqlCommand("Select Email_ID from Registration_Table where USER_NAME='" + LoginPage.username + "'", con);
                    //cmd2.CommandType = CommandType.Text;
                    String emailid = cmd2.ExecuteScalar().ToString();
                    LoginPage.email_id = emailid;
                    //Registration r = new Registration();
                    //r.Show();                    
                    Home home = new Home();
                    home.Show();
                    this.Close();
                }
                else
                {
                    errormessage.Text = "Sorry! Please enter existing EmailId/Password.";
                }
                con.Close();
            }
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
        private void registerTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Registration register = new Registration();
            register.Show();
            this.Close();
        }

        private void ForgotPswdButton_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword fp = new ForgotPassword();
            fp.Show();
            this.Close();
        }
    }
}
