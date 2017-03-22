using System;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public partial class LanguagePrompt : Form
    {
        public string Result;

        public LanguagePrompt()
        {
            InitializeComponent();
        }

        private void buttonEnglish_Click(object sender, EventArgs e)
        {
            Result = "eng";
            Close();
        }

        private void buttonRussian_Click(object sender, EventArgs e)
        {
            Result = "rus";
            Close();
        }
    }
}
