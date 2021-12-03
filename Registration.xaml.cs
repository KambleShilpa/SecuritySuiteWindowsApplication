using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
            UserNameTB.Focus();
        }

        private void loginTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmailTB.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                EmailTB.Focus();
            }
            else if (!Regex.IsMatch(EmailTB.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                EmailTB.Select(0, EmailTB.Text.Length);
                EmailTB.Focus();
            }
            else
            {
                string name = UserNameTB.Text;
                string email = EmailTB.Text;
                string loginName = UserIDTB.Text;
                string password = PasswordTB.Password;
                if (PasswordTB.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    PasswordTB.Focus();
                }
                else if (ConfPasswordTB.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                    ConfPasswordTB.Focus();
                }
                else if (PasswordTB.Password != ConfPasswordTB.Password)
                {
                    errormessage.Text = "Confirm password must be same as password.";
                    ConfPasswordTB.Focus();
                }
                else
                {
                    errormessage.Text = "";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(LoginPage.ConString))
                        {

                            con.Open();
                            SqlCommand cmd = new SqlCommand("Insert into Registration_Table (EMAIL_ID,NAME,USER_NAME,PASSWORD) values('" + email + "','" + name + "','" + loginName + "','" + password + "')", con);
                            cmd.ExecuteNonQuery();
                            errormessage.Text = "You have Registered successfully.";
                            con.Close();
                            Reset();
                            Login login = new Login();
                            login.Show();
                            this.Close();
                        }
                    }
                    catch
                    {
                        errormessage.Text = "Already Registered !";
                    }
                }
            }
        }
        public void Reset()
        {
            UserNameTB.Text = "";
            UserIDTB.Text = "";
            EmailTB.Text = "";
            PasswordTB.Password = "";
            ConfPasswordTB.Password = "";
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
    }
}
