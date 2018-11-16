using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BomberMadLad
{
    class End
    {
        int points;

        public End()
        {
            points = TimerClass.elapsedTime;
        }

        // Gör samma sak som main menu gör men denna är mer fokuserad på att avsluta spelet.
        public void GameOver(bool win)
        {
            if(win)
            {
                HighScore.AddHighScore(points);

                int index = 0;

                Menu.Buttons.Add("High Score");
                Menu.Buttons.Add("Quit");

                Menu.MenuList(index);

                // En meny med samma struktur som i main menu klassen, 
                // kan gjort om strukturen till en egen klass för lättare skapande och mindre hårdkodat.

                // Om du vinner spelet ska du få poäng.
                while (true)
                {
                    ConsoleKey input = Console.ReadKey(true).Key;
                    Console.WriteLine("You won the EPIC BomberLad Royal");
                    if (input == ConsoleKey.DownArrow)
                    {
                        if (index < Menu.Buttons.Count - 1)
                        {
                            Menu.MenuList(index + 1);
                            index = index + 1;
                        }
                    }
                    if (input == ConsoleKey.UpArrow)
                    {
                        if (index > 0)
                        {
                            Menu.MenuList(index - 1);
                            index = index - 1;
                        }
                    }

                    if (index == 0)
                    {
                        if (input == ConsoleKey.Enter)
                        {
                            HighScore.ShowHighScore();
                            Environment.Exit(0);
                        }
                    }
                    if (index == 1)
                    {
                        if (input == ConsoleKey.Enter)
                        {
                            HighScore.WriteHighScore();
                            Environment.Exit(0);
                        }
                    }
                }
            }
            // Om man förlorar får man inga poäng och spelet avslutas.
            if (!win)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Clear();
                Console.WriteLine("Game Over, You Lost!");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }
    }


}
