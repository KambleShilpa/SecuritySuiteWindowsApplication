using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SecuritySuite.Login;

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for HideDrive.xaml
    /// </summary>
    public partial class HideDrive : Page
    {
        public HideDrive()
        {
            InitializeComponent();
        }
        DateTime currentDay = DateTime.Now;
        public static void restart()
        {
            Process p = new Process();
            foreach (System.Diagnostics.Process exe in System.Diagnostics.Process.GetProcesses())
            {
                if (exe.ProcessName == "explorer")

                    exe.Kill();
            }
        }

        private void HideDriveButton_Click(object sender, RoutedEventArgs e)
        {
            //button hide drive
            try
            {
                if (comboBox1.SelectionBoxItem.ToString() == "C") //(comboBox1.SelectionBoxItem == "C")//(comboBox1.SelectedIndex == 0)           //C Drive
                {
                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 4. untuk menyembunyikan drive C
                    rKey1.SetValue("NoDrives", Convert.ToInt32("4", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "D") //(comboBox1.SelectedIndex == 1)           //D Drive
                {

                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 8. untuk menyembunyikan drive D
                    rKey1.SetValue("NoDrives", Convert.ToInt32("8", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "E") //(comboBox1.SelectedIndex == 2)           //E Drive
                {

                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 10. untuk menyembunyikan drive E
                    rKey1.SetValue("NoDrives", Convert.ToInt32("10", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "F") //(comboBox1.SelectedIndex == 3)           //F Drive
                {

                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 20. untuk menyembunyikan drive F
                    rKey1.SetValue("NoDrives", Convert.ToInt32("20", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "G") //(comboBox1.SelectedIndex == 4)           //G Drive
                {
                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 40. untuk menyembunyikan drive G
                    rKey1.SetValue("NoDrives", Convert.ToInt32("40", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "H") //(comboBox1.SelectedIndex == 5)           //H Drive
                {
                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 80. untuk menyembunyikan drive H
                    rKey1.SetValue("NoDrives", Convert.ToInt32("80", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "I") //(comboBox1.SelectedIndex == 6)           //I Drive
                {
                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 100. untuk menyembunyikan drive I
                    rKey1.SetValue("NoDrives", Convert.ToInt32("100", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "J") //(comboBox1.SelectedIndex == 7)           //J Drive
                {
                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 200. untuk menyembunyikan drive J
                    rKey1.SetValue("NoDrives", Convert.ToInt32("200", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "K") //(comboBox1.SelectedIndex == 8)           //K Drive
                {
                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 200. untuk menyembunyikan drive J
                    rKey1.SetValue("NoDrives", Convert.ToInt32("200", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }

                if (comboBox1.SelectionBoxItem.ToString() == "All Drive") //(comboBox1.SelectedIndex == 9)           //All Drive
                {
                    //subkey nodrive in HKEY_CURRENT_USER
                    string nodrive = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                    RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrive);

                    //membuat DWORD 'NoDrives' dengan value 3ffffff. untuk menyembunyikan semua drive
                    rKey1.SetValue("NoDrives", Convert.ToInt32("3ffffff", 16), RegistryValueKind.DWord);
                    rKey1.Close();

                    //tampilkan kotak pesan
                    System.Windows.Forms.MessageBox.Show("Drive " + comboBox1.Text + ":\\ successfull concealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //membutuhkan restart explorer untuk perubahan
                    restart();

                }
                using (SqlConnection con = new SqlConnection(LoginPage.ConString))
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into HIDE_Table values('" + comboBox1.SelectionBoxItem + "', '" + LoginPage.username + "', " + "GETDATE()" + ",'Drive'" + ")", con);
                    cmd.ExecuteNonQuery();
                    //System.Windows.MessageBox.Show("Data Inserted Successfully.");
                }

            }
            catch
            {
                //Tampilkan kotak pesan
                System.Windows.Forms.MessageBox.Show("Something Error! Hide Drive Failed!", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
