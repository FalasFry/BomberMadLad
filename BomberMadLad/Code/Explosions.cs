using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Explosions : GameObject
    {


        public override void Update()
        {
        }
        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("██");
        }

        public override void Action1()
        {
            //ta bort explosioner
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("  ");

            //om det är en karaktär i explosiosionen tar spelet sl
            if (!Collision.Char(XPosition, YPosition))
            {
                End end = new End();
                if (XPosition == Program.mygame.player.XPosition && YPosition == Program.mygame.player.YPosition)
                {
                    end.GameOver(false);
                }
                else
                {
                    end.GameOver(true);
                }
            }
        }

        public override void Action2()
        {
        }

        public override void Action3()
        {
        }
    }
}
