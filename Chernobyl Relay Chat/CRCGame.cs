using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Chernobyl_Relay_Chat
{
    class CRCGame
    {
        private ClientDisplay display;
        private CRCClient client;

        private int processID = -1;
        private string gamePath;
        private Regex outputRx = new Regex("^(.+?)/(.+?)/(.+)$");
        private Regex deathRx = new Regex("^(.+?)/(.+?)/(.+)$");

        public CRCGame(ClientDisplay clientDisplay, CRCClient crcClient)
        {
            display = clientDisplay;
            client = crcClient;
        }

        public void GameCheck()
        {
            if (processID != -1)
            {
                try
                {
                    Process.GetProcessById(processID);
                }
                catch (ArgumentException)
                {
                    processID = -1;
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
                        }
                    }
                }
            }
        }

        public void GameUpdate()
        {
            if (processID != -1)
            {
                try
                {
                    string[] lines = File.ReadAllLines(gamePath + CRCOptions.OutPath);
                    File.WriteAllText(gamePath + CRCOptions.OutPath, "");
                    foreach (string line in lines)
                    {
                        Match typeMatch = outputRx.Match(line);
                        string type = typeMatch.Groups[1].Value;
                        string faction = typeMatch.Groups[2].Value;
                        if (CRCOptions.AutoFaction)
                            CRCOptions.Faction = faction;
                        string body = typeMatch.Groups[3].Value;
                        if (type == "Message")
                        {
                            client.Send(body);
                        }
                        else if (type == "Death" && CRCOptions.SendDeath)
                        {
                            Match deathMatch = deathRx.Match(body);
                            string message = CRCStrings.DeathMessage(CRCOptions.Name, deathMatch.Groups[1].Value, deathMatch.Groups[2].Value, deathMatch.Groups[3].Value);
                            client.SendDeath(message, faction);
                        }
                    }
                }
                catch (IOException) { }
            }
        }

        private void SendToGame(string line)
        {
            if (processID != -1)
            {
                try
                {
                    File.AppendAllText(gamePath + CRCOptions.InPath, line + "\r\n");
                }
                catch (IOException) { }
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
