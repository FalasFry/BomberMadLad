using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Exposions : GameObject
    {
        public override void Blow()
        {
            throw new NotImplementedException();
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            throw new NotImplementedException();
        }

        public override void RemoveBlow()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }

    class Bombs : GameObject
    {
        //TimerClass timer;
        int index;
        int xPos;
        int yPos;
        int f = 0;
        int blinkTimes = 10;
        bool colorSwitch = true;


        public Bombs(int playerPosX, int playerPosY)
        {
            //timer = new TimerClass();
            index = Program.mygame.GameObjects.Count;
            xPos = playerPosX;
            yPos = playerPosY;

        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            if (colorSwitch)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.SetCursorPosition(xPos, yPos);
            Console.Write("██");
        }

        public override void Update()
        {

        }

        public void RemoveBoom()
        {
            int index = TimerClass.GetIndex(xPos, yPos);
            if (index != 0)
            {
                Program.mygame.GameObjects[index].Destroy(index, false);
            }
            CrossBomb(xPos, yPos);

        }
        public void BOOOOM()
        {
            CrossBomb(xPos, yPos);
        }
        public void CrossBomb(int oldX, int oldY)
        {
            List<GameObject> ExList = new List<GameObject>();
            List<int> ExIntList = new List<int>();
            List<int> remaining = new List<int> { 1, 2, 3, 4 };

            int Mult = 0;

            oldX = oldX / 2;

            ExIntList.Add(oldX);
            ExIntList.Add(oldX);

            int Q = 1;
            while (true)
            {
                int remaingnum = 4;
                for (int i = 0; i < remaining.Count; i++)
                {
                    switch (remaining[i])
                    {
                        case 0:
                            break;
                        case 1:

                            if (Mult < 0) Mult *= -1;

                            if (CollisionCheck((oldX + Mult) * 2, oldY))
                            {
                                ExIntList.Add((oldX + Mult) * 2);
                                ExIntList.Add(oldY);
                            }
                            else
                            {
                                remaining[i] = 0;
                            }

                            break;


                        case 2:

                            if (Mult < 0) Mult *= -1;

                            if (CollisionCheck((oldX) * 2, oldY + Mult))
                            {
                                ExIntList.Add((oldX) * 2);
                                ExIntList.Add(oldY + Mult);
                            }
                            else
                            {
                                remaining[i] = 0;
                            }

                            break;

                        case 3:

                            if (Mult > 0) Mult *= -1;

                            if (CollisionCheck((oldX + Mult) * 2, oldY))
                            {
                                ExIntList.Add((oldX + Mult) * 2);
                                ExIntList.Add(oldY);
                            }
                            else
                            {
                                remaining[i] = 0;
                            }
                            break;

                        case 4:

                            if (Mult > 0) Mult *= -1;

                            if (CollisionCheck((oldX) * 2, oldY + Mult))
                            {
                                ExIntList.Add((oldX) * 2);
                                ExIntList.Add(oldY + Mult);
                            }
                            else
                            {
                                remaining[i] = 0;
                            }
                            break;
                    }
                    if (remaining[i] == 0)
                    {
                        remaingnum--;
                    }
                }

                if (remaingnum == 0)
                {
                    break;
                }
            }

            for (int i = 0; i < ExIntList.Count; i += 2)
            {
                Exposions explo = new Exposions();
                explo.XPosition = ExIntList[i];
                explo.YPosition = ExIntList[i + 1];
                ExList.Add(new Exposions());
            }
        }
        public override void Blow()
        {
            if (f < blinkTimes - 1)
            {
                colorSwitch = !colorSwitch;
                f++;
            }
            else
            {
                RemoveBoom();


                int index = TimerClass.GetIndex(XPosition, YPosition);

                TimerClass.TimeList.Add(Program.mygame.GameObjects[index].RemoveBlow);

                int[] list = { 1000, 1000, 1, index, 0, 0 };

                TimerClass.intList.Add(list);
            }
        }

        public override void RemoveBlow()
        {
            BOOOOM();
        }
    }
}
