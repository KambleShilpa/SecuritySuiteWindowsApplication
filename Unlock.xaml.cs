using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
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
    /// Interaction logic for Unlock.xaml
    /// </summary>
    public partial class Unlock : Page
    {
        public Unlock()
        {
            InitializeComponent();
            FillLockDataGrid();
        }
        private void FillLockDataGrid()
        {
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(LoginPage_Class.ConString))
            {
                CmdString = "SELECT * FROM LOCK_Table where Username ='"+LoginPage.username+"'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("LOCK_Table");
                sda.Fill(dt);
                unlockGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void UnlockFileFolderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (unlockGrid.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < unlockGrid.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView selectedFile = (System.Data.DataRowView)unlockGrid.SelectedItems[i];
                        string str = Convert.ToString(selectedFile.Row.ItemArray[4]);
                        string str1 = Convert.ToString(selectedFile.Row.ItemArray[0]);  //MainPath
                                                                                        //System.Windows.MessageBox.Show(str);

                        FileSecurity fs = File.GetAccessControl(str);
                        fs.RemoveAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl, AccessControlType.Deny));
                        File.SetAccessControl(str, fs);

                        DirectoryInfo d = new DirectoryInfo(str);
                        string selectedpath = d.Parent.FullName + d.Name;
                        d.MoveTo(str.Substring(0, str.LastIndexOf(".")));
                        System.Windows.Forms.MessageBox.Show("Unlocked!!");

                        using (SqlConnection con = new SqlConnection(LoginPage.ConString))
                        {

                            con.Open();
                            SqlCommand cmd = new SqlCommand("Delete from LOCK_Table where Path ='" + str1 + "' and FILE_TYPE = 'Folder'", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                    }
                }
                FillLockDataGrid();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error!");
            }
        }
    }
}
