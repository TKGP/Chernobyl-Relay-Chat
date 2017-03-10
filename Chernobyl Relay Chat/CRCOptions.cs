using Chernobyl_Relay_Chat.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    class CRCOptions
    {
        private static Settings settings = Settings.Default;

        public const string Server = "irc.slashnet.org";
#if DEBUG
        public const string Channel = "#crc_debug";
#else
        public const string Channel = "#crc";
#endif
        public const string InPath = @"\..\gamedata\configs\crc_input.txt";
        public const string OutPath = @"\..\gamedata\configs\crc_output.txt";

        public static FormWindowState DisplayState;
        public static Point DisplayLocation;
        public static Size DisplaySize;
        public static bool AutoFaction;
        public static string GameFaction;
        public static string ManualFaction;
        public static string Name;
        public static bool SendDeath;
        public static bool ReceiveDeath;
        public static int DeathInterval;

        public static string GetIrcName()
        {
            return Name.Replace(' ', '_');
        }

        public static string GetFaction()
        {
            if (AutoFaction)
                return GameFaction;
            else
                return ManualFaction;
        }

        public static void Load()
        {
            if (settings.FirstRun)
                settings.Upgrade();

            DisplayState = settings.DisplayState;
            DisplayLocation = settings.DisplayLocation;
            DisplaySize = settings.DisplaySize;
            AutoFaction = settings.AutoFaction;
            GameFaction = settings.GameFaction;
            ManualFaction = settings.ManualFaction;
            SendDeath = settings.SendDeath;
            ReceiveDeath = settings.ReceiveDeath;
            DeathInterval = settings.DeathInterval;

            if (settings.FirstRun)
                Name = CRCStrings.RandomName(GetFaction());
            else
                Name = settings.Name;
        }

        public static void Save()
        {
            settings.FirstRun = false;
            settings.DisplayState = DisplayState;
            settings.DisplayLocation = DisplayLocation;
            settings.DisplaySize = DisplaySize;
            settings.AutoFaction = AutoFaction;
            settings.GameFaction = GameFaction;
            settings.ManualFaction = ManualFaction;
            settings.Name = Name;
            settings.SendDeath = SendDeath;
            settings.ReceiveDeath = ReceiveDeath;
            settings.DeathInterval = DeathInterval;
            settings.Save();
        }
    }
}
