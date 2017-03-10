using System;
using System.Collections.Generic;
using System.Text;

namespace Chernobyl_Relay_Chat
{
    // Quarantined in here instead of CRCStrings because it's totally grody

    class CRCZombie
    {
        private static Random rand = new Random();

        private static string PickRandom(List<string> list)
        {
            return list[rand.Next(list.Count)];
        }

        public static string Generate()
        {
            StringBuilder zombieSb = new StringBuilder();
            for (int x = 0; x != rand.Next(5) + 5; x++)
            {
                string word;
                if (rand.Next(5) == 0)
                    word = "brains";
                else
                    word = PickRandom(zombiePrefix) + PickRandom(zombieMiddle) + PickRandom(zombieSuffix);
                List<char> letters = new List<char>();
                foreach (char letter in word)
                    letters.Add(letter);
                for (int n = 0; n <= letters.Count; n++)
                {
                    int index = rand.Next(letters.Count);
                    word = word.Replace(letters[index].ToString(), new String(letters[index], rand.Next(5) + 1));
                    letters.RemoveAt(index);
                }
                if (x > 0)
                    zombieSb.Append(" ");
                zombieSb.Append(word);
            }
            zombieSb.Append("...");
            string zombie = zombieSb.ToString();
            zombie = zombie[0].ToString().ToUpper() + zombie.Substring(1);
            return zombie;
        }

        private static List<string> zombiePrefix = new List<string>()
        {
            "e",
            "u",
            "a",
            "r",
            "gr",
            "g",
            "b",
            "bl",
            "m",
            "h",
            "n",
        };

        private static List<string> zombieMiddle = new List<string>()
        {
            "a",
            "e",
            "u",
            "h",
        };

        private static List<string> zombieSuffix = new List<string>()
        {
            "e",
            "u",
            "a",
            "r",
            "rg",
            "g",
            "gh",
            "h",
            "n",
            "hn",
            "hg",
        };
    }
}
