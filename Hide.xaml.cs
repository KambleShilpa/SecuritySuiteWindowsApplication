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
    /// Interaction logic for Hide.xaml
    /// </summary>
    public partial class Hide : Page
    {
        public Hide()
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
                hideGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void HideFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                try
                {
                    string CMDcommand = "/C attrib +s +h \"" + filename + "\"";
                    Process.Start("cmd.exe", CMDcommand);
                    using (SqlConnection con = new SqlConnection(LoginPage.ConString))
                    {

                        con.Open();
                        SqlCommand cmd = new SqlCommand("insert into HIDE_Table values('" + filename + "', '" + LoginPage.username + "', " + "GETDATE()" + ",'File'" + ")", con);
                        cmd.ExecuteNonQuery();
                        //System.Windows.MessageBox.Show("Data Inserted Successfully.");
                        con.Close();
                    }
                    FillHideDataGrid();
                }
                catch
                {
                    System.Windows.MessageBox.Show("Error");
                }
            }
        }

        private void HideFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string Text1 = dlg.SelectedPath;
                    //System.Windows.MessageBox.Show(Text1);
                    if (Text1.Substring(Text1.LastIndexOf(".")).ToString() == ".{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}")        //Shivani
                    {
                        System.Windows.MessageBox.Show("Can not hide this file");
                    }
                    else
                    {
                        try
                        {
                            string command = "/C attrib +s +h \"" + Text1 + "\"";
                            Process.Start("cmd.exe", command);

                            using (SqlConnection con = new SqlConnection(LoginPage.ConString))
                            {

                                con.Open();
                                SqlCommand cmd = new SqlCommand("insert into HIDE_Table values('" + Text1 + "', '" + LoginPage.username + "', " + "GETDATE()" + ",'Folder'" + ")", con);
                                cmd.ExecuteNonQuery();
                                //System.Windows.MessageBox.Show("Data Inserted Successfully.");
                                con.Close();
                            }
                            FillHideDataGrid();
                        }
                        catch (Exception Ex)
                        {
                            System.Windows.MessageBox.Show("Error");
                        }
                    }
                }
            }
        }
        private void HideDriveButton_Click(object sender, RoutedEventArgs e)
        {
            HideDriveFrame.Navigate(new System.Uri("HideDrive.xaml",
             UriKind.RelativeOrAbsolute));
        }
    }
}
