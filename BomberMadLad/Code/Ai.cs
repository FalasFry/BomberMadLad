using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Ai : GameObject
    {
        Random rng = new Random();
        Move control = new Move();
        bool Bomb;

        public Ai()
        {
            XPosition = Console.LargestWindowWidth - 22;
            YPosition = 11;
            Bomb = true;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("██");
        }

        public override void Update()
        {

            int oldX = XPosition;
            int oldY = YPosition;
            int dir;

            //en loop som ser till att han inte går in i väggar, principen är att om han går in i vägg får han gå igen tills han lyckats gå åt rätt håll
            XPosition = oldX;
            YPosition = oldY;

            dir = rng.Next(1, 6);

            if (dir == 1)
            {
                control.Up(this);
            }
            if (dir == 2)
            {
                control.Down(this);
            }
            if (dir == 3)
            {
                control.Left(this);
            }
            if (dir == 4)
            {
                control.Right(this);
            }
            if (!CollisionCheck(XPosition, YPosition))
            {
                XPosition = oldX;
                YPosition = oldY;

                if (Bomb)
                {
                    TimerClass.AddTimer(0, 1, 0, 1, Action1);
                    Bomb = false;
                }
            }
            else
            {
                Delete(oldX, oldY);
                Draw(0, 0);
            }
        }

        public override void Action2()
        {
            Bomb = true;
        }
        public override void Action3()
        {
        }
        public override void Action1()
        {
            TimerClass.AddTimer(0, 5000, 0, 1, Action2);
            control.LayBomb(XPosition, YPosition);
        }
    }
}
