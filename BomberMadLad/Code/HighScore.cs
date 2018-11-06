using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BomberMadLad
{
    static class HighScore
    {
        static void ReadHighScore()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\william.persson8\Documents\Visual Studio 2017\Projects\BomberMadLad\HighScore\WriteLines.txt");

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of WriteLines2.txt = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static void AddHighScore()
        {
            List<int> score = new List<int>();
        }
        static void ShowHighScore()
        {

        }

        static void WriteHighScore()
        {

            string[] lines = { "first line", "second line", "third line", "fourth line", "fifth line" };

            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\william.persson8\Documents\Visual Studio 2017\Projects\BomberMadLad\HighScore\WriteLines.txt");

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

