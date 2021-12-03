using SecuritySuite.PasswordServices;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using static SecuritySuite.Login;

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for AddNewPasswords.xaml
    /// </summary>
    public partial class AddNewPasswords : Page
    {
        SqlConnection cnn;
        String UserName, UserID, Email, Website, PasswordText, Notes, resultPassword;
        DateTime DateCreated, DateModified;
        public AddNewPasswords()
        {
            InitializeComponent();
        }

        private async void generateButton_ClickAsync(object sender, RoutedEventArgs e)
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
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            validation();
            connectionDB();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            UserNameTB.Text = "";
            UserIDTB.Text = "";
            EmailTB.Text = "";
            NotesTB.Text = "";
            WebsiteTB.Text = "";
            PasswordTB.Text = "";
        }
        private void connectionDB()
        {
            accept();
            string Email_ID = LoginPage.email_id;
            cnn = new SqlConnection(LoginPage.connectionString);
            try
            {
                cnn.Open();
                string query = "INSERT INTO Passwords (UserName, UserID, Email, Website, PasswordText, Notes, DateCreated, DateModified, Email_ID)";
                query += " VALUES (@UserName, @UserID, @Email, @Website, @PasswordText, @Notes, @DateCreated, @DateModified, @Email_ID)";

                SqlCommand myCommand = new SqlCommand(query, cnn);

                myCommand.Parameters.AddWithValue("@UserName", UserName);
                myCommand.Parameters.AddWithValue("@UserID", UserID);
                myCommand.Parameters.AddWithValue("@Email", Email);
                myCommand.Parameters.AddWithValue("@Website", Website);
                myCommand.Parameters.AddWithValue("@PasswordText", PasswordText);
                myCommand.Parameters.AddWithValue("@Notes", Notes);
                myCommand.Parameters.AddWithValue("@DateCreated", DateCreated);
                myCommand.Parameters.AddWithValue("@DateModified", DateModified);
                myCommand.Parameters.AddWithValue("@Email_ID", Email_ID);

                myCommand.ExecuteNonQuery();
                MessageBox.Show("Saved Successfully");
                cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection ! " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }
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

            cnn = new SqlConnection(LoginPage.connectionString);
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
                //MessageBox.Show("Saved Successfully");
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
        public static string Decrypt(string TextToDecrypt, string mysecurityKey)
        {
            try
            {
                byte[] MyDecryptArray = Convert.FromBase64String
                   (TextToDecrypt);

                MD5CryptoServiceProvider MyMD5CryptoService = new
                   MD5CryptoServiceProvider();

                byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash
                   (UTF8Encoding.UTF8.GetBytes(mysecurityKey));

                MyMD5CryptoService.Clear();

                var MyTripleDESCryptoService = new
                   TripleDESCryptoServiceProvider();

                MyTripleDESCryptoService.Key = MysecurityKeyArray;


                MyTripleDESCryptoService.Mode = CipherMode.ECB;

                MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

                var MyCrytpoTransform = MyTripleDESCryptoService
                   .CreateDecryptor();

                byte[] MyresultArray = MyCrytpoTransform
                   .TransformFinalBlock(MyDecryptArray, 0,
                   MyDecryptArray.Length);

                MyTripleDESCryptoService.Clear();

                return UTF8Encoding.UTF8.GetString(MyresultArray);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Entered wrong secret key! Try again.", "Error");
                String error = "Entered key invalid.";
                return error;
            }
        }
    }
}
