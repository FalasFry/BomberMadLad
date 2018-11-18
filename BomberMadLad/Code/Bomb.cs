using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Bomb : GameObject
    {
        //listor för alla ríktningar
        List<List<int>> upList = new List<List<int>>();
        List<List<int>> downList = new List<List<int>>();
        List<List<int>> rightslist = new List<List<int>>();
        List<List<int>> leftList = new List<List<int>>();

        //håller alla explosioner som skapats
        List<GameObject> ExList = new List<GameObject>();

        //index
        int index;

        //hur många gånger bomben har blinkat
        int timesHaveBlinked = 0;

        //kollar om bomben sprängts
        bool didBlow = false;

        //hur många gånger bomben ska blinka
        int timesToBlink = 10;
        bool colorSwitch = true;

        //sätt index, Xposition och Yposition
        public Bomb(int playerPosX, int playerPosY)
        {
            index = Program.mygame.GameObjects.Count;
            XPosition = playerPosX;
            YPosition = playerPosY;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            //byt färg mellan röd och gul
            if (colorSwitch)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("██");
        }

        public override void Update()
        {

        }

        //spräng
        public override void Action1()
        {
            int index = Collision.GetIndex(XPosition, YPosition);

            CrossBomb(XPosition, YPosition);

        }

        public void CrossBomb(int oldX, int oldY)
        {
            //håller position för explosion, börjar med bombens position
            List<int> position = new List<int> { XPosition, YPosition };

            //mult kollar hur långt bort från bomben explosionen ska hamna
            int Mult = 0;

            //vi jobbar med x/2
            oldX = oldX / 2;

            //bools för alla sidor för att hålla reda på vilka som fortfarande används
            bool right = true;
            bool left = true;
            bool up = true;
            bool down = true;

            //lägg till explosion ovanpå bomben
            upList.Add(position);

            while (true)
            {
                //mult blir en större varje gång

                Mult++;

                //börjar med höger
                if (right)
                {
                    //om vi inte träffar något så lägger vi bara till i listan
                    if (Collision.Wall((oldX + Mult) * 2, oldY))
                    {
                        position = new List<int> { (oldX + Mult) * 2, oldY };
                        rightslist.Add(position);
                    }
                    //om vi träffar en vägg och väggen kan sprängas så sätter vi ut en explosion där och tar bort bomben
                    //sen sätter vi right till false så att bomben slutar spränga i den riktningen
                    else
                    {
                        if (Program.mygame.Walls[Collision.GetWallIndex((oldX + Mult) * 2, oldY)].CanBlow)
                        {
                            position = new List<int> { (oldX + Mult) * 2, oldY };
                            Program.mygame.Walls[0].Destroy(Collision.GetWallIndex((oldX + Mult) * 2, oldY), true);
                            rightslist.Add(position);
                        }
                        right = false;
                    }
                }
                //samma sak fast vänster
                if (left)
                {
                    if (Collision.Wall((oldX - Mult) * 2, oldY))
                    {
                        position = new List<int> { (oldX - Mult) * 2, oldY };
                        leftList.Add(position);
                    }
                    else
                    {
                        if (Program.mygame.Walls[Collision.GetWallIndex((oldX - Mult) * 2, oldY)].CanBlow)
                        {
                            position = new List<int> { (oldX - Mult) * 2, oldY };
                            Program.mygame.Walls[0].Destroy(Collision.GetWallIndex((oldX - Mult) * 2, oldY), true);
                            leftList.Add(position);
                        }
                        left = false;
                    }

                }
                //samma sak fast upp
                if (up)
                {
                    if (Collision.Wall(oldX * 2, oldY + Mult))
                    {
                        position = new List<int> { oldX * 2, oldY + Mult };
                        upList.Add(position);
                    }
                    else
                    {
                        if (Program.mygame.Walls[Collision.GetWallIndex(oldX * 2, oldY + Mult)].CanBlow)
                        {
                            position = new List<int> { oldX * 2, oldY + Mult };
                            Program.mygame.Walls[0].Destroy(Collision.GetWallIndex(oldX * 2, oldY + Mult), true);
                            upList.Add(position);
                        }
                        up = false;
                    }

                }
                //samma sak fast ner
                if (down)
                {
                    if (Collision.Wall(oldX * 2, oldY - Mult))
                    {
                        position = new List<int> { oldX * 2, oldY - Mult };
                        downList.Add(position);
                    }
                    else
                    {

                        if (Program.mygame.Walls[Collision.GetWallIndex(oldX * 2, oldY - Mult)].CanBlow)
                        {
                            position = new List<int> { oldX * 2, oldY - Mult };
                            Program.mygame.Walls[0].Destroy(Collision.GetWallIndex(oldX * 2, oldY - Mult), true);
                            downList.Add(position);
                        }
                        down = false;
                    }

                }
                //om alla riktningar är klara slutar vi skapa explosioner
                if (!down && !up && !left && !right)
                {
                    break;
                }

                //sätt ut timer för att ta bort explosionerna
                index = Collision.GetIndex(XPosition, YPosition);

                TimerClass.AddTimer(index, 500, 500, 1, Program.mygame.GameObjects[index].Action3);

            }
        }

        //går igenom alla listor och ritar ut dem
        public void DrawExplosions()
        {
            List<List<int>> totalList = new List<List<int>>();
            for (int i = 0; i < downList.Count; i++)
            {
                totalList.Add(downList[i]);
            }
            for (int i = 0; i < upList.Count; i++)
            {
                totalList.Add(upList[i]);
            }
            for (int i = 0; i < rightslist.Count; i++)
            {
                totalList.Add(rightslist[i]);
            }
            for (int i = 0; i < leftList.Count; i++)
            {
                totalList.Add(leftList[i]);
            }
            for (int i = 0; i < totalList.Count; i++)
            {
                Explosions explo = new Explosions();
                explo.XPosition = totalList[i][0];
                explo.YPosition = totalList[i][1];
                explo.Draw(0, 0);
                ExList.Add(explo);
            }
        }
        //blinkar tills den blinkat tillräckligt och då ritar den ut explosionerna
        public override void Action2()
        {
            if (timesHaveBlinked < timesToBlink - 1)
            {
                colorSwitch = !colorSwitch;
                timesHaveBlinked++;
            }
            else if (!didBlow)
            {
                Action1();
                DrawExplosions();
                didBlow = true;
            }
        }

        //ta bort alla explosioner och sedan bomben själv
        public override void Action3()
        {
            for (int i = 0; i < ExList.Count; i++)
            {
                ExList[i].Action1();
            }
            Program.mygame.GameObjects[0].Destroy(Collision.GetIndex(XPosition, YPosition), false);
        }
    }
}

