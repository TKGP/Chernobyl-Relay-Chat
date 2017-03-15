using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Chernobyl_Relay_Chat
{
    public class CRCStrings
    {
        private const int GENERIC_CHANCE = 10;
        private const int REMARK_CHANCE = 25;

        private static readonly Random rand = new Random();
        private static List<string> deathFormats, deathTimes, deathObservances, deathRemarks, deathGeneric;
        private static Dictionary<string, List<string>> deathLevels, deathSections, deathClasses, fNames, sNames;

        private static readonly Regex invalidNickRx = new Regex(@"[^a-zA-Z0-9_\-\\^{}|]");
        private static readonly Regex invalidNickFirstCharRx = new Regex(@"[^a-zA-Z_\\^{}|]");

        public static void Load()
        {
            deathFormats = loadXmlList(@"res\death_formats.xml");
            deathTimes = loadXmlList(@"res\death_times.xml");
            deathObservances = loadXmlList(@"res\death_observances.xml");
            deathRemarks = loadXmlList(@"res\death_remarks.xml");
            deathLevels = loadXmlListDict(@"res\death_levels.xml");
            deathSections = loadXmlListDict(@"res\death_sections.xml");
            deathClasses = loadXmlListDict(@"res\death_classes.xml");
            deathGeneric = loadXmlList(@"res\death_generic.xml");

            fNames = loadXmlListDict(@"res\fnames.xml");
            sNames = loadXmlListDict(@"res\snames.xml");

            fNames["actor_csky"] = MergeLists(fNames, "actor_stalker", "actor_ecolog");
            fNames["actor_dolg"] = MergeLists(fNames, "actor_stalker", "actor_army");
            fNames["actor_freedom"] = MergeLists(fNames, "actor_stalker", "actor_bandit");
            fNames["actor_killer"] = MergeLists(fNames, "actor_stalker", "actor_bandit", "actor_ecolog");
            fNames["actor_monolith"] = MergeLists(fNames, "actor_stalker", "actor_bandit", "actor_ecolog");
            fNames["actor_zombied"] = MergeLists(fNames, "actor_stalker", "actor_bandit", "actor_ecolog", "actor_army");

            sNames["actor_csky"] = MergeLists(sNames, "actor_stalker", "actor_ecolog");
            sNames["actor_dolg"] = MergeLists(sNames, "actor_stalker", "actor_army");
            sNames["actor_freedom"] = MergeLists(sNames, "actor_stalker", "actor_bandit");
            sNames["actor_killer"] = MergeLists(sNames, "actor_stalker", "actor_bandit", "actor_ecolog");
            sNames["actor_monolith"] = MergeLists(sNames, "actor_stalker", "actor_bandit", "actor_ecolog");
            sNames["actor_zombied"] = MergeLists(sNames, "actor_stalker", "actor_bandit", "actor_ecolog", "actor_army");
        }

        private static string PickRandom(List<string> list)
        {
            return list[rand.Next(list.Count)];
        }

        private static List<string> MergeLists(Dictionary<string, List<string>> listDict, params string[] keys)
        {
            List<string> list = new List<string>();
            foreach (string key in keys)
            {
                list = list.Concat(listDict[key]).ToList();
            }
            return list;
        }

        public static string RandomName(string faction)
        {
            faction = ValidateFaction(faction);
            return PickRandom(fNames[faction]) + " " + PickRandom(sNames[faction]);
        }

        public static string ValidateFaction(string faction)
        {
            return validFactions.Contains(faction) ? faction : "actor_stalker";
        }

        public static string ValidateNick(string nick)
        {
            if (nick.Length < 1)
                return "Nicks must be at least 1 character long.";
            if (nick.Length > 30)
                return "Nicks must be at most 30 characters long.";
            if (invalidNickRx.Match(nick).Success)
                return @"Nicks may only contain letters, numbers, and these characters: _-\^{}|";
            if (invalidNickFirstCharRx.Match(nick).Success)
                return "Nicks may not begin with a number or -";
            return null;
        }

        public static string DeathMessage(string name, string level, string xrClass, string section)
        {
            string levelText = deathLevels.ContainsKey(level) ? PickRandom(deathLevels[level]) : ("somewhere in the Zone (" + level + ")");
            string deathText;
            if (rand.Next(101) < GENERIC_CHANCE)
                deathText = PickRandom(deathGeneric);
            else if (deathSections.ContainsKey(section))
                deathText = PickRandom(deathSections[section]);
            else if (deathClasses.ContainsKey(xrClass))
                deathText = PickRandom(deathClasses[xrClass]);
            else
                deathText = "died of unknown causes (" + xrClass + "|" + section + ")";

            string message = PickRandom(deathFormats);
            message = message.Replace("$when", PickRandom(deathTimes));
            message = message.Replace("$level", levelText);
            message = message.Replace("$saw", PickRandom(deathObservances));
            message = message.Replace("$name", name);
            message = message.Replace("$death", deathText);
            message = message[0].ToString().ToUpper() + message.Substring(1);
            if (rand.Next(101) < REMARK_CHANCE)
                message += ' ' + PickRandom(deathRemarks);
            return message;
        }

        private static List<string> validFactions = new List<string>()
        {
            "actor_bandit",
            "actor_csky",
            "actor_dolg",
            "actor_ecolog",
            "actor_freedom",
            "actor_stalker",
            "actor_killer",
            "actor_army",
            "actor_monolith",
            "actor_zombied",
        };

        private static List<string> loadXmlList(string path)
        {

            List<string> list = new List<string>();
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(path);
                foreach (XmlNode stringNode in xml.DocumentElement.ChildNodes)
                {
                    list.Add(stringNode.InnerText);
                }
            }
            catch (Exception ex) when (ex is XmlException || ex is FileNotFoundException) { }
            return list;
        }

        private static Dictionary<string, List<string>> loadXmlListDict(string path)
        {
            Dictionary<string, List<string>> listDict = new Dictionary<string, List<string>>();
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(path);
                foreach (XmlNode keyNode in xml.DocumentElement.ChildNodes)
                {
                    string key = keyNode.Name;
                    listDict[key] = new List<string>();
                    XmlNode clone = keyNode.Attributes["clone"];
                    if (clone != null)
                    {
                        listDict[key] = listDict[clone.Value];
                    }
                    else
                    {
                        foreach (XmlNode stringNode in keyNode.ChildNodes)
                        {
                            listDict[key].Add(stringNode.InnerText);
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is XmlException || ex is FileNotFoundException) { }
            return listDict;
        }
    }
}
