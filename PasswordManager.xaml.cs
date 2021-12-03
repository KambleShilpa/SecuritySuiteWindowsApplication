using SecuritySuite.Entities;
using SecuritySuite.PasswordServices;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace SecuritySuite
{
    /// <summary>
    /// Interaction logic for PasswordManager.xaml
    /// </summary>
    public partial class PasswordManager : Page
    {
        public PasswordManager()
        {
            InitializeComponent();
        }

        private void SearchPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new System.Uri("SearchPassword.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void ShowPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new System.Uri("ShowAllPassword.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void AddNew_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new System.Uri("AddNewPasswords.xaml",
             UriKind.RelativeOrAbsolute));
        }

        private void Export_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Export Passwords";
            sfd.DefaultExt = "bpf";
            sfd.Filter = " files (*.bpf)|*.bpf|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.CheckPathExists = true;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Password.ID = ShowAllPassword.ID;
                    Password.UserName = ShowAllPassword.UserName;
                    Password.UserID = ShowAllPassword.UserID;
                    Password.Email = ShowAllPassword.Email;
                    Password.Website = ShowAllPassword.Website;
                    Password.PasswordText = ShowAllPassword.PasswordText;
                    Password.Notes = ShowAllPassword.Notes;
                    FileStream WriteFileStream = new FileStream(sfd.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                    TextWriter writer = new StreamWriter(WriteFileStream);
                    writer.WriteLine("ID: "+ Password.ID);
                    writer.Write("\n UserName: " + Password.UserName);
                    writer.Write("\n UserID: " + Password.UserID);
                    writer.Write("\n Email: " + Password.Email);
                    writer.Write("\n Website: " + Password.Website);
                    writer.Write("\n Password: " + Password.PasswordText);
                    writer.Write("\n Notes: " + Password.Notes);
                    writer.Write("\n");
                    writer.Flush();
                    writer.Close();
                    Messenger.Show("Passwords exported to " + sfd.FileName + " file.", "Info");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        private void Import_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Import Passwords";
            ofd.DefaultExt = "bpf";
            ofd.Filter = " files (*.bpf)|*.bpf|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] readarr = new byte[100];
                int count;
                FileStream rds = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                while ((count = rds.Read(readarr, 0, readarr.Length)) > 0)
                {
                    Console.WriteLine(Encoding.UTF8.GetString(readarr, 0, count));
                    Messenger.Show("Passwords imported to " + Encoding.UTF8.GetString(readarr, 0, count), "Info");
                }
                //string read = rds.Read
                rds.Flush();
                rds.Close();
            }
        }
    }
}