using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for Encryption.xaml
    /// </summary>
    public partial class Encryption : Page
    {
        string filePath, key, methodSelected, encryptedDt;
        string originalText, iv;
        string newFilePath;
        byte[] mykey, myIV, encryptedData;
        string dir = @"C:\securitysuite";
        //string connetionString = null;
        SqlConnection cnn;
        public Encryption()
        {
            InitializeComponent();
        }
        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".txt";
            openFileDlg.Filter = "Text documents (.txt)|*.txt";
            openFileDlg.InitialDirectory = @"C:\";
            openFileDlg.Multiselect = false;
            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                filePath = openFileDlg.FileName;
                FileToEncryptTB.Text = openFileDlg.FileName;
                //EncryptionKeyTextbox.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
        }

        private void methodcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            methodSelected = methodComboBox.SelectedItem.ToString();
            Console.WriteLine("Encryption Method Selected: " + methodSelected);
        }

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (EncryptionKeyTextbox.Text == "" || EncryptionKeyTextbox.Text == null)
            {
                MessageBox.Show("ENTER KEY");
            }
            if (FileToEncryptTB.Text == "" || FileToEncryptTB.Text == null)
            {
                MessageBox.Show("SELECT A FILE TO ENCRYPT");
            }
            if (methodComboBox.SelectedItem == null)
            {
                MessageBox.Show("SELECT A ENCRYPTION METHOD");
            }
            if (methodSelected.EndsWith("AES"))
            {
                if (EncryptionKeyTextbox.Text.Length < 32 || EncryptionKeyTextbox.Text.Length > 32)
                {
                    MessageBox.Show("Encryption Key should be of 32bits while encrypting with AES, Enter a 32bit encryption key.\nIf you want to encrypt with different length size of a key, please use 3DES encryption.");
                    EncryptionKeyTextbox.Text = "";
                }
                else { 
                initializeEncryption(filePath);
                }
            }
            else if (methodSelected.EndsWith("3DES"))
            {
                initializeEncryptionUsind3des();
            }
        }

        public void initializeEncryption(string filePath)
        {
            string path = filePath;
            string filename = System.IO.Path.GetFileNameWithoutExtension(path);
            originalText = File.ReadAllText(path);
            //newFilePathc =  @"C:\securitysuite\encryption\" + filename(given by user + "encryption.text";
            newFilePath = @"C:\securitysuite\encryption\" + filename + "encryption.txt";
            iv = "1234098760567894";
            myIV = Encoding.UTF8.GetBytes(iv);
            //accept user key and call encryption method
            //also can ask here if they want us to save their keys in application
            key = EncryptionKeyTextbox.Text.ToString();
            mykey = Encoding.UTF8.GetBytes(key);

            byte[] encrypted = EncryptStringToBytes_Aes(originalText, mykey, myIV);
            int a = encrypted.Length;
            encryptedData = new byte[a];
            for (int i = 0; i < encrypted.Length; i++)
            {
                encryptedData[i] = encrypted[i];
            }
            Console.WriteLine("encryptedData variable data");
            for (int i = 0; i < encryptedData.Length; i++)
            {
                //EncryptedDataTB.Text += encryptedData[i];
                Console.Write(encryptedData[i]);

            }
            EncryptedDataTB.Text = String.Join("", Array.ConvertAll(encryptedData, byteValue => byteValue.ToString()));
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            EncryptionKeyTextbox.Text = "";
            FileToEncryptTB.Text = "";
        }

        private void dataResetButton_Click_1(object sender, RoutedEventArgs e)
        {
            EncryptedDataTB.Text = "";
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        //3des encryption
        public void initializeEncryptionUsind3des()
        {
            string path = filePath;
            string filename = System.IO.Path.GetFileNameWithoutExtension(path);
            originalText = File.ReadAllText(path);
            //newFilePathc =  @"C:\securitysuite\encryption\" + filename(given by user + "encryption.text";
            newFilePath = @"C:\securitysuite\encryption\" + filename + "3des_encryption.txt";
            //also can ask here if they want us to save their keys in application
            key = EncryptionKeyTextbox.Text;
            var encryptedText = Encrypt(originalText, key);
            EncryptedDataTB.Text = encryptedText;
            encryptedDt = encryptedText;
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
            //Console.WriteLine("Key is: " + mysecurityKey);

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

        //saving
        private void savefileButton_Click(object sender, RoutedEventArgs e)
        {
            fileManagement();
            if (methodSelected.EndsWith("AES"))
            {
                File.WriteAllBytes(newFilePath, encryptedData);
                choiceToSave("AES");
            }
            else if (methodSelected.EndsWith("3DES"))
            {
                File.WriteAllText(newFilePath, encryptedDt);
                choiceToSave("3DES");
            }
        }

        public void fileManagement()
        {
            // If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string enDIR = @"C:\securitysuite\encryption";
            // If directory does not exist, create it
            if (!Directory.Exists(enDIR))
            {
                Directory.CreateDirectory(enDIR);
            }
        }

        public void choiceToSave(String methodname)
        {
            string filenamefromPath = System.IO.Path.GetFileName(filePath);
            string newFilePaths = newFilePath;
            //Console.WriteLine("new saved File Name: {0} new full path: {1} ", newFilePath, newFilePaths);
            string newkeys = EncryptionKeyTextbox.Text;
            string methodName = methodname;
            string Email_ID = LoginPage.email_id;
            MessageBoxResult mbResult = MessageBox.Show("Do you want to save the encryption key?", "Save Key", MessageBoxButton.YesNo);
            if (mbResult == MessageBoxResult.Yes)
            {
                //connetionString = "Data Source=localhost;Initial Catalog=aesDatabase;persist security info=True; Integrated Security=SSPI";
                cnn = new SqlConnection(LoginPage.connectionString);
                try
                {
                    cnn.Open();
                    string query = "INSERT INTO aesUserEncryption (FileName, EncryptionKey, NewEncryptedFilePath, MethodName, Email_ID)";
                    query += " VALUES (@FileName, @EncryptionKey, @NewEncryptedFilePath, @MethodName, @Email_ID)";

                    SqlCommand myCommand = new SqlCommand(query, cnn);
                    myCommand.Parameters.AddWithValue("@FileName", filenamefromPath);
                    myCommand.Parameters.AddWithValue("@EncryptionKey", newkeys);
                    myCommand.Parameters.AddWithValue("@NewEncryptedFilePath", newFilePaths);
                    myCommand.Parameters.AddWithValue("@MethodName", methodName);
                    myCommand.Parameters.AddWithValue("@Email_ID", Email_ID);
                    myCommand.ExecuteNonQuery();
                    MessageBox.Show("Saved File With Key");
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can not open connection ! " + ex.Message);
                }
            }
            else if (mbResult == MessageBoxResult.No)
            {
                //connetionString = "Data Source=localhost;Initial Catalog=aesDatabase;persist security info=True; Integrated Security=SSPI";
                cnn = new SqlConnection(LoginPage.connectionString);
                try
                {
                    cnn.Open();
                    string query = "INSERT INTO aesUserEncryption ( FileName, EncryptionKey, NewEncryptedFilePath, MethodName, Email_ID)";
                    query += " VALUES ( @FileName, @EncryptionKey, @NewEncryptedFilePath, @MethodName, @Email_ID)";

                    SqlCommand myCommand = new SqlCommand(query, cnn);
                    myCommand.Parameters.AddWithValue("@FileName", filenamefromPath);
                    myCommand.Parameters.AddWithValue("@EncryptionKey", "secret");
                    myCommand.Parameters.AddWithValue("@NewEncryptedFilePath", newFilePaths);
                    myCommand.Parameters.AddWithValue("@MethodName", methodName);
                    myCommand.Parameters.AddWithValue("@Email_ID", Email_ID);

                    myCommand.ExecuteNonQuery();
                    MessageBox.Show("Saved File Without Key");
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can not open connection ! " + ex.Message);
                }
            }
            else
            {
            }
        }
    }
}
