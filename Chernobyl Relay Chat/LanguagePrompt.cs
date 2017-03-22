using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
