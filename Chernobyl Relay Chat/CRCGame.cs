using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    class CRCGame
    {
        private static readonly CRCGameWrapper wrapper = new CRCGameWrapper();
        private static readonly Encoding encoding = Encoding.GetEncoding(1251);
        private static readonly Regex outputRx = new Regex("^(.+?)(?:/(.+))?$");
        private static readonly Regex messageRx = new Regex("^(.+?)/(.+)$");
        private static readonly Regex deathRx = new Regex("^(.+?)/(.+?)/(.+?)/(.+)$");

        private static bool disable = false;
        private static int processID = -1;
        private static string gamePath;
        private static bool firstClear = false;
        private static StringBuilder sendQueue = new StringBuilder();
        private static object queueLock = new object();

        private static ClientDisplay display;
        private static CRCClient client;

        public CRCGame(ClientDisplay clientDisplay, CRCClient crcClient)
        {
            display = clientDisplay;
            client = crcClient;
        }

        private static void Disable()
        {
            disable = true;
            MessageBox.Show("CRC was unable to read or write files needed to communicate with the game.\r\n"
                + "You may continue using the built-in client for now.\r\n"
                + "Please try running the program As Administrator.",
                "Chernobyl Relay Chat", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void GameCheck()
        {
            if (disable) return;

            if (processID != -1)
            {
                try
                {
                    Process.GetProcessById(processID);
                }
                catch (ArgumentException)
                {
                    processID = -1;
                    lock (queueLock)
                    {
                        sendQueue.Clear();
                    }
                }
            }
            if (processID == -1)
            {
                foreach (Process process in Process.GetProcesses())
                {
                    if (process.ProcessName == "xrEngine")
                    {
                        string path = Path.GetDirectoryName(process.GetProcessPath());
                        if (File.Exists(path + CRCOptions.InPath))
                        {
                            gamePath = path;
                            firstClear = false;
                            processID = process.Id;
                            UpdateSettings();
                            break;
                        }
                    }
                }
            }
        }

        public static void GameUpdate()
        {
            if (disable || processID == -1) return;

            // Wipe game output when first discovered
            if (!firstClear)
            {
                try
                {
                    File.WriteAllText(gamePath + CRCOptions.OutPath, "", encoding);
                    firstClear = true;
                }
                catch (IOException)
                {
                    return;
                }
                catch (Exception ex) when (ex is SecurityException || ex is UnauthorizedAccessException)
                {
                    Disable();
                    return;
                }
            }

            // Get messages from game
            try
            {
                string[] lines = File.ReadAllLines(gamePath + CRCOptions.OutPath, encoding);
                File.WriteAllText(gamePath + CRCOptions.OutPath, "", encoding);
                foreach (string line in lines)
                {
                    Match typeMatch = outputRx.Match(line);
                    string type = typeMatch.Groups[1].Value;
                    if (type == "Settings")
                    {
                        UpdateSettings();
                    }
                    else if (type == "Message")
                    {
                        Match messageMatch = messageRx.Match(typeMatch.Groups[2].Value);
                        string faction = messageMatch.Groups[1].Value;
                        string message = messageMatch.Groups[2].Value;
                        if (message[0] == '/')
                            CRCCommands.ProcessCommand(message, wrapper);
                        else
                        {
                            CRCOptions.GameFaction = CRCStrings.ValidateFaction(faction);
                            if (CRCOptions.GameFaction == "actor_zombied")
                                CRCClient.Send(CRCZombie.Generate());
                            else
                                CRCClient.Send(message);
                        }
                    }
                    else if (type == "Death" && CRCOptions.SendDeath)
                    {
                        Match deathMatch = deathRx.Match(typeMatch.Groups[2].Value);
                        string faction = deathMatch.Groups[1].Value;
                        string level = deathMatch.Groups[2].Value;
                        string xrClass = deathMatch.Groups[3].Value;
                        string section = deathMatch.Groups[4].Value;
                        CRCOptions.GameFaction = CRCStrings.ValidateFaction(faction);
                        if (CRCOptions.GameFaction != "actor_zombied")
                        {
                            string message = CRCStrings.DeathMessage(CRCOptions.Name, level, xrClass, section);
                            CRCClient.SendDeath(message);
                        }
                    }
                }
            }
            catch (IOException) { }
            catch (Exception ex) when (ex is SecurityException || ex is UnauthorizedAccessException)
            {
                Disable();
                return;
            }

            // Send messages to game
            lock (sendQueue)
            {
                try
                {
                    File.AppendAllText(gamePath + CRCOptions.InPath, sendQueue.ToString(), encoding);
                    sendQueue.Clear();
                }
                catch (IOException) { }
                catch (Exception ex) when (ex is SecurityException || ex is UnauthorizedAccessException)
                {
                    Disable();
                    return;
                }
            }
        }

        public static void UpdateSettings()
        {
            SendToGame("Setting/NewsDuration/" + (CRCOptions.NewsDuration * 1000));
            SendToGame("Setting/ChatKey/DIK_" + CRCOptions.ChatKey);
        }

        private static void SendToGame(string line)
        {
            if (disable || processID == -1) return;

            lock (sendQueue)
            {
                sendQueue.AppendLine(line);
            }
        }

        public static void AddInformation(string message)
        {
            SendToGame("Information/" + message);
        }

        public static void AddError(string message)
        {
            SendToGame("Error/" + message);
        }


        public static void OnBanned()
        {
            AddError("Woops, you're banned!");
        }

        public static void OnError(string message)
        {
            AddError(message);
        }

        public static void OnUpdate(string message)
        {
            AddInformation(message);
        }

        public static void OnConnected()
        {
            AddInformation("You are now connected to the network");
        }

        public static void OnHighlightMessage(string nick, string faction, string message)
        {
            SendToGame("Highlight/" + faction + "/" + nick + "/" + message);
        }

        public static void OnChannelMessage(string nick, string faction, string message)
        {
            SendToGame("Message/" + faction + "/" + nick + "/" + message);
        }

        public static void OnQueryMessage(string from, string to, string faction, string message)
        {
            SendToGame("Query/" + faction + "/" + from + "/" + to + "/" + message);
        }

        public static void OnJoin(string nick)
        {
            AddInformation(nick + " has logged on");
        }

        public static void OnPart(string nick)
        {
            AddInformation(nick + " has logged off");
        }

        public static void OnKick(string victim, string reason)
        {
            AddInformation(victim + " has been kicked for: " + reason);
        }

        public static void OnGotKicked(string reason)
        {
            AddInformation("You have been kicked for: " + reason);
        }

        public static void OnNickChange(string oldNick, string newNick)
        {
            AddInformation(oldNick + " is now known as " + newNick);
        }

        public static void OnOwnNickChange(string newNick)
        {
            AddInformation("You are now known as " + newNick);
        }
    }
}
