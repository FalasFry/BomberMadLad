using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Player : GameObject
    {
        bool canBomb = true;

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
                Move.Up(this);
                Delete(oldX, oldY);
            }
            if (input == ConsoleKey.S)
            {
                Move.Down(this);
                Delete(oldX, oldY);
            }
            if (input == ConsoleKey.D)
            {
                Move.Right(this);
                Delete(oldX, oldY);
            }
            if (input == ConsoleKey.A)
            {
                Move.Left(this);
                Delete(oldX, oldY);
            }

            //om collisionCheck träffar något så står vi stilla och deletar inte något
            if (!Collision.Wall(XPosition, YPosition))
            {
                XPosition = oldX;
                YPosition = oldY;
            }
            else
            {
                Draw(0, 0);
            }

            // Lägg bomb
            if (input == ConsoleKey.Spacebar && canBomb)
            {
                List<int> position = new List<int> { XPosition, YPosition };

                Move.LayBomb(XPosition, YPosition);

                canBomb = false;

                //om sex sekunder kan spelaren bomba igen
                int index = TimerClass.GetCharIndex(XPosition, YPosition);

                TimerClass.AddTimer(index, 6000, 6000, 1, Program.mygame.Characters[index].Action2);
            }
        }
        //spelaren kan bomba igen
        public override void Action2()
        {
            canBomb = true;
        }
        public override void Action3()
        {
        }
        public override void Action1()
        {
        }
    }
}
