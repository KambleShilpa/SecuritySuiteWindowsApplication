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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using static SecuritySuite.Login;

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for Lock.xaml
    /// </summary>
    public partial class Lock : Page
    {
        string[] arr;
        public string status;
        public Lock()
        {
            InitializeComponent();
            FillLockDataGrid();
            arr = new string[6];
            arr[0] = ".{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}";
            arr[1] = ".{21EC2020-3AEA-1069-A2DD-08002B30309D}";
            arr[2] = ".{2559a1f4-21d7-11d4-bdaf-00c04f60b9f0}";
            arr[3] = ".{645FF040-5081-101B-9F08-00AA002F954E}";
            arr[4] = ".{2559a1f1-21d7-11d4-bdaf-00c04f60b9f0}";
            arr[5] = ".{7007ACC7-3202-11D1-AAD2-00805FC1270E}";
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
                lockGrid.ItemsSource = dt.DefaultView;
            }
        }
        private void LockFolderButton_Click(object sender, RoutedEventArgs e)
        {
            status = arr[0];
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string Text1 = dlg.SelectedPath;
                    //System.Windows.MessageBox.Show(Text1);
                    FolderTolockTB.Text = Text1;
                    try
                    {
                        DirectoryInfo d = new DirectoryInfo(dlg.SelectedPath);
                        string selectedpath = d.Parent.FullName + d.Name;
                        if (dlg.SelectedPath.LastIndexOf(".{") == -1)
                        {
                            //LockFolderFunc(dlg.SelectedPath);
                            if (!d.Root.Equals(d.Parent.FullName))
                            {
                                d.MoveTo(d.Parent.FullName + "\\" + d.Name + status);
                            }
                            else
                            {
                                d.MoveTo(d.Parent.FullName + d.Name + status);
                            }
                            System.Windows.Forms.MessageBox.Show("locked!!");
                        }

                        FileSecurity fs = File.GetAccessControl(Text1 + status);
                        fs.AddAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl, AccessControlType.Deny));
                        File.SetAccessControl(Text1 + status, fs);


                        using (SqlConnection con = new SqlConnection(LoginPage.ConString))
                        {

                            con.Open();
                            SqlCommand cmd = new SqlCommand("insert into LOCK_Table values('" + Text1 + "', '" + LoginPage.username + "', " + "GETDATE()" + ",'Folder','" + Text1 + status + "'" + ")", con);
                            cmd.ExecuteNonQuery();
                            //System.Windows.MessageBox.Show("Data Inserted Successfully.");
                            con.Close();
                        }
                        FillLockDataGrid();

                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.ToString());//"Data Inserted Successfully.");
                    }
                }
            }
        }
        public void LockFolderFunc(string path)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement xmlelem;
            XmlNode xmlnode;
            XmlText xmltext;
            xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmldoc.AppendChild(xmlnode);
            xmlelem = xmldoc.CreateElement("", "ROOT", "");
            xmltext = xmldoc.CreateTextNode("abcdefghi");
            xmlelem.AppendChild(xmltext);
            xmldoc.AppendChild(xmlelem);
            xmldoc.Save(path + "\\p.xml");
            //this.Close();
        }
    }
}
