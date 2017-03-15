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

        private static readonly IrcClient client = new IrcClient();
        private static DateTime lastDeath = new DateTime();
        private static string lastQuery;
        private static Thread listenThread;

#if DEBUG
        private static DebugDisplay debug = new DebugDisplay();
        private static Thread debugThread;
#endif

        public static void Start()
        {
#if DEBUG
            debugThread = new Thread(() => Application.Run(debug));
            debugThread.Start();
#endif
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
            client.Listen();
#if DEBUG
            debug.Invoke(new Action(() => debug.Close()));
            debugThread.Join();
#endif
        }

        public static void Stop()
        {
            if (client.IsConnected)
            {
                client.RfcQuit("Safe");
                client.Disconnect();
            }
        }

        public static void UpdateSettings()
        {
            client.RfcNick(CRCOptions.Name);
            CRCGame.UpdateSettings();
        }

        public static void ChangeNick(string nick)
        {
            CRCOptions.Name = nick;
            client.RfcNick(nick);
        }

        public static void Send(string message)
        {
            client.SendMessage(SendType.Message, CRCOptions.Channel, CRCOptions.GetFaction() + META_DELIM + message);
            CRCDisplay.OnOwnChannelMessage(CRCOptions.Name, message);
            CRCGame.OnChannelMessage(CRCOptions.Name, CRCOptions.GetFaction(), message);
        }

        public static void SendDeath(string message)
        {
            string nick = CRCStrings.RandomName(CRCOptions.GameFaction);
            client.SendMessage(SendType.Message, CRCOptions.Channel, nick + FAKE_DELIM + CRCOptions.GetFaction() + META_DELIM + message);
            CRCDisplay.OnChannelMessage(nick, message);
            CRCGame.OnChannelMessage(nick, CRCOptions.GameFaction, message);
        }

        public static void SendQuery(string nick, string message)
        {
            client.SendMessage(SendType.Message, nick, CRCOptions.GetFaction() + META_DELIM + message);
            CRCDisplay.OnQueryMessage(CRCOptions.Name, nick, message);
            CRCGame.OnQueryMessage(CRCOptions.Name, nick, CRCOptions.GetFaction(), message);
        }

        public static bool SendReply(string message)
        {
            if (lastQuery != null)
            {
                SendQuery(lastQuery, message);
                return true;
            }
            return false;
        }

        private static string GetMetadata(string message, out string fakeNick, out string faction)
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



        private static void OnConnection(object sender, EventArgs e)
        {
            client.Login(CRCOptions.Name, "Chernobyl Relay Chat " + Application.ProductVersion);
            client.RfcJoin(CRCOptions.Channel);
        }

        private static void OnChannelActiveSynced(object sender, IrcEventArgs e)
        {
            List<string> users = new List<string>();
            foreach (ChannelUser user in client.GetChannel(CRCOptions.Channel).Users.Values)
                users.Add(user.Nick);
            CRCDisplay.OnChannelActiveSynced(users);
        }

        private static void OnRawMessage(object sender, IrcEventArgs e)
        {
#if DEBUG
            debug?.AddRaw(e.Data.RawMessage);
#endif
        }

        private static void OnChannelMessage(object sender, IrcEventArgs e)
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
                    CRCDisplay.OnHighlightMessage(nick, message);
                    CRCGame.OnHighlightMessage(nick, faction, message);
                }
                else
                {
                    CRCDisplay.OnChannelMessage(nick, message);
                    CRCGame.OnChannelMessage(nick, faction, message);
                }
            }
        }

        private static void OnQueryMessage(object sender, IrcEventArgs e)
        {
            lastQuery = e.Data.Nick;
            string fakeNick, faction;
            string message = GetMetadata(e.Data.Message, out fakeNick, out faction);
            // Never use fakeNick for query
            string nick = e.Data.Nick;
            CRCDisplay.OnQueryMessage(nick, CRCOptions.Name, message);
            CRCGame.OnQueryMessage(nick, CRCOptions.Name, faction, message);
        }

        private static void OnJoin(object sender, JoinEventArgs e)
        {
            string nick = e.Who;
            if (nick != CRCOptions.Name)
            {
                CRCDisplay.OnJoin(nick);
                CRCGame.OnJoin(nick);
            }
            else
            {
                CRCDisplay.OnConnected();
                CRCGame.OnConnected();
            }
        }

        private static void OnPart(object sender, PartEventArgs e)
        {
            string nick = e.Who;
            CRCDisplay.OnPart(nick);
            CRCGame.OnPart(nick);
        }

        private static void OnQuit(object sender, QuitEventArgs e)
        {
            string nick = e.Who;
            CRCDisplay.OnPart(nick);
            CRCGame.OnPart(nick);
        }

        private static void OnKick(object sender, KickEventArgs e)
        {
            string victim = e.Whom;
            if (victim == CRCOptions.Name)
            {
                CRCDisplay.OnGotKicked(e.KickReason);
                CRCGame.OnGotKicked(e.KickReason);
            }
            else
            {
                CRCDisplay.OnKick(victim, e.KickReason);
                CRCGame.OnKick(victim, e.KickReason);
            }
        }

        private static void OnNickChange(object sender, NickChangeEventArgs e)
        {
            string oldNick = e.OldNickname;
            string newNick = e.NewNickname;
            if (newNick != CRCOptions.Name)
            {
                CRCDisplay.OnNickChange(oldNick, newNick);
                CRCGame.OnNickChange(oldNick, newNick);
            }
            else
            {
                CRCDisplay.OnOwnNickChange(oldNick, newNick);
                CRCGame.OnOwnNickChange(newNick);
            }
        }

        private static void OnErrorMessage(object sender, IrcEventArgs e)
        {
            switch (e.Data.ReplyCode)
            {
                case ReplyCode.ErrorBannedFromChannel:
                    CRCDisplay.OnBanned();
                    CRCGame.OnBanned();
                    break;
                case ReplyCode.ErrorNoMotd:
                case ReplyCode.ErrorNotRegistered:
                    break;
                default:
                    CRCDisplay.OnError(e.Data.Message);
                    CRCGame.OnError(e.Data.Message);
                    break;
            }
        }
    }
}
