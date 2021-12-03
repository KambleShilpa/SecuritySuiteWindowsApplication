using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Unhide.xaml
    /// </summary>
    public partial class Unhide : Page
    {
        public Unhide()
        {
            InitializeComponent();
            FillHideDataGrid();
        }
        private void FillHideDataGrid()
        {
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(LoginPage_Class.ConString))
            {
                CmdString = "SELECT * FROM HIDE_Table where Username ='"+LoginPage.username+"'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("HIDE_Table");
                sda.Fill(dt);
                unhideGrid.ItemsSource = dt.DefaultView;
            }

        }

        private void UnhideFileFolderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (unhideGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < unhideGrid.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView selectedFile = (System.Data.DataRowView)unhideGrid.SelectedItems[i];
                        string str = Convert.ToString(selectedFile.Row.ItemArray[0]);
                        //System.Windows.MessageBox.Show(str);
                        string CMDcommand = "/C attrib -s -h \"" + str + "\"";
                        Process.Start("cmd.exe", CMDcommand);

                        using (SqlConnection con = new SqlConnection(LoginPage.ConString))
                        {

                            con.Open();
                            SqlCommand cmd = new SqlCommand("Delete from HIDE_Table where Path ='" + str + "' and FILE_TYPE <> 'Drive'", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                    }
                }
                FillHideDataGrid();

            }
            catch
            {
                System.Windows.MessageBox.Show("Error");
            }

        }

        private void UnhideDriveButton_Click(object sender, RoutedEventArgs e)
        {
            //button unhide drive
            try
            {
                if (unhideGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < unhideGrid.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView selectedFile = (System.Data.DataRowView)unhideGrid.SelectedItems[i];
                        string str = Convert.ToString(selectedFile.Row.ItemArray[0]);
                        //System.Windows.MessageBox.Show(str);

                        //subkey nodrive in HKEY_CURRENT_USER
                        string nodrives = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                        RegistryKey rKey1 = Registry.CurrentUser.CreateSubKey(nodrives);

                        //menghapus 'NoDrives' DWORD
                        rKey1.DeleteValue("NoDrives");
                        rKey1.Close();
                        HideDrive.restart();

                        //tampilkan kotak pesan
                        //System.Windows.Forms.MessageBox.Show("Drive " + HideGrid.SelectedItem + ":\\ successfully unconcealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Windows.Forms.MessageBox.Show("Drive successfully unconcealed ", "Security Suite", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //membutuhkan restart explorer untuk perubahan
                        HideDrive.restart();
                        using (SqlConnection con = new SqlConnection(LoginPage.ConString))
                        {

                            con.Open();
                            SqlCommand cmd = new SqlCommand("Delete from HIDE_Table where Path ='" + str + "' and FILE_TYPE = 'Drive'", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                FillHideDataGrid();
            }
            catch
            {
            }

            try
            {
                //memulai kembali explorer jika explorer tidak muncul kembali
                Process.Start("C:\\Windows\\explorer.exe");
            }
            catch
            {
            }
        }
    }
}
