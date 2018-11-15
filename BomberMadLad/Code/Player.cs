using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Player : GameObject
    {
        // Hämtar kontroller.
        Move control = new Move();

        public Player(int x, int y)
        {
            XPosition = x;
            YPosition = y;
        }

        //rita ut cyan spelare
        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("██");
        }

        public override void Update()
        {
            // Återställ oldX +old Y
            int oldX = XPosition;
            int oldY = YPosition;
            ConsoleKey input = new ConsoleKey();

            //kollar efter spelarens input. OBS måste bytas ut för att inte bli turnbased eftersom readkey väntar på knapptryck.
            if (Console.KeyAvailable)
            {
                input = Console.ReadKey(true).Key;
            }

            //movement beroende på knapptryck, xpos är två steg i taget eftersom den är två pixlar bred
            if (input == ConsoleKey.W)
            {
                control.Up(this);
            }
            if (input == ConsoleKey.S)
            {
                control.Down(this);
            }
            if (input == ConsoleKey.D)
            {
                control.Right(this);
            }
            if (input == ConsoleKey.A)
            {
                control.Left(this);
            }
            //om collisionCheck träffar något så står vi stilla och deletar inte något
            if (!CollisionCheck(XPosition, YPosition))
            {
                XPosition = oldX;
                YPosition = oldY;
            }
            else
            {
                Delete(oldX, oldY);
                Draw(0, 0);
            }

            // Lägg bomb
            if (input == ConsoleKey.Spacebar)
            {
                List<int> position = new List<int> { XPosition, YPosition };

                Program.mygame.ai.bombPoints.Add(position);

                control.LayBomb(XPosition, YPosition);
            }
        }

        public override void Action2()
        {
        }
        public override void Action3()
        {
        }
        public override void Action1()
        {
        }
    }
}
