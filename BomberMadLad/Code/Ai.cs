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
        public BOOM latestBoom;
        public bool layBomb = true;

        public List<List<int>> bombPoints = new List<List<int>>();


        public Ai(int xpoz, int ypoz)
        {
            XPosition = xpoz;
            YPosition = ypoz;
        }

        public override void Action1()
        {
            latestBoom = new BOOM(XPosition, YPosition);

            //lägg till i gameobjects
            Program.mygame.GameObjects.Add(latestBoom);

            //lägg till timer
            int index = TimerClass.GetIndex(latestBoom.XPosition, latestBoom.YPosition);

            TimerClass.AddTimer(index, 1000, 500, 10, Program.mygame.GameObjects[index].Action2);

            layBomb = true;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(XPosition, YPosition);
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

            int oldX = XPosition;
            int oldY = YPosition;
            int dir = rng.Next(1, 5);

            //en loop som ser till att han inte går in i väggar, principen är att om han går in i vägg får han gå igen tills han lyckats gå åt rätt håll
            XPosition = oldX;
            YPosition = oldY;

            dir = rng.Next(1, 6);

            if (dir == 1)
            {
                YPosition--;
            }
            if (dir == 2)
            {
                YPosition++;
            }
            if (dir == 3)
            {
                XPosition += 2;
            }
            if (dir == 4)
            {
                XPosition -= 2;
            }
            if(dir == 5 && layBomb)
            {
                List<int> position = new List<int> { XPosition, YPosition };
                bombPoints.Add(position);
                
                TimerClass.AddTimer(0, 5000, 0, 1, Action1);
                layBomb = false;
            }
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
        }
    }
}
