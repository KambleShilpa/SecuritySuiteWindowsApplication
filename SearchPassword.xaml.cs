using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for SearchPassword.xaml
    /// </summary>
    public partial class SearchPassword : Page
    {
        string resultPassword, PasswordText;
        String searchText, lookingFor, options;
        public static int ID;
        public SearchPassword()
        {
            InitializeComponent();
        }
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            searchText = searchTextTB.Text;
            lookingFor = lookingforComboBox.SelectedItem.ToString();
            options = optionsComboBox.SelectedItem.ToString();
            if (optionsComboBox.SelectedItem == null)
            {
                optionsComboBox.SelectedItem = "Equals";
                options = "Equals";
            }
            if (lookingforComboBox.SelectedItem == null)
            {
                lookingforComboBox.SelectedItem = "UserName";
                lookingFor = "UserName";
            }
            searchDatabase();
        }
        private void searchDatabase()
        {
            string CmdString = string.Empty;
            //SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=aesDatabase;persist security info=True; Integrated Security=SSPI");
            SqlConnection con = new SqlConnection(LoginPage.connectionString);
            con.Open();
            if (lookingFor.EndsWith("UserName"))
            {
                if (options.EndsWith("Contains"))
                {
                    CmdString = "SELECT * FROM Passwords where UserName LIKE '%" + searchText + "%' AND Email_ID ='" + LoginPage.email_id + "'";
                }
            }
            else if (lookingFor.EndsWith("UserID"))
            {
                if (options.EndsWith("Contains"))
                {
                    CmdString = "SELECT * FROM Passwords where UserID LIKE '%" + searchText + "%' AND Email_ID ='" + LoginPage.email_id + "'";
                }
                else if (options.EndsWith("Equals"))
                { CmdString = "select * from Passwords where UserID = '" + searchText + "' AND Email_ID ='" + LoginPage.email_id + "'";
                }
            }
            else if (lookingFor.EndsWith("Email"))
            {
                if (options.EndsWith("Contains"))
                { CmdString = "SELECT * FROM Passwords where Email LIKE '%" + searchText + "%' AND Email_ID ='" + LoginPage.email_id + "'";
                }
                else if (options.EndsWith("Equals"))
                { CmdString = "select * from Passwords where Email = '" + searchText + "' AND Email_ID ='" + LoginPage.email_id + "'";
                }
            }
            else
            {
                CmdString = "select * from Passwords where UserName = '" + searchText + "' AND Email_ID ='" + LoginPage.email_id + "'";
            }
            //p = Decrypt(PasswordText, resultPassword);

            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Passwords");
            sda.Fill(dt);
            searchDataGrid.ItemsSource = dt.DefaultView;

        }

        private void searchDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (searchDataGrid.SelectedItem != null)
            {
                var cellInfo = searchDataGrid.SelectedCells[5];
                var content = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                Console.WriteLine("Cell Info: {0} \n Content: {1}", cellInfo, content);
                SqlConnection cnn;
                cnn = new SqlConnection(LoginPage.connectionString);
                try
                {
                    cnn.Open();
                    string query = "SELECT PASSWORD FROM Registration_Table where Email_ID= ' " + LoginPage.email_id + " '";
                    SqlCommand myCommand = new SqlCommand(query, cnn);
                    object result = myCommand.ExecuteReader();
                    if (result != null)
                    {
                        resultPassword = result.ToString();
                        PasswordText = Decrypt(content, resultPassword);
                        MessageBox.Show("Decrypted Password: " + PasswordText);
                    }
                    cnn.Close();
                }
                catch (Exception ex)
                { }
            }
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
