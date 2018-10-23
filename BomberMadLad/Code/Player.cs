using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Player : GameObject
    {
        int xPos = 10;
        int yPos = 10;

        bool u = false;

        bool layBomb;

        //senaste bomben som spawnats
        public BOOM latestBoom;

        public Player()
        {
            layBomb = true;
        }

        //rita ut cyan spelare
        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(xPos, yPos);
            Console.Write("██");

            if (!u)
            {
                TimerClass.GetIndex(xPos, yPos);
                u = true;
            }
            
        }

        public override void Update()
        {
            //återställ oldX + old Y
            int oldX = xPos;
            int oldY = yPos;
            ConsoleKey input = ConsoleKey.B;

            //kollar efter spelarens input. OBS måste bytas ut för att inte bli turnbased eftersom readkey väntar på knapptryck.
            if (Console.KeyAvailable)
            {
                input = Console.ReadKey(true).Key;
            }

            //movement beroende på knapptryck, xpos är två steg i taget eftersom den är två pixlar bred
            if (input == ConsoleKey.W)
            {
                yPos--;
            }
            if (input == ConsoleKey.S)
            {
                yPos++;
            }
            if (input == ConsoleKey.D)
            {
                xPos += 2;
            }
            if (input == ConsoleKey.A)
            {
                xPos -= 2;
            }
            //om collisionCheck träffar något så står vi stilla och deletar inte något
            if (!CollisionCheck(xPos, yPos))
            {
                xPos = oldX;
                yPos = oldY;
            }
            else Delete(oldX, oldY);
            Draw(0, 0);

            //lägg bomb
            if (input == ConsoleKey.Spacebar && layBomb)
            {
                latestBoom = new BOOM(xPos, yPos);

                //lägg till i gameobjects
                Program.mygame.GameObjects.Add(latestBoom);


                layBomb = false;

                //lägg till timer
                

                int index = TimerClass.GetIndex(latestBoom.XPosition, latestBoom.YPosition);

                TimerClass.AddTimer(index, 1000, 500, 1, 0, Program.mygame.GameObjects[index].Action1);
            }

            if (input == ConsoleKey.Escape)
            {
                Program.mygame.Pause();
            }
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
