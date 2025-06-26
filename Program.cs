// Program:    DnD_NPC_Creator_V1
// Date:       26 JUN 2025
// Programmer: Thomas A. Morrison

using System;
using System.Collections.Immutable;
using System.Globalization;
namespace DnD_NCP_Creator_V1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This program rolls the stats for basic, non-elite Dungeons & Dragons NPCs: Adepts, Commoners, Experts, and Warriors.\n");
            Console.WriteLine("For each of the six attributes, it will use the sum of three six-sided dice, weighted toward mean value,");
            Console.WriteLine("  meaning that the result of each die can be 2, 3, 3, 4, 4, or 5, giving each attribute a range of 6-15.\n");
            Console.WriteLine("This program also assigns a random surname to the NPC from a bank of 100 Old English surnames.\n");
            Console.WriteLine("This program determines the NPC's level, using the following assumptions:");
            Console.WriteLine("  10% of Commoners would gain one or more levels.");
            Console.WriteLine("  25% of Adepts, Experts, and Warriors would gain one or more levels.\n");

            string[] surnames = 
            { 
                "Aldridge", "Alford", "Alston", "Arrington", "Astbury", "Barlow", "Bingham", "Blackburn", "Bolton", "Brandon", "Byron", "Caldwell", "Camden", "Cantrell", 
                "Carlisle", "Carlton", "Clark", "Clemons", "Clifford", "Compton", "Cooper", "Copeland", "Cotton", "Cromwell", "Davenport", "Davison", "Denton", "Drake", 
                "Dudley", "Dunham", "Eaton", "Fulton", "Gifford", "Godwin", "Hadley", "Hanley", "Hart", "Hastings", "Hawkins", "Hayden", "Helton", "Henley", "Hinton", 
                "Holbrook", "Hollingsworth", "Hurt", "Jacobs", "Kendall", "Kent", "Kirby", "Lancaster", "Langford", "Langley", "Langston", "Law", "Lester", "Linsday", 
                "Lockwood", "Lynn", "Manley", "Marlowe", "Mead", "Miller", "Milton", "Nevis", "Payton", "Ramsey", "Reeves", "Riley", "Rutherford", "Sanford", "Saxton",
                "Sheffield", "Sheldon", "Sherrow", "Shipley", "Shirley", "Smith", "Stapleton", "Stanford", "Stanton", "Stratton", "Strickland", "Sutherland", "Tatum", 
                "Thorpe", "Thornton", "Townsend", "Vance", "Waddell", "Wainwright", "Wesley", "Westbrook", "Weston", "Whaley", "Whitfield", "Whitmarsh", "Whitley", 
                "Whitney", "Yarbrough"
            };
            string[] attributes = { "STR", "DEX", "CON", "INT", "WIS", "CHA" };
            int[] values = { 2, 3, 3, 4, 4, 5 };
            int[] stats = new int[6];
            int[] rolls = new int[3];
            Random dice = new Random();
            string charClass;
            int level = 1;

            Console.WriteLine($"Surname: {surnames[dice.Next(0, 100)]}");
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rolls[j] = values[dice.Next(0, 6)];
                }
                stats[i] = rolls.Sum();
            }
            charClass = DetermineClass(stats);
            Console.WriteLine($"  Class: {charClass}");
            level = DetermineLevel(charClass);
            Console.WriteLine($"  Level: {level}");
            for (int i = 0; i < 6; i++) 
            {
                Console.WriteLine($"    {attributes[i]}: {stats[i],2}");
            }

            Console.WriteLine();

            Console.Write("Press any key to continue. ");
            Console.ReadKey();
        }

        static string DetermineClass(int[] stats) 
        {
            Random percent = new Random();
            int roll = percent.Next(1, 101);
            int temp;
            int index;
            int[] tempStats = new int[6];
            Array.Copy(stats, 0, tempStats, 0, 6);
            Array.Sort(tempStats);
            if (roll < 81)
            {
                return "Commoner";
            }
            else if (roll < 93)
            {
                if (tempStats[5] < 12 && tempStats[4] < 12 && tempStats[3] < 10)    // If the three highest rolls are too low, the NPC would be rejected for military service.
                {
                    return "Commoner";
                }
                if (stats[0] < 12 || stats[1] < 10 || stats[2] < 12)    // A Warrior needs a minimum of STR 12, DEX 10, CON 12.
                { 
                    stats[0] = tempStats[5];
                    stats[1] = tempStats[3];
                    stats[2] = tempStats[4];
                    stats[3] = tempStats[1];
                    stats[4] = tempStats[2];
                    stats[5] = tempStats[0];
                }
                return "Warrior";
            }
            else if (roll < 98)
            {
                if (tempStats[5] < 12 && tempStats[4] < 12 )    // If the two highest rolls are too low, the NPC would be rejected as an apprentice.
                {
                    return "Commoner";
                }
                if (stats[3] < 12 || stats[4] < 12)    // An Expert needs a minimum of INT 12, WIS 12: Crafting is an INT skill; Profession is a WIS skill.
                {
                    stats[0] = tempStats[3];
                    stats[1] = tempStats[0];
                    stats[2] = tempStats[2];
                    stats[3] = tempStats[5];
                    stats[4] = tempStats[4];
                    stats[5] = tempStats[1];
                }
                return "Expert";
            }
            else 
            {
                if (tempStats[5] < 13 && tempStats[4] < 10 && tempStats[3] < 10)    // If the three highest rolls are too low, the NPC would be rejected as an apprentice.
                {
                    return "Commoner";
                }
                if (stats[4] < 13 || stats[3] < 10)    // An Adept needs a minimum of INT 10, WIS 13: Divine spells require high WIS; Knowledge is an INT skill.
                {
                    stats[0] = tempStats[1];
                    stats[1] = tempStats[0];
                    stats[2] = tempStats[2];
                    stats[3] = tempStats[4];
                    stats[4] = tempStats[5];
                    stats[5] = tempStats[3];
                }
                return "Adept";
            }
        
        }
        static int DetermineLevel(string charClass) 
        {
            int level = 1;
            Random percent = new Random();
            if (charClass == "Commoner")
            {
                while (percent.Next(1, 101) <= 10)  // Commoners rarely seek out danger and are the least equipped to survive it.
                {
                    level++;
                }
            }
            else 
            {
                while (percent.Next(1, 101) <= 25)  // Warriors, adepts, and experts are more likely to seek out and survive danger.
                {
                    level++;
                }
            }
            return level;
        }
    }
}