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
    /// Interaction logic for Decryption.xaml
    /// </summary>
    public partial class Decryption : Page
    {
        string roundtrip, methodSelected, DecryptedDt;
        string myDekeys, filePath, iv;
        string newFilePath;
        byte[] encryptedDataFromFile, myDecryptKey, myIV;
        string dir = @"C:\securitysuite";

        //string connetionString = null;
        SqlConnection cnn;
        public Decryption()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".txt";
            openFileDlg.Filter = "Text documents (.txt)|*.txt";
            openFileDlg.InitialDirectory = @"C:\securitysuite\encryption\";
            openFileDlg.Multiselect = false;
            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                filePath = openFileDlg.FileName;
                FileToDecryptTB.Text = openFileDlg.FileName;
                //SecretKey.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
        }

        private void methodcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            methodSelected = methodComboBox.SelectedItem.ToString();
            Console.WriteLine("Encryption Method Selected: " + methodSelected);
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (DecryptionKeyTextbox.Text == "" || DecryptionKeyTextbox.Text == null)
            {
                MessageBox.Show("ENTER KEY");
            }
            if (FileToDecryptTB.Text == "" || FileToDecryptTB.Text == null)
            {
                MessageBox.Show("SELECT A FILE TO ENCRYPT");
            }
            if (methodComboBox.SelectedItem == null)
            {
                MessageBox.Show("SELECT A ENCRYPTION METHOD");
            }
            if (methodSelected.EndsWith("AES"))
            {
                if (DecryptionKeyTextbox.Text.Length < 32 || DecryptionKeyTextbox.Text.Length > 32)
                {
                    MessageBox.Show("Encryption Key should be of 32bits while encrypting with AES, Enter a 32bit encryption key.\nIf you want to encrypt with different length size of a key, please use 3DES encryption.");
                }
                initializeDecryption();
            }
            else if (methodSelected.EndsWith("3DES"))
            {
                initializeDecryptionUsing3Des();
            }
        }

        public void initializeDecryption()
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(filePath);
            //newFilePathc =  @"C:\securitysuite\encryption\" + filename(given by user + "encryption.text";
            newFilePath = @"C:\securitysuite\decryption\" + filename + "decryption.txt";

            encryptedDataFromFile = File.ReadAllBytes(filePath);
            iv = "1234098760567894";
            myIV = Encoding.UTF8.GetBytes(iv);
            myDekeys = DecryptionKeyTextbox.Text;
            myDecryptKey = Encoding.UTF8.GetBytes(myDekeys);
            roundtrip = DecryptStringFromBytes_Aes(encryptedDataFromFile, myDecryptKey, myIV);
            DecryptedDataTB.Text = roundtrip;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            try
            {
                // Check arguments.
                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");

                // Declare the string used to hold
                // the decrypted text.
                string plaintext = null;

                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                return plaintext;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Entered wrong secret key! Try again.", "Error");
                String error = "Entered key invalid.";
                return error;
            }
        }

        //3des decryption 
        public void initializeDecryptionUsing3Des()
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(filePath);
            newFilePath = @"C:\securitysuite\decryption\" + filename + "_3des_decryption.txt";
            string encryptedDataFromFile = File.ReadAllText(filePath);
            string key = DecryptionKeyTextbox.Text;
            var decryptedText = Decrypt(encryptedDataFromFile, key);
            DecryptedDataTB.Text = decryptedText;
            DecryptedDt = decryptedText;
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

        private void savefileButton_Click(object sender, RoutedEventArgs e)
        {
            fileManagement();
            if (methodSelected.EndsWith("AES"))
            {
                File.WriteAllText(newFilePath, roundtrip);
                choiceToSave("AES");
            }
            else if (methodSelected.EndsWith("3DES"))
            {
                File.WriteAllText(newFilePath, DecryptedDt);
                choiceToSave("3DES");
            }
        }

        public void choiceToSave(String methodname)
        {
            string filenamefromPath = System.IO.Path.GetFileName(filePath);
            Console.WriteLine("File Name: " + filenamefromPath);
            string newFilePaths = newFilePath;
            string newkeys = DecryptionKeyTextbox.Text;
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
                    string query = "INSERT INTO aesDecryption (FileName, DecryptionKey, NewDecryptedFilePath, MethodName, Email_ID)";
                    query += " VALUES (@FileName, @DecryptionKey, @NewDecryptedFilePath, @MethodName, @Email_ID)";

                    SqlCommand myCommand = new SqlCommand(query, cnn);
                    myCommand.Parameters.AddWithValue("@FileName", filenamefromPath);
                    myCommand.Parameters.AddWithValue("@DecryptionKey", newkeys);
                    myCommand.Parameters.AddWithValue("@NewDecryptedFilePath", newFilePaths);
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
                    string query = "INSERT INTO aesDecryption ( FileName, DecryptionKey, NewDecryptedFilePath, MethodName, Email_ID)";
                    query += " VALUES ( @FileName, @DecryptionKey, @NewDecryptedFilePath, @MethodName, @Email_ID)";

                    SqlCommand myCommand = new SqlCommand(query, cnn);
                    myCommand.Parameters.AddWithValue("@FileName", filenamefromPath);
                    myCommand.Parameters.AddWithValue("@DecryptionKey", "secret");
                    myCommand.Parameters.AddWithValue("@NewDecryptedFilePath", newFilePaths);
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

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            DecryptionKeyTextbox.Text = "";
            FileToDecryptTB.Text = "";
        }


        private void dataResetButton_Click(object sender, RoutedEventArgs e)
        {
            DecryptedDataTB.Text = "";
        }
    }
}
