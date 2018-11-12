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
        public static string[] OldHighScore;

        public static List<int> Score = new List<int>();

        public static void ReadHighScore()
        {
            OldHighScore = File.ReadAllLines(@"C:\Users\william.persson8\Documents\Visual Studio 2017\Projects\BomberMadLad\HighScore\WriteLines.txt");
        }

        public static void AddHighScore(int thisScore)
        {
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
            WriteHighScore();
            Console.WriteLine("Contents of HighScore = ");
            for (int i = 0; i < Score.Count; i++)
            {
                Console.WriteLine("\t" + i+1 + "s place got: " + Convert.ToString(Score[i]) + " Score");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        static public void WriteHighScore()
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                lines.Add(Convert.ToString(Score[i]));
            }

            StreamWriter file = new StreamWriter(@"C:\Users\william.persson8\Documents\Visual Studio 2017\Projects\BomberMadLad\HighScore\WriteLines.txt");

            foreach (string line in lines)
            {
                if (lines.Count <= 5)
                {
                    file.WriteLine(line);
                }
            }

        }
    }


}

