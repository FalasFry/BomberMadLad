using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    static class Menu
    {
        // Skapar en lista med knappar.
        public static  List<string> Buttons = new List<string>();

        static ConsoleColor Gray { get; set; }
        static ConsoleColor Black { get; set; }
        static string logo = (@"  ____                        _                     __  __               _   _                    _ 
 |  _ \                      | |                   |  \/  |             | | | |                  | |
 | |_) |   ___    _ __ ___   | |__     ___   _ __  | \  / |   __ _    __| | | |        __ _    __| |
 |  _ <   / _ \  |  _   _ \  |  _ \   / _ \ |  __| | |\/| |  / _  |  / _  | | |       / _  |  / _  |
 | |_) | | (_) | | | | | | | | |_) | |  __/ | |    | |  | | | (_| | | (_| | | |____  | (_| | | (_| |
 |____/   \___/  |_| |_| |_| |____/   \___| |_|    |_|  |_|  \____|  \____| |______|  \____|  \____|");

        public static  void MainMenu()
        {
            Black = ConsoleColor.Black;
            Gray = ConsoleColor.Gray;

            int index = 1;

            // Lägger till tre knappar.
            Buttons.Add(logo);
            Buttons.Add("Start");
            Buttons.Add("Quit");

            MenuList(index);

            // Fixar Knapptryck.
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
                    if (index > 1)
                    {
                        MenuList(index - 1);
                        index = index - 1;
                    }
                }

                if (index == 1)
                {
                    if (input == ConsoleKey.Enter)
                    {
                        Program.mygame = new Game(Console.LargestWindowWidth - 13, Console.LargestWindowHeight - 12);
                        break;
                    }
                }
                if (index == 2)
                {
                    if (input == ConsoleKey.Enter)
                    {
                        Environment.Exit(0);
                    }
                }
            }
            BackColour(Black);
            ForColour(Gray);
            Console.Clear();
        }

        // Sätter färgen på texten till det man vill.
        public static void ForColour(ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
        }
        // Sätter bakgrundsfärgen till det man vill.
        public static  void BackColour(ConsoleColor consoleColor)
        {
            Console.BackgroundColor = consoleColor;
        }

        // Ritar ut menyn så fint det går.
        public static  void MenuList(int index)
        {
            // Suddar allt.
            Console.Clear();
            // Ritar ut varje knapp men om knappen är dem du har markerat är den grå.
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
