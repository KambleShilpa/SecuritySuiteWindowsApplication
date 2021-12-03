using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
    /// Interaction logic for ListOfFilesEnDecrypted.xaml
    /// </summary>
    public partial class ListOfFilesEnDecrypted : Page
    {
        public ListOfFilesEnDecrypted()
        {
            InitializeComponent();
            refreshdataEncryption();
        }
        public void refreshdataEncryption()
        {
            string CmdString = string.Empty;
            //SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=aesDatabase;persist security info=True; Integrated Security=SSPI");
            SqlConnection con = new SqlConnection(LoginPage.connectionString);
            con.Open();
            CmdString = "SELECT * FROM aesUserEncryption where Email_ID ='"+LoginPage.email_id+"'";
            SqlCommand cmd = new SqlCommand(CmdString, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("aesUserEncryption");
            sda.Fill(dt);
            dataGrid1.ItemsSource = dt.DefaultView;

            CmdString = "SELECT * FROM aesDecryption where Email_ID ='"+LoginPage.email_id+"'";
            cmd = new SqlCommand(CmdString, con);
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable("aesUserEncryption");
            sda.Fill(dt);
            dataGrid2.ItemsSource = dt.DefaultView;
            con.Close();
        }
    }
}
