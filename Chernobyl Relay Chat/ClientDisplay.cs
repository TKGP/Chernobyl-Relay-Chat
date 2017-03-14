using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public partial class ClientDisplay : Form, ICRCSendable
    {
        private CRCClient client;
        private List<string> users = new List<string>();
        private Font mainFont, boldFont, timeFont;

        public ClientDisplay()
        {
            InitializeComponent();
        }

        private void ClientDisplay_Load(object sender, EventArgs e)
        {
            mainFont = richTextBoxMessages.Font;
            boldFont = new Font(mainFont, FontStyle.Bold);
            timeFont = new Font("Courier New", mainFont.SizeInPoints, FontStyle.Regular);
            Text = "Chernobyl Relay Chat " + Application.ProductVersion;
            if (CRCOptions.DisplaySize != new Size(-1, -1))
            {
                Location = CRCOptions.DisplayLocation;
                Size = CRCOptions.DisplaySize;
            }

            AddInformation("Connecting...");
            client = new CRCClient(this);
        }

        private void ClientDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                CRCOptions.DisplayLocation = Location;
                CRCOptions.DisplaySize = Size;
            }
            else
            {
                CRCOptions.DisplayLocation = RestoreBounds.Location;
                CRCOptions.DisplaySize = RestoreBounds.Size;
            }

            client.Close();
        }

        private void buttonOptions_Click(object sender, EventArgs e)
        {
            new OptionsForm(client).Show();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string trimmed = textBoxInput.Text.Trim();
            if (trimmed[0] == '/')
            {
                CRCCommands.ProcessCommand(trimmed, this);
            }
            else if (trimmed.Length > 0)
            {
                client.Send(trimmed);
            }
            textBoxInput.Clear();
        }

        private void timerGameCheck_Tick(object sender, EventArgs e)
        {
            client.GameCheck();
        }

        private void timerGameUpdate_Tick(object sender, EventArgs e)
        {
            client.GameUpdate();
        }

        private async void timerCheckUpdate_Tick(object sender, EventArgs e)
        {
            await CRCUpdate.CheckUpdate(this, client);
        }

        private void richTextBoxMessages_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            // Apparently safer than just passing the link
            Process.Start("explorer.exe", e.LinkText);
        }



        private void AddLinePrefix()
        {
            if (richTextBoxMessages.Lines.Length != 0)
                richTextBoxMessages.AppendText("\r\n");
            if (CRCOptions.ShowTimestamps)
            {
                richTextBoxMessages.SelectionColor = Color.Black;
                richTextBoxMessages.SelectionFont = timeFont;
                richTextBoxMessages.AppendText(DateTime.Now.ToString("hh:mm:ss "));
            }
        }

        private void AddLine(string line, Color color)
        {
            Invoke(new Action(() =>
            {
                AddLinePrefix();
                richTextBoxMessages.SelectionFont = mainFont;
                richTextBoxMessages.SelectionColor = color;
                richTextBoxMessages.AppendText(line);
            }));
        }

        public void AddInformation(string line)
        {
            AddLine(line, Color.DarkBlue);
        }

        public void AddError(string line)
        {
            AddLine(line, Color.Red);
        }

        private void AddMessage(string nick, string message, Color nickColor)
        {
            Invoke(new Action(() =>
            {
                AddLinePrefix();
                richTextBoxMessages.SelectionFont = boldFont;
                richTextBoxMessages.SelectionColor = nickColor;
                richTextBoxMessages.AppendText(nick + ": ");
                richTextBoxMessages.SelectionFont = mainFont;
                richTextBoxMessages.SelectionColor = Color.Black;
                richTextBoxMessages.AppendText(message);
            }));
        }

        private void UpdateUsers()
        {
            users.Sort();
            Invoke(new Action(() =>
            {
                textBoxUsers.Text = string.Join("\r\n", users);
            }));
        }



        public void OnConnected()
        {
            Invoke(new Action(() =>
            {
                buttonSend.Enabled = true;
                buttonOptions.Enabled = true;
            }));
            AddInformation("You are now connected to the network");
        }

        public void OnChannelActiveSynced(List<string> usersOnJoin)
        {
            users = usersOnJoin;
            UpdateUsers();
        }

        public void OnHighlightMessage(string nick, string message)
        {
            Invoke(new Action(() =>
            {
                AddMessage(nick, message, Color.Black);
                int start = richTextBoxMessages.GetFirstCharIndexOfCurrentLine();
                int length = richTextBoxMessages.TextLength - start;
                richTextBoxMessages.Select(start, length);
                richTextBoxMessages.SelectionBackColor = Color.Yellow;
                richTextBoxMessages.Select(0, 0);
                richTextBoxMessages.SelectionBackColor = Color.White;
            }));
        }

        public void OnChannelMessage(string nick, string message)
        {
            AddMessage(nick, message, Color.Black);
        }

        public void OnOwnChannelMessage(string nick, string message)
        {
            AddMessage(nick, message, Color.Gray);
        }

        public void OnQueryMessage(string from, string to, string message)
        {
            SystemSounds.Asterisk.Play();
            AddMessage(from + " -> " + to, message, Color.DeepPink);
        }

        public void OnJoin(string nick)
        {
            users.Add(nick);
            UpdateUsers();
            AddInformation(nick + " has logged on");
        }

        public void OnPart(string nick)
        {
            users.Remove(nick);
            UpdateUsers();
            AddInformation(nick + " has logged off");
        }

        public void OnKick(string victim, string reason)
        {
            users.Remove(victim);
            UpdateUsers();
            AddInformation(victim + " has been kicked for: " + reason);
        }

        public void OnGotKicked(string reason)
        {
            users.Clear();
            UpdateUsers();
            Invoke(new Action(() =>
            {
                buttonSend.Enabled = false;
                buttonOptions.Enabled = false;
            }));
            AddError("You have been kicked for: " + reason);
        }

        public void OnNickChange(string oldNick, string newNick)
        {
            users.Remove(oldNick);
            users.Add(newNick);
            UpdateUsers();
            AddInformation(oldNick + " is now known as " + newNick);
        }

        public void OnOwnNickChange(string oldNick, string newNick)
        {
            users.Remove(oldNick);
            users.Add(newNick);
            UpdateUsers();
            AddInformation("You are now known as " + newNick);
        }

        public void OnBanned()
        {
            AddError("Woops, you're banned!");
        }

        public void OnError(string message)
        {
            AddError("Error: " + message);
        }
    }
}
