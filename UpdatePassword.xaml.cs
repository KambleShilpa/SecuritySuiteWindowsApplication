using SecuritySuite.PasswordServices;
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
using static SecuritySuite.AddNewPasswords;
using System.Security.Cryptography;
using static SecuritySuite.ShowAllPassword;

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for UpdatePassword.xaml
    /// </summary>
    public partial class UpdatePassword : Window
    {
        String UserName, UserID, Email, Website, PasswordText, Notes, resultPassword;
        DateTime DateCreated, DateModified;
        public UpdatePassword()
        {
            InitializeComponent();
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void generateButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordTB.Text = await PasswordService.Instance().GeneratePasswordAsync();
        }
        public void validation()
        {
            if (UserNameTB.Text == "" || UserNameTB.Text == null)
            {
                MessageBox.Show("Enter User Name");
            }
            if (UserIDTB.Text == "" || UserIDTB.Text == null)
            {
                MessageBox.Show("Enter User ID");
            }
            if (EmailTB.Text == "" || EmailTB.Text == null)
            {
                MessageBox.Show("Enter Email ID");
            }
            if (PasswordTB.Text == "" || PasswordTB.Text == null)
            {
                MessageBox.Show("Enter Password");
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            int id = ShowAllPassword.ID;
            validation();
            accept();
            SqlConnection cnn = new SqlConnection(LoginPage.connectionString);
            SqlCommand myCommand = new SqlCommand("update Passwords set UserName = @UserName, UserID = @UserID, Email = @Email, Website = @Website, PasswordText = @PasswordText, Notes = @Notes, DateCreated = @DateCreated, DateModified = @DateModified where id=@id", cnn);
            cnn.Open();
            myCommand.Parameters.AddWithValue("@id", id);
            myCommand.Parameters.AddWithValue("@UserName", UserName);
            myCommand.Parameters.AddWithValue("@UserID", UserID);
            myCommand.Parameters.AddWithValue("@Email", Email);
            myCommand.Parameters.AddWithValue("@Website", Website);
            myCommand.Parameters.AddWithValue("@PasswordText", PasswordText);
            myCommand.Parameters.AddWithValue("@Notes", Notes);
            myCommand.Parameters.AddWithValue("@DateCreated", DateCreated);
            myCommand.Parameters.AddWithValue("@DateModified", DateModified);
            myCommand.ExecuteNonQuery();
            MessageBox.Show("Record Updated Successfully");
            cnn.Close();
            this.Close();
        }
        public void accept()
        {
            UserName = UserNameTB.Text;
            UserID = UserIDTB.Text;
            Email = EmailTB.Text;
            Website = WebsiteTB.Text;
            PasswordText = PasswordTB.Text;
            Notes = NotesTB.Text;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;

            SqlConnection cnn = new SqlConnection(LoginPage.connectionString);
            try
            {
                cnn.Open();
                string query = "SELECT PASSWORD FROM Registration_Table where USER_NAME= ' " + LoginPage.username + " '";
                SqlCommand myCommand = new SqlCommand(query, cnn);
                object result = myCommand.ExecuteReader();
                if (result != null)
                {
                    resultPassword = result.ToString();
                }
                MessageBox.Show("Saved Successfully");
                //MessageBox.Show("password from sql: "+resultPassword);
                cnn.Close();
                PasswordText = Encrypt(PasswordText, resultPassword);
                //MessageBox.Show("password from encrypt: " + PasswordText);
                //string p;
                //p = Decrypt(PasswordText, resultPassword);
                //MessageBox.Show("password from decrypt: " + p);
            }
            catch (Exception ex)
            { }

        }
        public static string Encrypt(string TextToEncrypt, string mysecurityKey)
        {
            byte[] MyEncryptedArray = UTF8Encoding.UTF8
               .GetBytes(TextToEncrypt);

            MD5CryptoServiceProvider MyMD5CryptoService = new
               MD5CryptoServiceProvider();

            byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash
               (UTF8Encoding.UTF8.GetBytes(mysecurityKey));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new
               TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;
            Console.WriteLine("Key is: " + mysecurityKey);

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService
               .CreateEncryptor();

            byte[] MyresultArray = MyCrytpoTransform
               .TransformFinalBlock(MyEncryptedArray, 0,
               MyEncryptedArray.Length);

            MyTripleDESCryptoService.Clear();

            return Convert.ToBase64String(MyresultArray, 0,
               MyresultArray.Length);
        }
    }
}
