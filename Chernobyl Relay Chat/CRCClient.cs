using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public class CRCClient
    {
        private const char META_DELIM = '☺'; // Separates metadata
        private const char FAKE_DELIM = '☻'; // Separates fake nick for death messages
        private static readonly Regex metaRx = new Regex("^(.*?)" + META_DELIM + "(.*)$");
        private static readonly Regex deathRx = new Regex("^(.*?)" + FAKE_DELIM + "(.*)$");
        private static readonly Regex commandArgsRx = new Regex(@"\S+");
        private string lastQuery;

        private ClientDisplay display;
        private CRCGame game;
#if DEBUG
        private DebugDisplay debug;
#endif

        private IrcClient client;
        private Thread listener;
        private DateTime lastDeath = new DateTime();

        public CRCClient(ClientDisplay clientDisplay)
        {
            CRCCommands.client = this;
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

            client.OnConnected += new EventHandler(OnConnection);
            client.OnChannelActiveSynced += new IrcEventHandler(OnChannelActiveSynced);
            client.OnRawMessage += new IrcEventHandler(OnRawMessage);
            client.OnChannelMessage += new IrcEventHandler(OnChannelMessage);
            client.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
            client.OnJoin += new JoinEventHandler(OnJoin);
            client.OnPart += new PartEventHandler(OnPart);
            client.OnQuit += new QuitEventHandler(OnQuit);
            client.OnNickChange += new NickChangeEventHandler(OnNickChange);
            client.OnErrorMessage += new IrcEventHandler(OnErrorMessage);
            client.OnKick += new KickEventHandler(OnKick);

            client.Connect(CRCOptions.Server, 6667);
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
#if DEBUG
            debug.Close();
#endif
        }

        public void GameCheck()
        {
            game.GameCheck();
        }

        public void GameUpdate()
        {
            game.GameUpdate();
        }

        public void UpdateSettings()
        {
            client.RfcNick(CRCOptions.Name);
            game.UpdateSettings();
        }

        public void ChangeNick(string nick)
        {
            CRCOptions.Name = nick;
            client.RfcNick(nick);
        }

        public void Send(string message)
        {
            client.SendMessage(SendType.Message, CRCOptions.Channel, CRCOptions.GetFaction() + META_DELIM + message);
            display.OnOwnChannelMessage(CRCOptions.Name, message);
            game.OnChannelMessage(CRCOptions.Name, CRCOptions.GetFaction(), message);
        }

        public void SendDeath(string message)
        {
            string nick = CRCStrings.RandomName(CRCOptions.GameFaction);
            client.SendMessage(SendType.Message, CRCOptions.Channel, nick + FAKE_DELIM + CRCOptions.GetFaction() + META_DELIM + message);
            display.OnChannelMessage(nick, message);
            game.OnChannelMessage(nick, CRCOptions.GameFaction, message);
        }

        public void SendQuery(string nick, string message)
        {
            client.SendMessage(SendType.Message, nick, CRCOptions.GetFaction() + META_DELIM + message);
            display.OnQueryMessage(CRCOptions.Name, nick, message);
            game.OnQueryMessage(CRCOptions.Name, nick, CRCOptions.GetFaction(), message);
        }

        public bool SendReply(string message)
        {
            if (lastQuery != null)
            {
                SendQuery(lastQuery, message);
                return true;
            }
            return false;
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

        public void OnUpdate(string message)
        {
            game.OnUpdate(message);
        }



        private void OnConnection(object sender, EventArgs e)
        {
            client.Login(CRCOptions.Name, "Chernobyl Relay Chat " + Application.ProductVersion);
            client.RfcJoin(CRCOptions.Channel);
        }

        private void OnChannelActiveSynced(object sender, IrcEventArgs e)
        {
            List<string> users = new List<string>();
            foreach (ChannelUser user in client.GetChannel(CRCOptions.Channel).Users.Values)
                users.Add(user.Nick);
            display.OnChannelActiveSynced(users);
        }

        private void OnRawMessage(object sender, IrcEventArgs e)
        {
#if DEBUG
            debug.AddRaw(e.Data.RawMessage);
#endif
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
                    nick = e.Data.Nick;
                else if (CRCOptions.ReceiveDeath && (DateTime.Now - lastDeath).TotalSeconds > CRCOptions.DeathInterval)
                {
                    lastDeath = DateTime.Now;
                    nick = fakeNick;
                }
                else
                    return;
                if (message.Contains(CRCOptions.Name))
                {
                    SystemSounds.Asterisk.Play();
                    display.OnHighlightMessage(nick, message);
                    game.OnHighlightMessage(nick, faction, message);
                }
                else
                {
                    display.OnChannelMessage(nick, message);
                    game.OnChannelMessage(nick, faction, message);
                }
            }
        }

        private void OnQueryMessage(object sender, IrcEventArgs e)
        {
            lastQuery = e.Data.Nick;
            string fakeNick, faction;
            string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
            // Never use fakeNick for query
            string nick = e.Data.Nick;
            display.OnQueryMessage(nick, CRCOptions.Name, message);
            game.OnQueryMessage(nick, CRCOptions.Name, faction, message);
        }

        private void OnJoin(object sender, JoinEventArgs e)
        {
            string nick = e.Who;
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
            string nick = e.Who;
            display.OnPart(nick);
            game.OnPart(nick);
        }

        private void OnQuit(object sender, QuitEventArgs e)
        {
            string nick = e.Who;
            display.OnPart(nick);
            game.OnPart(nick);
        }

        private void OnKick(object sender, KickEventArgs e)
        {
            string victim = e.Whom;
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
            string oldNick = e.OldNickname;
            string newNick = e.NewNickname;
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
            switch (e.Data.ReplyCode)
            {
                case ReplyCode.ErrorBannedFromChannel:
                    display.OnBanned();
                    game.OnBanned();
                    break;
                case ReplyCode.ErrorNoMotd:
                case ReplyCode.ErrorNotRegistered:
                    break;
                default:
                    display.OnError(e.Data.Message);
                    game.OnError(e.Data.Message);
                    break;
            }
        }
    }
}
