using Meebey.SmartIrc4net;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public class CRCClient
    {
        private const char META_DELIM = '☺'; // Separates metadata
        private const char FAKE_DELIM = '☻'; // Separates fake nick for death messages
        private Regex metaRx = new Regex("^(.*?)" + META_DELIM + "(.*)$");
        private Regex deathRx = new Regex("^(.*?)" + FAKE_DELIM + "(.*)$");

        private ClientDisplay display;
        private CRCGame game;
        private DebugDisplay debug;

        private IrcClient client;
        private Thread listener;
        private DateTime lastDeath = new DateTime();

        public CRCClient(ClientDisplay clientDisplay)
        {
            display = clientDisplay;
            game = new CRCGame(display, this);
#if DEBUG
            debug = new DebugDisplay();
            debug.Show();
#endif

            client = new IrcClient();
            client.Encoding = Encoding.UTF8;
            client.SendDelay = 200;
            client.ActiveChannelSyncing = true;

            client.OnChannelActiveSynced += new IrcEventHandler(OnChannelActiveSynced);
            client.OnRawMessage += new IrcEventHandler(OnRawMessage);
            client.OnChannelMessage += new IrcEventHandler(OnChannelMessage);
            //client.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
            client.OnJoin += new JoinEventHandler(OnJoin);
            client.OnPart += new PartEventHandler(OnPart);
            client.OnQuit += new QuitEventHandler(OnQuit);
            client.OnNickChange += new NickChangeEventHandler(OnNickChange);
            client.OnErrorMessage += new IrcEventHandler(OnErrorMessage);
            client.OnKick += new KickEventHandler(OnKick);

            client.Connect(CRCOptions.Server, 6667);
            client.Login(CRCOptions.GetIrcName(), "Chernobyl Relay Chat " + Application.ProductVersion);
            client.RfcJoin(CRCOptions.Channel);
            listener = new Thread(client.Listen);
            listener.IsBackground = true;
            listener.Start();
        }

        public void Close()
        {
            if (client.IsConnected)
            {
                client.RfcQuit();
                client.Disconnect();
            }
            if (debug != null)
                debug.Close();
        }

        public void GameCheck()
        {
            game.GameCheck();
        }

        public void GameUpdate()
        {
            game.GameUpdate();
        }

        public void UpdateNick()
        {
            client.RfcNick(CRCOptions.GetIrcName());
        }

        public void Send(string message)
        {
            client.SendMessage(SendType.Message, CRCOptions.Channel, CRCOptions.GetFaction() + META_DELIM + message);
            display.OnChannelMessage(CRCOptions.Name, message);
            game.OnChannelMessage(CRCOptions.Name, CRCOptions.GetFaction(), message);
        }

        public void SendDeath(string message)
        {
            string nick = CRCStrings.RandomName(CRCOptions.GameFaction);
            client.SendMessage(SendType.Message, CRCOptions.Channel, nick + FAKE_DELIM + CRCOptions.GetFaction() + META_DELIM + message);
            display.OnChannelMessage(nick, message);
            game.OnChannelMessage(nick, CRCOptions.GameFaction, message);
        }

        private string GetMetadata(string message, out string fakeNick, out string faction)
        {
            Match metaMatch = metaRx.Match(message);
            if (metaMatch.Success)
            {
                Match deathMatch = deathRx.Match(metaMatch.Groups[1].Value);
                if (deathMatch.Success)
                {
                    fakeNick = deathMatch.Groups[1].Value;
                    faction = CRCStrings.ValidateFaction(deathMatch.Groups[2].Value);
                    return metaMatch.Groups[2].Value;
                }
                else
                {
                    fakeNick = null;
                    faction = CRCStrings.ValidateFaction(metaMatch.Groups[1].Value);
                    return metaMatch.Groups[2].Value;
                }
            }
            else
            {
                fakeNick = null;
                faction = "actor_stalker";
                return message;
            }
        }



        private void OnChannelActiveSynced(object sender, IrcEventArgs e)
        {
            List<string> users = new List<string>();
            foreach (ChannelUser user in client.GetChannel(CRCOptions.Channel).Users.Values)
                users.Add(user.Nick.Replace('_', ' '));
            display.OnChannelActiveSynced(users);
        }

        private void OnRawMessage(object sender, IrcEventArgs e)
        {
            if (debug != null)
                debug.AddRaw(e.Data.RawMessage);
        }

        private void OnChannelMessage(object sender, IrcEventArgs e)
        {
            string fakeNick, faction;
            string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
            // If some cheeky m8 just sends delimiters, ignore it
            if (message.Length > 0)
            {
                string nick;
                if (fakeNick == null)
                    nick = e.Data.Nick.Replace('_', ' ');
                else if (CRCOptions.ReceiveDeath && (DateTime.Now - lastDeath).TotalSeconds > CRCOptions.DeathInterval)
                {
                    lastDeath = DateTime.Now;
                    nick = fakeNick;
                }
                else
                    return;
                display.OnChannelMessage(nick, message);
                game.OnChannelMessage(nick, faction, message);
            }
        }

        // Queries not supported currently
        private void OnQueryMessage(object sender, IrcEventArgs e)
        {
            string fakeNick, faction;
            string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
            // Never use fakeNick for query
            string nick = e.Data.Nick.Replace('_', ' ');
            display.OnQueryMessage(nick, CRCOptions.Name, message);
            game.OnQueryMessage(nick, CRCOptions.Name, faction, message);
        }

        private void OnJoin(object sender, JoinEventArgs e)
        {
            string nick = e.Who.Replace('_', ' ');
            if (nick != CRCOptions.Name)
            {
                display.OnJoin(nick);
                game.OnJoin(nick);
            }
            else
            {
                display.OnConnected();
                game.OnConnected();
            }
        }

        private void OnPart(object sender, PartEventArgs e)
        {
            string nick = e.Who.Replace('_', ' ');
            display.OnPart(nick);
            game.OnPart(nick);
        }

        private void OnQuit(object sender, QuitEventArgs e)
        {
            string nick = e.Who.Replace('_', ' ');
            display.OnPart(nick);
            game.OnPart(nick);
        }

        private void OnKick(object sender, KickEventArgs e)
        {
            string victim = e.Whom.Replace('_', ' ');
            if (victim == CRCOptions.Name)
            {
                display.OnGotKicked(e.KickReason);
                game.OnGotKicked(e.KickReason);
            }
            else
            {
                display.OnKick(victim, e.KickReason);
                game.OnKick(victim, e.KickReason);
            }
        }

        private void OnNickChange(object sender, NickChangeEventArgs e)
        {
            string oldNick = e.OldNickname.Replace('_', ' ');
            string newNick = e.NewNickname.Replace('_', ' ');
            if (newNick != CRCOptions.Name)
            {
                display.OnNickChange(oldNick, newNick);
                game.OnNickChange(oldNick, newNick);
            }
            else
            {
                display.OnOwnNickChange(oldNick, newNick);
                game.OnOwnNickChange(newNick);
            }
        }

        private void OnErrorMessage(object sender, IrcEventArgs e)
        {
            if (e.Data.ReplyCode == ReplyCode.ErrorBannedFromChannel)
                display.OnBanned();
        }
    }
}
