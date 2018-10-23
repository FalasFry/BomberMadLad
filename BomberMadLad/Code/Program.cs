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

            TimerClass.AddTimer(0, 3000, 1000, 1, 0, BrTimer);
            TimerClass.AddTimer(0, 3000, 1000, 1, 0, Br);
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
            if (Program.HaveAi == true)
            {
                GameObjects.Add(new Ai());
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
            Debug.WriteLine("erasing + " + XPosition);
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("  ");
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(XPosition, YPosition);
        Console.Write("██");
        }

        public override void Action2()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
        public override void Action3()
        {
            throw new NotImplementedException();
        }
    }

    class BOOM : GameObject
    {
        //TimerClass timer;
        int index;
        int xPos;
        int yPos;
        int f = 0;
        int blinkTimes = 5;
        bool colorSwitch = true;
        List<GameObject> ExList = new List<GameObject>();

        public BOOM(int playerPosX, int playerPosY)
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

        public override void Action1()
        {
            int index = TimerClass.GetIndex(xPos, yPos);
            if (index != 0)
            {
                Program.mygame.GameObjects[index].Destroy(index, false);
            }
            Debug.WriteLine("ddddd");
            CrossBomb(xPos, yPos);

        }
        public void CrossBomb(int oldX, int oldY)
        {
            
            List<int> ExIntList = new List<int>();
            List<int> remaining = new List<int> { 1, 2, 3, 4 };

            int Mult = 0;

            oldX = oldX / 2;

            bool right = true;
            bool left = true;
            bool up = true;
            bool down = true;

            ExIntList.Add(oldX);
            ExIntList.Add(oldX);
            
            while (true)
            {
                Mult++;

                if (CollisionCheck((oldX + Mult) * 2, oldY) && right)
                {
                    Exposions explo = new Exposions();
                    explo.XPosition = (oldX + Mult) * 2;
                    explo.YPosition = oldY;
                    explo.Draw(0, 0);
                    ExList.Add(explo);
                    
                }
                else
                {
                    right = false;
                }

                if (CollisionCheck((oldX - Mult) * 2, oldY) && left)
                {
                    Exposions explo = new Exposions();
                    explo.XPosition = (oldX - Mult) * 2;
                    explo.YPosition = oldY;
                    explo.Draw(0, 0);
                    ExList.Add(explo);
                }
                else
                {
                    left = false;
                }

                if (CollisionCheck(oldX * 2, oldY + Mult) && up)
                {
                    Exposions explo = new Exposions();
                    explo.XPosition = oldX * 2;
                    explo.YPosition = oldY + Mult;
                    explo.Draw(0, 0);
                    ExList.Add(explo);
                }
                else
                {
                    up = false;
                }

                if (CollisionCheck(oldX * 2, oldY - Mult) && down)
                {
                    Exposions explo = new Exposions();
                    explo.XPosition = oldX * 2;
                    explo.YPosition = oldY - Mult;
                    explo.Draw(0, 0);
                    ExList.Add(explo);
                }
                else
                {
                    down = false;
                }
                if (!down && !up && !left && !right)
                {
                    Debug.WriteLine("RATP");
                    break;
                }
            }
            for (int i = 0; i < ExList.Count; i++)
            {
                ExList[i].Action1();
            }

            Debug.WriteLine("end");
        }

        public override void Action2()
        {
            if (f < blinkTimes - 1)
            {
                colorSwitch = !colorSwitch;
                f++;
            }
            else
            {
                Debug.WriteLine("xxx");
                Action1();
            }
        }

        public override void Action3()
        {
            Debug.WriteLine("destroy");
            for (int i = 0; i < ExList.Count; i++)
            {
                Console.SetCursorPosition(ExList[i].XPosition, ExList[i].YPosition);
                Debug.WriteLine("x " + XPosition + "y " + YPosition);
                Console.Write("");
            }
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
        
        public void SpawnPowerup(object o)
        {
            wait = true;
        }

        public override void Action1()
        {

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

    class PowerUps : GameObject
    {
        public PowerUps()
        {
        }

        public override void Action1()
        {
            throw new NotImplementedException();
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {

        }

        public override void Action2()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            //Om du nuddar en Poerup och det är powerup 1

        }

        public override void Action3()
        {
            throw new NotImplementedException();
        }
    }
}
