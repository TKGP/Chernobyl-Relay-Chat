using System;
using System.Drawing;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public partial class ClientDisplay : Form
    {
        private Regex delimRx = new Regex("[☺☻]");
        private CRCClient client;

        public ClientDisplay()
        {
            InitializeComponent();
        }

        private void ClientDisplay_Load(object sender, EventArgs e)
        {
            AddLine("Connecting...", Color.DarkBlue);
            client = new CRCClient(this);
        }

        private void ClientDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
        }

        private void AddLine(string line, Color color)
        {
            Invoke(new Action(() =>
            {
                if (richTextBoxMessages.Lines.Length != 0)
                    richTextBoxMessages.AppendText("\r\n");
                richTextBoxMessages.SelectionColor = color;
                richTextBoxMessages.AppendText(line);
            }));
        }

        private void Send()
        {
            if (delimRx.Match(textBoxInput.Text).Success)
                MessageBox.Show("I don't think so, Tim.", "Nice Try", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            else
            {
                string trimmed = textBoxInput.Text.Trim();
                if (trimmed.Length > 0)
                {
                    client.Send(trimmed);
                }
            }
            textBoxInput.Text = "";
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && buttonSend.Enabled)
            {
                Send();
            }
        }

        private void timerGameCheck_Tick(object sender, EventArgs e)
        {
            client.GameCheck();
        }

        private void timerGameUpdate_Tick(object sender, EventArgs e)
        {
            client.GameUpdate();
        }



        public void OnConnected()
        {
            Invoke(new Action(() =>
            {
                buttonSend.Enabled = true;
                buttonOptions.Enabled = true;
            }));
            AddLine("You are now connected to the network", Color.DarkBlue);
        }

        public void OnChannelMessage(string nick, string message)
        {
            AddLine(nick + ": " + message, Color.Black);
        }

        public void OnQueryMessage(string from, string to, string message)
        {
            SystemSounds.Asterisk.Play();
            AddLine(from + " -> " + to + ": " + message, Color.DeepPink);
        }

        public void OnJoin(string nick)
        {
            AddLine(nick + " has logged on", Color.DarkBlue);
        }

        public void OnPart(string nick)
        {
            AddLine(nick + " has logged off", Color.DarkBlue);
        }

        public void OnNickChange(string oldNick, string newNick)
        {
            AddLine(oldNick + " is now known as " + newNick, Color.DarkBlue);
        }

        public void OnOwnNickChange(string newNick)
        {
            AddLine("You are now known as " + newNick, Color.DarkBlue);
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            new OptionsForm(client).Show();
        }
    }
}
