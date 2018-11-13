using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace BomberMadLad
{
    static class HighScore
    {
        static string path = "HighScore.txt";
        public static string[] OldHighScore;

        public static List<int> Score = new List<int>();

        public static void ReadHighScore()
        {
            OldHighScore = File.ReadAllLines(path);
        }

        public static void AddHighScore(int thisScore)
        {
            ReadHighScore();

            Score.Add(thisScore);

            for (int i = 0; i < OldHighScore.Length; i++)
            {
                Score.Add(Convert.ToInt32(OldHighScore[i]));
                Score.Sort();
            }

            if(Score.Count > 5)
            {
                while(Score.Count > 5)
                {
                    Score.Sort();
                    Score.RemoveAt(0);
                }
            }
        }
        static public void ShowHighScore()
        {
            Console.Clear();
            WriteHighScore();

            Console.WriteLine("Contents of HighScore = ");
            Console.ForegroundColor = ConsoleColor.Green;
            if (Score.Count > 1)
            {
                Console.WriteLine("\t" + "1st " + "place got: " + Convert.ToString(Score[1]) + " Score");
                Console.WriteLine("\t" + "2nd " + "place got: " + Convert.ToString(Score[0]) + " Score");
            }
            else
            {
                Console.WriteLine("\t" + "1st " + "place got: " + Convert.ToString(Score[0]) + " Score");
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        static public void WriteHighScore()
        {
            List<string> lines = new List<string>();

            for (int i = 0; i < Score.Count; i++)
            {
                lines.Add(Convert.ToString(Score[i]));
            }

            //StreamWriter file = new StreamWriter(path);


            for (int i = 0; i < lines.Count; i++)
            {
                if(lines.Count <= 5)
                {
                    File.WriteAllText(path, lines[i]);
                }
            }

        }
    }


}

