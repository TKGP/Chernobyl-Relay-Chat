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
            if (CRCOptions.Load())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ClientDisplay());
                CRCOptions.Save();
            }
            else
            {
                MessageBox.Show("CRC was unable to access the registry, which is needed to preserve settings.\r\n"
                    + "Please try running the application As Administrator.",
                    "Chernobyl Relay Chat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
