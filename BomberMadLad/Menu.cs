using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Menu
    {
        public List<string> Buttons = new List<string>();
        public ConsoleColor gray = ConsoleColor.Gray;
        public ConsoleColor black = ConsoleColor.Black;

        public void MainMenu()
        {
            int index = 0;
            Buttons.Add("Start");
            Buttons.Add("Quit");
            Buttons.Add("Load");
            Buttons.Add("Save");
            Buttons.Add("HighScore");

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
                    if (input == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
                if (index == 1)
                {
                    if (input == ConsoleKey.Enter)
                    {
                        Environment.Exit(0);
                    }
                }
            }
            BackColour(black);
            ForColour(gray);
            Console.Clear();
            index = 1;

            Buttons.Clear();
            Buttons.Add("Do you want ai?");
            Buttons.Add("Yes");
            Buttons.Add("No");
            MenuList(index);

            while (true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.DownArrow && index < Buttons.Count - 1)
                {
                    MenuList(index + 1);
                    index = index + 1;

                }
                if (input == ConsoleKey.UpArrow && index > 1)
                {
                    MenuList(index - 1);
                    index = index - 1;
                }

                if (index == 1 && input == ConsoleKey.Enter)
                {
                    Program.haveAI = true;
                    break;
                }
                if (index == 2 && input == ConsoleKey.Enter)
                {
                    Program.haveAI = false;
                    break;
                }
            }
            Console.Clear();
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
                    ForColour(gray);
                    BackColour(black);
                    Console.WriteLine(Buttons[i]);
                }
                //index == 2
                else if (i == index)
                {
                    ForColour(black);
                    BackColour(gray);
                    Console.WriteLine(Buttons[index]);
                }
                Console.ResetColor();
            }

        }

    }
}
