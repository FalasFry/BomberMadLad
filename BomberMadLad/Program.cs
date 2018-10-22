﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Timers;


namespace BomberMadLad
{
    class Program
    {
        //skapa game utefter storleken på skärmen
        public static Game mygame = new Game(Console.LargestWindowWidth - 13, Console.LargestWindowHeight - 12);

        //om vi ska använda AI
        public static bool haveAI;

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
                for (int i = 0; i < TimerClass.TimeList.Count; i++)
                {
                    TimerClass.TimeMethod(TimerClass.intList[i][0], TimerClass.intList[i][1], TimerClass.intList[i][2], TimerClass.intList[i][3], TimerClass.intList[i][4], TimerClass.intList[i][5], TimerClass.TimeList[i]);
                }

                stopwatch.Stop();

                TimerClass.elapsedTime = (int)stopwatch.ElapsedMilliseconds;
                mygame.UpdateBoard();

            }
        }
    }

    class Game
    {
        //allt som har update
        public List<GameObject> GameObjects = new List<GameObject>();

        //väggar
        public List<GameObject> Walls = new List<GameObject>();

        //skapa ny spelare
        public Player player = new Player();

        public int x { get; set; }
        public int y { get; set; }

        //skapar game med måtten vi skickade in i Program
        public Game(int xSize, int ySize)
        {
            y = ySize;
            x = xSize;

            //sålänge i <= så många rutor vi behöver i yLed (yZize + 1)
            for (int i = 0; i <= y + 1; i++)
            {
                //sålänge J <= antal rutor i xLed (XSize + 1)
                for (int j = 0; j <= x + 1; j++)
                {
                    //om j eller i är största eller minsta möjliga tal
                    if (j == 0 || i == 0 || i == y + 1 || j == x + 1)
                    {
                        //lägg till vägg i den positionen
                        Walls.Add(new Wall(j, i, false));
                    }
                    //räkna ut koordinaterna för mönster. (OBS RÖR INGET DET FUNKAR)
                    if (i <= ySize / 2 && j <= x / 4)
                    {
                        Walls.Add(new Wall(j * 4, i * 2, false));
                    }

                }
            }

            TimerClass.AddTimer(0, 3000, 1000, 1, 0, BrTimer);

            GameObjects.Add(player);
        }

        int index = 1;
        int maxIndex = 23;
        public void BrTimer()
        {
            if(index < maxIndex)
            {
                index++;
            }

            TimerClass.AddTimer(0, 300, 1000, 1, 0, BrTimer);
            TimerClass.AddTimer(0, 300, 1000, 1, 0, Br);
        }
        public void Br()
        {
            for (int i = 0; i < index; i++)
            {
                for (int k = 0; k <= y + 1; k++)
                {
                    //såänge J <= antal rutor i xLed (XSize + 1)
                    for (int j = 0; j <= x + 1; j++)
                    {
                        //om j eller i är största eller minsta möjliga tal
                        if (j == 0 || k == 0 || k == y + 1 || j == x + 1)
                        {
                            Wall wall = null;

                            if (x / 2 < j)
                            {
                                wall = (new Wall(j - i * 2, k, false));
                                wall.Draw(0, 0);
                                Walls.Add(wall);
                            }
                            if (x / 2 > j)
                            {
                                wall = (new Wall(j + i * 2, k, false));
                                wall.Draw(0, 0);
                                Walls.Add(wall);
                            }
                            if (y / 2 < k)
                            {
                                wall = (new Wall(j, k - i, false));
                                wall.Draw(0, 0);
                                Walls.Add(wall);
                            }
                            if (y / 2 > k)
                            {
                                wall = (new Wall(j, k + i, false));
                                wall.Draw(0, 0);
                                Walls.Add(wall);
                            }
                        }
                    }
                }
            }
        }

        public void DrawBoard()
        {
            foreach (GameObject gameObject in Walls)
            {
                gameObject.Draw(1, 1);
            }
            if (Program.haveAI == true)
            {
                GameObjects.Add(new AI());
            }
        }
        //rita ut gameobjects
        public void DrawStuff()
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Draw(1, 1);
            }
        }
        //uppdatera alla objekt
        public void UpdateBoard()
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update();
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
            int startX = XPosition * xBoxSize;
            int startY = YPosition * yBoxSize;
            if (!CanBlow) Console.ForegroundColor = ConsoleColor.DarkGray;
            else Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("██");
        }

        public override void Update()
        {
        }

        public override void Blow()
        {
            throw new NotImplementedException();
        }

        public override void RemoveBlow()
        {
            throw new NotImplementedException();
        }
    }

    class Player : GameObject
    {

        int xPos = 10;
        int yPos = 10;

        int u = 0;

        bool layBomb;

        //senaste bomben som spawnats
        public Bombs latestBoom;

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

            if (u == 0)
            {
                TimerClass.GetIndex(xPos, yPos);
            }
            u++;
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
                latestBoom = new Bombs(xPos, yPos);

                //lägg till i gameobjects
                Program.mygame.GameObjects.Add(latestBoom);

                
                layBomb = false;

                //lägg till timer

                int index = TimerClass.GetIndex(xPos, yPos);

                TimerClass.AddTimer(index, 1000, 0, 1, 0, Program.mygame.GameObjects[index].Blow);



                index = TimerClass.GetIndex(latestBoom.XPosition, latestBoom.YPosition);

                TimerClass.AddTimer(index, 1000, 500, 10, 0, Program.mygame.GameObjects[index].Blow);
            }
        }

        public void PlayerBoomCooldown(object o)
        {
            layBomb = true;
        }

        public override void Blow()
        {
            layBomb = true;
        }

        public override void RemoveBlow()
        {
        }
    }

    class AI : GameObject
    {
        public Random rng = new Random();
        int xPos;
        int yPos;

        public AI()
        {
            xPos = Console.LargestWindowWidth - 22;
            yPos = 10;
        }

        public override void Blow()
        {
            throw new NotImplementedException();
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("██");
        }

        public override void RemoveBlow()
        {
            throw new NotImplementedException();
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

    class PoweupsSpawn : GameObject
    {
        Player player;
        Game game;
        Random rng = new Random();
        //TimerClass timer;

        int Xpos;
        int Ypos;
        bool wait = true;
        public int PowerNumber { get; set; }


        public PoweupsSpawn()
        {
            game = Program.mygame;
            player = game.player;
            //timer = new TimerClass();
            Xpos = rng.Next(0, 113 / 2) * 2;
            Ypos = rng.Next(0, 51);
            PowerNumber = rng.Next(1, 4);
        }
        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(Xpos, Ypos);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("██");
        }

        public override void Update()
        {
            while (wait)
            {
                //timer.PowerupCooldown();
                wait = false;
            }

            if (PowerNumber == 1)
            {
                Draw(0, 0);
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            if (PowerNumber == 2)
            {
                Draw(0, 0);
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (rng.Next(1, 4) == 3)
            {
                Draw(0, 0);
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
        }

        public override void Blow()
        {
            throw new NotImplementedException();
        }

        public override void RemoveBlow()
        {
            throw new NotImplementedException();
        }
    }
}
