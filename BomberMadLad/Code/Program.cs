using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using System.Windows;


namespace BomberMadLad
{
    class Program
    {
        //skapa game utefter storleken på skärmen
        public static Game mygame = new Game(Console.LargestWindowWidth - 13, Console.LargestWindowHeight - 12);

        //om vi ska använda AI
        public static bool HaveAi { get; set; }

        //metod som startas när spelet gör det
        static void Main(string[] args)
        {
            //sätter musen till osynlig
            //frågar om AI
            Console.CursorVisible = false;
            Menu mainMenu = new Menu();
            mainMenu.MainMenu();

            //ändra storlek på konsolfönstret till största möjliga
            Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight - 9);

            //kalla på drawboard metoden för att rita ut alla saker i walls listan. den körs bara en gång eftersom väggarna inte behöver uppdateras
            mygame.DrawBoard();

            Stopwatch stopwatch = new Stopwatch();

            //denna metoden körs hela tiden
            while (true)
            {
                stopwatch.Start();
                //ritar ut alla gameobjects i listan GameObjects
                mygame.DrawStuff();

                //kallar på update i alla GameObjects
                mygame.UpdateBoard();

                TimerClass.TimeMethod();

                stopwatch.Stop();

                TimerClass.elapsedTime = (int)stopwatch.ElapsedMilliseconds;
            }
        }
    }

    class Game
    {
        //allt som har update
        public List<GameObject> GameObjects = new List<GameObject>();

        // Lista för spelare.
        public List<GameObject> Characters = new List<GameObject>();

        //väggar
        public List<GameObject> Walls = new List<GameObject>();

        //skapa ny spelare
        public Player player = new Player(10,11);

        public Ai ai = new Ai(Console.LargestWindowWidth - 22, 11);

        public int X { get; set; }
        public int Y { get; set; }

        //skapar game med måtten vi skickade in i Program
        public Game(int xSize, int ySize)
        {
            Y = ySize;
            X = xSize;

            //sålänge i <= så många rutor vi behöver i yLed (yZize + 1)
            for (int i = 0; i <= Y + 1; i++)
            {
                //sålänge J <= antal rutor i xLed (XSize + 1)
                for (int j = 0; j <= X + 1; j+=2)
                {
                    //om j eller i är största eller minsta möjliga tal
                    if (j == 0 || i == 0 || i == Y + 1 || j == X + 1)
                    {
                        //lägg till vägg i den positionen
                        Walls.Add(new Wall(j, i, false));
                    }
                    else if((j + 2) % 4 == 0 || (i + 1) % 2 == 0)
                    {
                        Walls.Add(new Wall(j, i, true));
                    }

                    //räkna ut koordinaterna för mönster. (OBS RÖR INGET DET FUNKAR)
                    if (i <= ySize / 2 && j <= X / 2)
                    {
                        Walls.Add(new Wall(j * 2, i * 2, false));
                    }
                }
            }
            Characters.Add(player);
        }

        int BrIndex = 0;
        int maxIndex = 20;

        public void Br()
        {
            BrIndex++;
            for (int k = 0; k <= Y + 1; k++)
            {
                for (int j = 0; j <= X + 1; j++)
                {
                    Wall wall = null;
                    if (j == (X+1) - BrIndex)
                    {
                        wall = (new Wall(j - BrIndex, k, false));
                        wall.Draw(0, 0);
                        Walls.Add(wall);
                    }

                    if (j == 0 + BrIndex)
                    {
                        wall = (new Wall(j + BrIndex, k, false));
                        wall.Draw(0, 0);
                        Walls.Add(wall);
                    }

                    if (k == (Y+1) - BrIndex)
                    {
                        wall = (new Wall(j, k, false));
                        wall.Draw(0, 0);
                        Walls.Add(wall);
                    }

                    if (k == 0 + BrIndex)
                    {
                        wall = (new Wall(j, k, false));
                        wall.Draw(0, 0);
                        Walls.Add(wall);
                    }
                }
            }
        }
        
        
        public void DrawBoard()
        {
            //TimerClass.AddTimer(0, 10000, 10000, maxIndex, Br);
            int wallsIndex = 1;

            if (Program.HaveAi == true)
            {
                Characters.Add(ai);
            }
            

            for (int i = 0; i < Characters.Count; i++)
            {
                wallsIndex = TimerClass.GetWallIndex(Characters[i].XPosition, Characters[i].YPosition);
                Walls[wallsIndex].Destroy(wallsIndex, true);
                wallsIndex = TimerClass.GetWallIndex(Characters[i].XPosition - 2, Characters[i].YPosition);
                Walls[wallsIndex].Destroy(wallsIndex, true);
                wallsIndex = TimerClass.GetWallIndex(Characters[i].XPosition + 2, Characters[i].YPosition);
                Walls[wallsIndex].Destroy(wallsIndex, true);
                wallsIndex = TimerClass.GetWallIndex(Characters[i].XPosition, Characters[i].YPosition - 1);
                Walls[wallsIndex].Destroy(wallsIndex, true);
                wallsIndex = TimerClass.GetWallIndex(Characters[i].XPosition, Characters[i].YPosition + 1);
                Walls[wallsIndex].Destroy(wallsIndex, true);

            }
            foreach (GameObject gameObject in Walls)
            {
                gameObject.Draw(1, 1);
            }
        }

        // Rita ut gameobjects.
        public void DrawStuff()
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Draw(1, 1);
            }
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].Draw(1, 1);
            }
        }

        // Uppdatera alla objekt.
        public void UpdateBoard()
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update();
            }
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].Update();
            }
        }
    }
    
    class Wall : GameObject
    {
        public Wall(int xPosition, int yPosition, bool Destroyable)
        {
            CanBlow = Destroyable;
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            if (!CanBlow) Console.ForegroundColor = ConsoleColor.DarkGray;
            else Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("██");
        }

        public override void Update()
        {
        }

        public override void Action1()
        {
            throw new NotImplementedException();
        }

        public override void Action2()
        {
            throw new NotImplementedException();
        }

        public override void Action3()
        {
            throw new NotImplementedException();
        }
    }
    
    class Exposions : GameObject
    {
        
        public override void Action1()
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("  ");

            if(!CollisionCheckChar(XPosition, YPosition))
            {
                End end = new End();
                end.GameOver();
            }
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("██");
        }

        public override void Action2()
        {
        }

        public override void Update()
        {
        }
        public override void Action3()
        {
        }
    }

    class BOOM : GameObject
    {
        List<List<int>> upList = new List<List<int>>();
        List<List<int>> downList = new List<List<int>>();
        List<List<int>> rightslist = new List<List<int>>();
        List<List<int>> leftList = new List<List<int>>();
        List<GameObject> ExList = new List<GameObject>();

        public Move control = new Move();


        int index;
        int f = 0;
        bool didBlow = false;
        int blinkTimes = 5;
        bool colorSwitch = true;


        public BOOM(int playerPosX, int playerPosY)
        {
            index = Program.mygame.GameObjects.Count;
            XPosition = playerPosX;
            YPosition = playerPosY;
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

            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("██");
        }

        public override void Update()
        {

        }

        public override void Action1()
        {
            int index = TimerClass.GetIndex(XPosition, YPosition);

            CrossBomb(XPosition, YPosition);

        }
        public void CrossBomb(int oldX, int oldY)
        {

            List<int> ExIntList = new List<int>();
            List<int> remaining = new List<int> { 1, 2, 3, 4 };
            List<int> position = new List<int>();


            int Mult = 0;

            oldX = oldX / 2;

            bool right = true;
            bool left = true;
            bool up = true;
            bool down = true;


            while (true)
            {
                Mult++;
                if (right)
                {
                    if (CollisionCheck((oldX + Mult) * 2, oldY))
                    {
                        position = new List<int> { (oldX + Mult) * 2, oldY };
                        rightslist.Add(position);
                    }
                    else
                    {
                        if (Program.mygame.Walls[TimerClass.GetWallIndex((oldX + Mult) * 2, oldY)].CanBlow)
                        {
                            position = new List<int> { (oldX + Mult) * 2, oldY };
                            Program.mygame.Walls[0].Destroy(TimerClass.GetWallIndex((oldX + Mult) * 2, oldY), true);
                            rightslist.Add(position);
                        }
                        right = false;
                    }
                }

                if (left)
                {
                    if (CollisionCheck((oldX - Mult) * 2, oldY))
                    {
                        position = new List<int> { (oldX - Mult) * 2, oldY };
                        leftList.Add(position);
                    }
                    else
                    {
                        if (Program.mygame.Walls[TimerClass.GetWallIndex((oldX - Mult) * 2, oldY)].CanBlow)
                        {
                            position = new List<int> { (oldX - Mult) * 2, oldY };
                            Program.mygame.Walls[0].Destroy(TimerClass.GetWallIndex((oldX - Mult) * 2, oldY), true);
                            leftList.Add(position);
                        }
                        left = false;
                    }
                    
                }

                if (up)
                {
                    if (CollisionCheck(oldX * 2, oldY + Mult))
                    {
                        position = new List<int> { oldX * 2, oldY + Mult };
                        upList.Add(position);
                    }
                    else
                    {
                        if (Program.mygame.Walls[TimerClass.GetWallIndex(oldX * 2, oldY + Mult)].CanBlow)
                        {
                            position = new List<int> { oldX * 2, oldY + Mult };
                            Program.mygame.Walls[0].Destroy(TimerClass.GetWallIndex(oldX * 2, oldY + Mult), true);
                            upList.Add(position);
                        }
                        up = false;
                    }
                    
                }

                if (down)
                {
                    if (CollisionCheck(oldX * 2, oldY - Mult))
                    {
                        position = new List<int> { oldX * 2, oldY - Mult };
                        downList.Add(position);
                    }
                    else
                    {

                        if (Program.mygame.Walls[TimerClass.GetWallIndex(oldX * 2, oldY - Mult)].CanBlow)
                        {
                            position = new List<int> { oldX * 2, oldY - Mult };
                            Program.mygame.Walls[0].Destroy(TimerClass.GetWallIndex(oldX * 2, oldY - Mult), true);
                            downList.Add(position);
                        }
                        down = false;
                    }
                    
                }

                if (!down && !up && !left && !right)
                {
                    break;
                }

                index = TimerClass.GetIndex(XPosition, YPosition);

                TimerClass.AddTimer(index, 500, 500, 1, Program.mygame.GameObjects[index].Action3);

            }
        }

        public void drawExplosions()
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
                Exposions explo = new Exposions();
                explo.XPosition = totalList[i][0];
                explo.YPosition = totalList[i][1];
                explo.Draw(0, 0);
                ExList.Add(explo);
            }
        }
        public override void Action2()
        {
            if (f < blinkTimes - 1)
            {
                colorSwitch = !colorSwitch;
                f++;
            }
            else if (!didBlow)
            {
                Action1();
                drawExplosions();
                didBlow = true;
            }
        }

        public override void Action3()
        {
            for (int i = 0; i < ExList.Count; i++)
            {
                ExList[i].Action1();
            }
            Program.mygame.GameObjects[0].Destroy(TimerClass.GetIndex(XPosition, YPosition), false);
        }
    }
}
