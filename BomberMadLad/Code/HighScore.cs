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
        // Filen som texten sparas till.
        static string path = "HighScore.txt";

        // Score som finns i filen.
        static string[] OldHighScore;

        static List<int> Score = new List<int>();
        
        // Sparar ner gamla higscore från .txt filen till en array.
        public static void ReadHighScore()
        {
            OldHighScore = File.ReadAllLines(path);
        }

        // Lägger till de gamla och de nya poängen i en lista.
        public static void AddHighScore(int thisScore)
        {
            ReadHighScore();

            Score.Add(thisScore);

            for (int i = 0; i < OldHighScore.Length; i++)
            {
                Score.Add(Convert.ToInt32(OldHighScore[i]));
                Score.Sort();
            }
        }

        // Skriver ut highscore som man har och visar i vilket som är mest och vilket som är älst.
        static public void ShowHighScore()
        {
            Console.Clear();
            WriteHighScore();

            Console.WriteLine("HighScores is:  ");

            // Om det finns två score så skriver den de bästa först.
            // Om det bara finns ett score skriver den det som bäst.
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
        }

        // Skriver ner allt till en fil.
        static public void WriteHighScore()
        {
            List<string> lines = new List<string>();

            for (int i = 0; i < Score.Count; i++)
            {
                lines.Add(Convert.ToString(Score[i]));
            }

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

