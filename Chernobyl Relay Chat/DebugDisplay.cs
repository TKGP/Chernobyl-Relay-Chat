using System;
using System.Text;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public partial class DebugDisplay : Form
    {
        StringBuilder rawSb = new StringBuilder();

        public DebugDisplay()
        {
            InitializeComponent();
        }

        public void AddRaw(string message)
        {
            if (rawSb.Length > 0)
                rawSb.Append("\r\n");
            rawSb.Append(message);
            Invoke(new Action(() =>
            {
                textBoxRaw.Text = rawSb.ToString();
                textBoxRaw.SelectionStart = textBoxRaw.Text.Length;
                textBoxRaw.ScrollToCaret();
            }));
        }
    }
}
