using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class PlayerControl : GameObject
    {

        public PlayerControl()
        {
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
        }

        public override void Update()
        {
            ConsoleKey input = new ConsoleKey();
            if (Console.KeyAvailable)
            {
                input = Console.ReadKey(true).Key;
            }

            if (input == ConsoleKey.W)
            {
                Program.mygame.player.YPosition--;
            }
            if (input == ConsoleKey.S)
            {
                Program.mygame.player.YPosition++;
            }
            if (input == ConsoleKey.A)
            {
                Program.mygame.player.XPosition -= 2;
            }
            if (input == ConsoleKey.D)
            {
                Program.mygame.player.XPosition += 2;
            }
        }

        #region Actions
        public override void Action1()
        {
        }

        public override void Action2()
        {
        }

        public override void Action3()
        {
        }
        #endregion
    }
}
