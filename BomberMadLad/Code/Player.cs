using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Player : GameObject
    {
        bool u = false;

        bool layBomb;
        PlayerControl control = new PlayerControl();
        //senaste bomben som spawnats
        public BOOM latestBoom;
        public Player(int x, int y)
        {
            layBomb = true;
            XPosition = x;
            YPosition = y;
        }

        //rita ut cyan spelare
        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("██");

            if (!u)
            {
                TimerClass.GetIndex(XPosition, YPosition);
                u = true;
            }
            
        }

        public override void Update()
        {
            //återställ oldX + old Y
            int oldX = XPosition;
            int oldY = YPosition;
            //ConsoleKey input = new ConsoleKey();

            ////kollar efter spelarens input. OBS måste bytas ut för att inte bli turnbased eftersom readkey väntar på knapptryck.
            //if (Console.KeyAvailable)
            //{
            //    input = Console.ReadKey(true).Key;
            //}
            //control.Update();

            ////movement beroende på knapptryck, xpos är två steg i taget eftersom den är två pixlar bred
            //if (input == ConsoleKey.W)
            //{
            //    YPosition--;
            //}
            //if (input == ConsoleKey.S)
            //{
            //    YPosition++;
            //}
            //if (input == ConsoleKey.D)
            //{
            //    XPosition += 2;
            //}
            //if (input == ConsoleKey.A)
            //{
            //    XPosition -= 2;
            //}
            ////om collisionCheck träffar något så står vi stilla och deletar inte något
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

            //lägg bomb
            //if (input == ConsoleKey.Spacebar && layBomb)
            //{
            //    latestBoom = new BOOM(XPosition, YPosition);

            //    //lägg till i gameobjects
            //    Program.mygame.GameObjects.Add(latestBoom);


            //    layBomb = true;

            //    //lägg till timer
            //    int index = TimerClass.GetIndex(latestBoom.XPosition, latestBoom.YPosition);

            //    TimerClass.AddTimer(index, 1000, 500, 10, Program.mygame.GameObjects[index].Action2);
            //}
        }

        public void PlayerBoomCooldown(object o)
        {
            layBomb = true;
        }

        public override void Action1()
        {
            layBomb = true;
        }
        public override void Action2()
        {
        }
        public override void Action3()
        {
        }
    }
}
