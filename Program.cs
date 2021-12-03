using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecuritySuite
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            SplashScreen splash = new SplashScreen();
            splash.Show();
            Thread.Sleep(3000);
            splash.Close();
            App.Main();

        }
    }
}
