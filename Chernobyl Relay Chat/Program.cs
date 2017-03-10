using System;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CRCStrings.Populate();
            CRCOptions.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientDisplay());

            CRCOptions.Save();
        }
    }
}
