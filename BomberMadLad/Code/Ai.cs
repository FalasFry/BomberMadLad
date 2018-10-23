using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Ai : GameObject
    {
        public Random rng = new Random();
        int xPos;
        int yPos;

        public Ai()
        {
            xPos = Console.LargestWindowWidth - 22;
            yPos = 10;
        }

        public override void Action1()
        {
            throw new NotImplementedException();
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("██");
        }

        public override void Action2()
        {
            throw new NotImplementedException();
        }
        public override void Action3()
        {
        }

        public override void Update()
        {

            int oldX = xPos;
            int oldY = yPos;
            int dir = rng.Next(1, 5);
            bool moved = false;


            //en loop som ser till att han inte går in i väggar, principen är att om han går in i vägg får han gå igen tills han lyckats gå åt rätt håll
            while (!moved)
            {
                xPos = oldX;
                yPos = oldY;

                dir = rng.Next(1, 5);

                if (dir == 1)
                {
                    yPos--;
                }
                if (dir == 2)
                {
                    yPos++;
                }
                if (dir == 3)
                {
                    xPos += 2;
                }
                if (dir == 4)
                {
                    xPos -= 2;
                }
                if (!CollisionCheck(xPos, yPos)) moved = false;
                else moved = true;

            }
            //efter loopen tar vi bort gamla spelaren
            Delete(oldX, oldY);
        }
    }


}
