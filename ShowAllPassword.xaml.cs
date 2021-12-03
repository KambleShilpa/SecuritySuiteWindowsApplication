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
    /// Interaction logic for ShowAllPassword.xaml
    /// </summary>
    public partial class ShowAllPassword : Page
    {
        public static string resultPassword, PasswordText;
        public static int ID;
        public static string UserName, UserID, Email, Website, Notes;
        public static DateTime DateCreated, DateModified;
        public ShowAllPassword()
        {
            InitializeComponent();
        }

        private void showAllPasswordDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (showAllPasswordDataGrid.SelectedItem != null)
            {
                var cellInfo = showAllPasswordDataGrid.SelectedCells[5];
                var content = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                Console.WriteLine("Cell Info: {0} \n Content: {1}", cellInfo, content);
                cellInfo = showAllPasswordDataGrid.SelectedCells[0];
                ID = Convert.ToInt32((cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text);
                cellInfo = showAllPasswordDataGrid.SelectedCells[1];
                UserName = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                cellInfo = showAllPasswordDataGrid.SelectedCells[2];
                UserID = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                cellInfo = showAllPasswordDataGrid.SelectedCells[3];
                Email = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                cellInfo = showAllPasswordDataGrid.SelectedCells[4];
                Website = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                cellInfo = showAllPasswordDataGrid.SelectedCells[6];
                Notes = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                SqlConnection cnn;
                cnn = new SqlConnection(LoginPage.connectionString);
                try
                {
                    cnn.Open();
                    string query = "SELECT PASSWORD FROM Registration_Table  where Email_ID ='" + LoginPage.email_id + "'";
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

        private void showAllPasswordDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            string CmdString = string.Empty;
            SqlConnection con = new SqlConnection(LoginPage.connectionString);
            con.Open();
            CmdString = "Select * from Passwords where Email_ID ='" + LoginPage.email_id + "'";
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Passwords");
            sda.Fill(dt);
            showAllPasswordDataGrid.ItemsSource = dt.DefaultView;
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
