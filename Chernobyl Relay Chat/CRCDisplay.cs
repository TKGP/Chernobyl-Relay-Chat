using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    class CRCDisplay
    {
        private static ClientDisplay clientDisplay;

        public static void Start()
        {
            clientDisplay = new ClientDisplay();
            Application.Run(clientDisplay);
        }

        public static void Stop()
        {
            clientDisplay?.Invoke(new Action(() =>
                clientDisplay.Close())
                );
        }

        public static void OnConnected()
        {
            clientDisplay?.Enable();
            clientDisplay?.AddInformation("You are now connected to the network");
        }

        public static void UpdateUsers()
        {
            clientDisplay?.UpdateUsers(CRCClient.Users);
        }

        public static void OnHighlightMessage(string nick, string message)
        {
            clientDisplay?.AddHighlightMessage(nick, message);
        }

        public static void OnChannelMessage(string nick, string message)
        {
            clientDisplay?.AddMessage(nick, message, Color.Black);
        }

        public static void OnOwnChannelMessage(string nick, string message)
        {
            clientDisplay?.AddMessage(nick, message, Color.Gray);
        }

        public static void OnQueryMessage(string from, string to, string message)
        {
            SystemSounds.Asterisk.Play();
            clientDisplay?.AddMessage(from + " -> " + to, message, Color.DeepPink);
        }

        public static void OnJoin(string nick)
        {
            clientDisplay?.AddInformation(nick + " has logged on");
        }

        public static void OnPart(string nick)
        {
            clientDisplay?.AddInformation(nick + " has logged off");
        }

        public static void OnKick(string victim, string reason)
        {
            clientDisplay?.AddInformation(victim + " has been kicked for: " + reason);
        }

        public static void OnGotKicked(string reason)
        {
            clientDisplay?.AddError("You have been kicked for: " + reason);
            clientDisplay?.Disable();
        }

        public static void OnNickChange(string oldNick, string newNick)
        {
            clientDisplay?.AddInformation(oldNick + " is now known as " + newNick);
        }

        public static void OnOwnNickChange(string newNick)
        {
            clientDisplay?.AddInformation("You are now known as " + newNick);
        }

        public static void OnBanned()
        {
            clientDisplay?.AddError("Woops, you're banned!");
        }

        public static void OnError(string message)
        {
            clientDisplay?.AddError("Error: " + message);
        }

        public static void OnReconnecting()
        {
            clientDisplay?.AddInformation("Reconnecting...");
        }
    }
}
