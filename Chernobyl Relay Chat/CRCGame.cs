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
        private static readonly Encoding encoding = Encoding.GetEncoding(1251);

        private ClientDisplay display;
        private CRCClient client;

        private bool disable = false;
        private int processID = -1;
        private string gamePath;
        private StringBuilder sendQueue = new StringBuilder();
        private object queueLock = new object();
        private Regex outputRx = new Regex("^(.+?)/(.+?)/(.+)$");
        private Regex deathRx = new Regex("^(.+?)/(.+?)/(.+)$");

        public CRCGame(ClientDisplay clientDisplay, CRCClient crcClient)
        {
            display = clientDisplay;
            client = crcClient;
        }

        private void Disable()
        {
            disable = true;
            MessageBox.Show("CRC was unable to read or write files needed to communicate with the game.\r\n"
                + "You may continue using the built-in client for now.\r\n"
                + "Please try running the program As Administrator.",
                "Chernobyl Relay Chat", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void GameCheck()
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
                        string path = Path.GetDirectoryName(process.Modules[0].FileName);
                        if (File.Exists(path + CRCOptions.InPath))
                        {
                            processID = process.Id;
                            gamePath = path;
                            break;
                        }
                    }
                }
            }
        }

        public void GameUpdate()
        {
            if (disable) return;

            if (processID != -1)
            {
                // Get messages from game
                try
                {
                    string[] lines = File.ReadAllLines(gamePath + CRCOptions.OutPath, encoding);
                    File.WriteAllText(gamePath + CRCOptions.OutPath, "", encoding);
                    foreach (string line in lines)
                    {
                        Match typeMatch = outputRx.Match(line);
                        string type = typeMatch.Groups[1].Value;
                        CRCOptions.GameFaction = CRCStrings.ValidateFaction(typeMatch.Groups[2].Value);
                        string body = typeMatch.Groups[3].Value;
                        if (type == "Message")
                        {
                            if (CRCOptions.GameFaction == "actor_zombied")
                                client.Send(CRCZombie.Generate());
                            else
                                client.Send(body);
                        }
                        else if (type == "Death" && CRCOptions.SendDeath && CRCOptions.GameFaction != "actor_zombied")
                        {
                            Match deathMatch = deathRx.Match(body);
                            string message = CRCStrings.DeathMessage(CRCOptions.Name, deathMatch.Groups[1].Value, deathMatch.Groups[2].Value, deathMatch.Groups[3].Value);
                            client.SendDeath(message);
                        }
                    }
                }
                catch (IOException) { }
                catch (Exception ex) when (ex is SecurityException || ex is UnauthorizedAccessException)
                {
                    Disable();
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
                    }
                }
            }
        }

        private void SendToGame(string line)
        {
            if (disable) return;

            if (processID != -1)
            {
                lock (sendQueue)
                {
                    sendQueue.AppendLine(line);
                }
            }
        }

        public void OnConnected()
        {
            SendToGame("Information/You are now connected to the network");
        }

        public void OnChannelMessage(string nick, string faction, string message)
        {
            SendToGame("Message/" + faction + "/" + nick + "/" + message);
        }

        public void OnQueryMessage(string from, string to, string faction, string message)
        {
            SendToGame("Query/" + faction + "/" + from + "/" + to + "/" + message);
        }

        public void OnJoin(string nick)
        {
            SendToGame("Information/" + nick + " has logged on");
        }

        public void OnPart(string nick)
        {
            SendToGame("Information/" + nick + " has logged off");
        }

        public void OnKick(string victim, string reason)
        {
            SendToGame("Information/" + victim + " has been kicked for: " + reason);
        }

        public void OnGotKicked(string reason)
        {
            SendToGame("Information/You have been kicked for: " + reason);
        }

        public void OnNickChange(string oldNick, string newNick)
        {
            SendToGame("Information/" + oldNick + " is now known as " + newNick);
        }

        public void OnOwnNickChange(string newNick)
        {
            SendToGame("Information/You are now known as " + newNick);
        }
    }
}
