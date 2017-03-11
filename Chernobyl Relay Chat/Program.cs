using System;
using System.Threading;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    static class Program
    {
        // GUID from AssemblyInfo.cs
        private static readonly Mutex mutex = new Mutex(false, "64eb5dda-2131-47fc-a32c-fbc64f440d8a");

        [STAThread]
        static void Main()
        {
            // Prevent multiple instances
            if (!mutex.WaitOne(TimeSpan.FromSeconds(0), false))
                return;

            if (CRCUpdate.CheckFirstUpdate())
              return;

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
