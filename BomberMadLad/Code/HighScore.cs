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
        public static string[] lines = File.ReadAllLines(@"C:\Users\william.persson8\Documents\Visual Studio 2017\Projects\BomberMadLad\HighScore\WriteLines.txt");

        public static void ReadHighScore()
        {
            // Display the file contents by using a foreach loop.
            Console.WriteLine("Contents of WriteLines2.txt = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static void AddHighScore()
        {
            List<int> score = new List<int>();

            for (int i = 0; i < lines.Length; i++)
            {
                score.Add(Convert.ToInt32(lines[i]));
                score.Sort();
            }
        }
        static public void ShowHighScore()
        {

        }

        static public void WriteHighScore()
        {

            string[] lines = { "first line", "second line", "third line", "fourth line", "fifth line" };

            StreamWriter file = new System.IO.StreamWriter(@"C:\Users\william.persson8\Documents\Visual Studio 2017\Projects\BomberMadLad\HighScore\WriteLines.txt");

            foreach (string line in lines)
            {
                // If the line doesn't contain the word 'Second', write the line to the file.
                if (!line.Contains("Second"))
                {
                    file.WriteLine(line);
                }
            }

        }
    }


}

