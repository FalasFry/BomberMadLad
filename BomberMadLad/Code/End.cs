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
        Menu menu;
        ConsoleColor Black;
        ConsoleColor Gray;
        int points;

        public End()
        {
            menu = new Menu();
            Black = ConsoleColor.Black;
            Gray = ConsoleColor.Gray;
            points = TimerClass.elapsedTime;
        }

        // Gör samma sak som main menu gör men denna är mer fokuserad på att avsluta spelet.
        public void GameOver(bool win)
        {
            HighScore.AddHighScore(points);

            int index = 0;

            menu.Buttons.Add("High Score");
            menu.Buttons.Add("Quit");

            MenuList(index);

            while(true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.DownArrow)
                {
                    if (index < menu.Buttons.Count - 1)
                    {
                        menu.MenuList(index + 1);
                        index = index + 1;
                    }
                }
                if (input == ConsoleKey.UpArrow)
                {
                    if (index > 0)
                    {
                        menu.MenuList(index - 1);
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

        // Gör samma sak som i Menu klassen men var tvungen att göra ny annars fungerade det inte. (menu.MenuList) fungerade inte korrekt.
        public void MenuList(int index)
        {
            Console.Clear();
            menu.ForColour(Gray);
            Console.WriteLine("Your Score Was " + points);
            for (int i = 0; i < menu.Buttons.Count; i++)
            {
                if (i != index)
                {
                    menu.ForColour(Gray);
                    menu.BackColour(Black);
                    Console.WriteLine(menu.Buttons[i]);
                }
                else if (i == index)
                {
                    menu.ForColour(Black);
                    menu.BackColour(Gray);
                    Console.WriteLine(menu.Buttons[index]);
                }
                Console.ResetColor();
            }
        }
    }


}
