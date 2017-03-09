using Microsoft.Win32;

namespace Chernobyl_Relay_Chat
{
    class CRCOptions
    {
        private static RegistryKey registry = Registry.CurrentUser.CreateSubKey(@"Software\CRC");

        public const string Server = "irc.slashnet.org";
#if DEBUG
        public const string Channel = "#crc_debug";
#else
        public const string Channel = "#crc";
#endif
        public const string InPath = @"\..\gamedata\configs\crc_input.txt";
        public const string OutPath = @"\..\gamedata\configs\crc_output.txt";

        public static bool AutoFaction;
        private static string faction;
        public static string Faction
        {
            get { return faction; }
            set
            {
                faction = CRCStrings.ValidateFaction(value);
            }
        }
        public static string Name;
        public static bool SendDeath;
        public static bool ReceiveDeath;
        private static int deathInterval;
        public static int DeathInterval
        {
            get { return deathInterval; }
            set
            {
                if (value >= 0 && value <= 3600)
                    deathInterval = value;
                else
                    deathInterval = 60;
            }
        }

        public static string IrcName
        {
            get
            {
                return Name.Replace(' ', '_');
            }
        }

        public static void Init()
        {
            AutoFaction = (string)registry.GetValue("AutoFaction", "True") == "True";
            Faction = (string)registry.GetValue("Faction", "stalker");
            Name = (string)registry.GetValue("Name", CRCStrings.RandomName(Faction));
            SendDeath = (string)registry.GetValue("SendDeath", "True") == "True";
            ReceiveDeath = (string)registry.GetValue("ReceiveDeath", "True") == "True";
            DeathInterval = (int)registry.GetValue("DeathInterval", 60);
        }

        public static void Save()
        {
            registry.SetValue("AutoFaction", AutoFaction);
            registry.SetValue("Faction", Faction);
            registry.SetValue("Name", Name);
            registry.SetValue("SendDeath", SendDeath);
            registry.SetValue("ReceiveDeath", ReceiveDeath);
            registry.SetValue("DeathInterval", DeathInterval);
        }
    }
}
