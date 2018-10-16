using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Timers;


namespace GridGame
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
            Menu();

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

        #region Main Menu

        public static List<string> Buttons = new List<string>();
        public static ConsoleColor gray = ConsoleColor.Gray;
        public static ConsoleColor black = ConsoleColor.Black;

        public static void Menu()
        {
            Console.WriteLine("Press Enter To Continue!");
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    break;
                }
                else
                {
                }
            }
            Console.Clear();
            int index = 0;
            Buttons.Add("                                                          Start                                                         ");
            Buttons.Add("                                                          Quit                                                          ");
            Buttons.Add("                                                          Load                                                          ");
            Buttons.Add("                                                          Save                                                          ");
            Buttons.Add("                                                        HighScore                                                       ");

            MenuList(index);

            while (true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.DownArrow)
                {
                    if (index < Buttons.Count - 1)
                    {
                        MenuList(index + 1);
                        index = index + 1;
                    }
                }
                if (input == ConsoleKey.UpArrow)
                {
                    if (index > 0)
                    {
                        MenuList(index - 1);
                        index = index - 1;
                    }
                }

                if (index == 0)
                {
                    if (input == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
                if (index == 1)
                {
                    if (input == ConsoleKey.Enter)
                    {
                        Environment.Exit(0);
                    }
                }
            }

            BackColour(black);
            ForColour(gray);
            Console.Clear();
            Console.WriteLine(" Do you want AI? (Y/N)");
            while (true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.Y)
                {
                    haveAI = true;
                    Console.Clear();
                    break;
                }
                if (input == ConsoleKey.N)
                {
                    haveAI = false;
                    Console.Clear();
                    break;
                }
            }
        }

        public static void ForColour(ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
        }
        public static void BackColour(ConsoleColor consoleColor)
        {
            Console.BackgroundColor = consoleColor;
        }

        public static void MenuList(int index)
        {
            Console.Clear();
            for (int i = 0; i < Buttons.Count; i++)
            {
                if(i != index)
                {
                    ForColour(gray);
                    BackColour(black);
                    Console.WriteLine(Buttons[i]);
                }
                //index == 2
                else if (i == index)
                {
                    ForColour(black);
                    BackColour(gray);
                    Console.WriteLine(Buttons[index]);
                }
                Console.ResetColor();
            }

        }

        #endregion
    }

    

    class Game
    {
        //allt som har update
        public List<GameObject> GameObjects = new List<GameObject>();

        //väggar
        public List<GameObject> Walls = new List<GameObject>();

        public List<GameObject> Map = new List<GameObject>();

        //skapa ny spelare
        public Player player = new Player();


        //skapar game med måtten vi skickade in i Program
        public Game(int xSize, int ySize)
        {
            //sålänge i <= så många rutor vi behöver i yLed (yZize + 1)
            for (int i = 0; i <= ySize + 1; i++)
            {
                //sålänge J <= antal rutor i xLed (XSize + 1)
                for (int j = 0; j <= xSize + 1; j++)
                {
                    //om j eller i är största eller minsta möjliga tal
                    if (j == 0 || i == 0 || i == ySize + 1 || j == xSize + 1)
                    {
                        //lägg till vägg i den positionen
                        Walls.Add(new Wall(j, i, false));
                    }
                    //räkna ut koordinaterna för mönster. (OBS RÖR INGET DET FUNKAR)
                    if (i <= ySize / 2 && j <= xSize / 4)
                    {
                        Walls.Add(new Wall(j * 4, i * 2, false));
                    }
                }
            }

            Map.Add(new Map());

            GameObjects.Add(player);

        }

        public void DrawBoard()
        {
            //rita ut väggar
            foreach (GameObject gameObject in Map)
            {
                gameObject.Update();
            }
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

    abstract class GameObject
    {
        public bool CanBlow;
        public int XPosition;
        public int YPosition;
        public abstract void Draw(int xBoxSize, int yBoxSize);
        public abstract void Update();
        public abstract void Blow();
        public abstract void RemoveBlow();

        //ta bort en ruta på positionen som skickas in
        public void Delete(int oldX, int oldY)
        {
            Console.SetCursorPosition(oldX, oldY);
            Console.Write("  ");
        }
        public void Destroy(int index, bool walls)
        {
            if (walls)
            {
                if (index > Program.mygame.GameObjects.Count)
                {
                    Program.mygame.Walls.RemoveAt(index);
                }

            }
            else Program.mygame.GameObjects.RemoveAt(index);

            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("  ");


        }

        //kolla kollision på inskickade koordinater
        public bool CollisionCheck(int xPos, int yPos)
        {
            //för varje vägg som finns
            for (int i = 0; i < Program.mygame.Walls.Count; i++)
            {
                //om y positionen är samma
                if (Program.mygame.Walls[i].YPosition == yPos)
                {
                    //och x positionen och dens grannar är samma (vet ej varför men det funkar)
                    if (Program.mygame.Walls[i].XPosition == xPos - 1 || Program.mygame.Walls[i].XPosition == xPos || Program.mygame.Walls[i].XPosition == xPos + 1)
                    {
                        //WE GOT COLLISION
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CollisionCheckGameObj(int xPos, int yPos)
        {
            //för varje vägg som finns
            for (int i = 0; i < Program.mygame.GameObjects.Count; i++)
            {
                //om y positionen är samma
                if (Program.mygame.GameObjects[i].YPosition == yPos)
                {
                    //och x positionen och dens grannar är samma (vet ej varför men det funkar)
                    if (Program.mygame.GameObjects[i].XPosition == xPos - 1 || Program.mygame.GameObjects[i].XPosition == xPos || Program.mygame.GameObjects[i].XPosition == xPos + 1)
                    {
                        //WE GOT COLLISION
                        return false;
                    }
                }
            }
            return true;
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
            Console.SetCursorPosition(startX, startY);
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

    class Map : GameObject
    {

        Random rng = new Random();
        int xPos;
        int yPos;
        AI ai;

        public Map()
        {
            xPos = Console.LargestWindowWidth / 2;
            yPos = Console.LargestWindowHeight / 2;
        }

        public override void Blow()
        {
            throw new NotImplementedException();
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(xPos, yPos);
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
            List<int[]> posList = new List<int[]>();
            int maxMove = 5000;
            //en loop som ser till att han inte går in i väggar, principen är att om han går in i vägg får han gå igen tills han lyckats gå åt rätt håll
            for (int i = 0; i <= maxMove; i++)
            {
                moved = false;
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

                posList.Add(new int[2] { xPos, yPos });
                oldX = xPos;
                oldY = yPos;
                Draw(0, 0);
            }
            for (int i = 0; i < posList.Count; i++)
            {
                Program.mygame.Walls.Add(new Wall(posList[i][0], posList[i][1], true));
            }

        }
    }

    class Player : GameObject
    {

        int xPos = 10;
        int yPos = 10;

        int u = 0;

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

            //kollar efter spelarens input. OBS måste bytas ut för att inte bli turnbased eftersom readkey väntar på knapptryck

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
                Debug.WriteLine("x " + xPos + " y " + yPos);
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
    class BOOM : GameObject
    {
        //TimerClass timer;
        int index;
        int xPos;
        int yPos;
        int f = 0;
        int blinkTimes = 10;
        bool colorSwitch = true;


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
                Debug.WriteLine("booming");
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
            Debug.WriteLine("deathto guy");
            BOOOOM();
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

        public override void Blow()
        {
            throw new NotImplementedException();
        }

        public override void RemoveBlow()
        {
            throw new NotImplementedException();
        }
    }

    class PowerUps : GameObject
    {
        public PowerUps()
        {
        }

        public override void Blow()
        {
            throw new NotImplementedException();
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {

        }

        public override void RemoveBlow()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            //Om du nuddar en Poerup och det är powerup 1

        }
    }

    class TimerClass
    {
        public static List<Action> TimeList = new List<Action>();

        public static List<int[]> intList = new List<int[]>();

        public static void AddTimer(int index, int timetostart, int intervalTime, int timeAmount, int starttime, Action method)
        {

            TimeList.Add(method);

            int[] list = { timetostart, intervalTime, timeAmount, index, 0, starttime };

            intList.Add(list);
        }
    
            

        public static int GetIndex(int x, int y)
        {
            int index = 0;
            for (int i = 0; i < Program.mygame.GameObjects.Count; i++)
            {
                if (Program.mygame.GameObjects[i].XPosition == x && Program.mygame.GameObjects[i].YPosition == y)
                {
                    index = i;
                }
            }
            return index;

        }
        public static int GetWallIndex(int x, int y)
        {
            int index = 0;
            for (int i = 0; i < Program.mygame.Walls.Count; i++)
            {
                if (Program.mygame.Walls[i].XPosition == x && Program.mygame.Walls[i].YPosition == y)
                {
                    index = i;
                }
            }
            return index;
        }

        public static void TimeMethod(int timeToStart, int intervalTime, int timeAmount, int index, int f, int starttime, Action method)
        {
            if (starttime == 0)
            {
                starttime = elapsedTime;
            }
            bool continuee = true;
            for (int i = 0; i < TimeList.Count; i++)
            {
                if (TimeList[i] == method && continuee)
                {
                    TimeList.RemoveAt(i);
                    intList.RemoveAt(i);
                    continuee = false;
                    //break;
                }
            }

            if (f < timeAmount)
            {
                if (elapsedTime - starttime >= timeToStart && f == 0 || elapsedTime - starttime > intervalTime && f != 0)
                {
                    f++;
                    starttime = elapsedTime;
                    method();

                }
                TimeList.Add(method);

                int[] list = { timeToStart, intervalTime, timeAmount, index, f, starttime };

                intList.Add(list);
            }
        }
        public static int elapsedTime = 0;
    }
}
