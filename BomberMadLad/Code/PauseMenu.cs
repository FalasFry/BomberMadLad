using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class PauseMenu
    {
        public List<string> Buttons = new List<string>();

        public ConsoleColor Gray { get; set; }
        public ConsoleColor Black { get; set; }

        public void Pause()
        {
            Black = ConsoleColor.Black;
            Gray = ConsoleColor.Gray;

            int index = 0;
            Buttons.Add("Resume");
            Buttons.Add("MainMenu");

            MenuList(index);

            while (true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.DownArrow)
                {
                    if (index < Buttons.Count - 1)
                    {
                        MenuList(index + 1);
                        index = index + 1;
                    }
                }
                if (input == ConsoleKey.UpArrow)
                {
                    if (index > 0)
                    {
                        MenuList(index - 1);
                        index = index - 1;
                    }
                }

                if (index == 0)
                {
                    Console.Clear();
                    if (input == ConsoleKey.Enter)
                    {
                        for (int i = 0; i < Program.mygame.Walls.Count; i++)
                        {
                            Program.mygame.Walls[i].Draw(0,0);
                        }
                        break;
                    }
                }
                if (index == 1)
                {
                    if (input == ConsoleKey.Enter)
                    {
                        Menu menu = new Menu();
                        menu.MainMenu();
                    }
                }
            }
            BackColour(Black);
            ForColour(Gray);
        }

        public void ForColour(ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
        }
        public void BackColour(ConsoleColor consoleColor)
        {
            Console.BackgroundColor = consoleColor;
        }

        public void MenuList(int index)
        {
            Console.Clear();
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (i != index)
                {
                    ForColour(Gray);
                    BackColour(Black);
                    Console.WriteLine(Buttons[i]);
                }
                else if (i == index)
                {
                    ForColour(Black);
                    BackColour(Gray);
                    Console.WriteLine(Buttons[index]);
                }
                Console.ResetColor();
            }
        }
    }
}

