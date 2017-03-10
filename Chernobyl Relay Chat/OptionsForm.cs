using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public partial class OptionsForm : Form
    {
        private CRCClient client;
        private Regex nickCheckRx = new Regex(@"^\w[\w\d ]*$");

        private readonly Dictionary<string, int> factionToIndex = new Dictionary<string, int>()
        {
            ["actor_bandit"] = 0,
            ["actor_csky"] = 1,
            ["actor_dolg"] = 2,
            ["actor_ecolog"] = 3,
            ["actor_freedom"] = 4,
            ["actor_stalker"] = 5,
            ["actor_killer"] = 6,
            ["actor_army"] = 7,
            ["actor_monolith"] = 8,
        };

        private readonly Dictionary<int, string> indexToFaction = new Dictionary<int, string>()
        {
            [0] = "actor_bandit",
            [1] = "actor_csky",
            [2] = "actor_dolg",
            [3] = "actor_ecolog",
            [4] = "actor_freedom",
            [5] = "actor_stalker",
            [6] = "actor_killer",
            [7] = "actor_army",
            [8] = "actor_monolith",
        };

        public OptionsForm(CRCClient crcClient)
        {
            InitializeComponent();
            client = crcClient;

            radioButtonFactionAuto.Checked = CRCOptions.AutoFaction;
            radioButtonFactionManual.Checked = !CRCOptions.AutoFaction;
            textBoxName.Text = CRCOptions.Name;
            int index = factionToIndex[CRCOptions.ManualFaction];
            comboBoxFaction.SelectedIndex = index;
            checkBoxDeathSend.Checked = CRCOptions.SendDeath;
            checkBoxDeathReceive.Checked = CRCOptions.ReceiveDeath;
            numericUpDownDeath.Value = CRCOptions.DeathInterval;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            if (name.Length < 1 || name.Length > 30)
            {
                MessageBox.Show("Nickname must be between 1 and 30 characters in length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!nickCheckRx.Match(name).Success)
            {
                MessageBox.Show("Nicknames must start with a letter and only use letters, numbers, and spaces.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CRCOptions.AutoFaction = radioButtonFactionAuto.Checked;
            CRCOptions.ManualFaction = indexToFaction[comboBoxFaction.SelectedIndex];
            CRCOptions.Name = name;
            CRCOptions.SendDeath = checkBoxDeathSend.Checked;
            CRCOptions.ReceiveDeath = checkBoxDeathReceive.Checked;
            CRCOptions.DeathInterval = (int)numericUpDownDeath.Value;
            CRCOptions.Save();
            client.UpdateNick();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            textBoxName.Text = CRCStrings.RandomName(comboBoxFaction.SelectedItem.ToString());
        }

        private void radioButtonFactionManual_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxFaction.Enabled = radioButtonFactionManual.Checked;
        }

        private void checkBoxDeathReceive_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownDeath.Enabled = checkBoxDeathReceive.Checked;
        }
    }
}
