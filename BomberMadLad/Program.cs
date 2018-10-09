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
        public static List<Action> TimeList = new List<Action>();
        public static List<int[]> intList = new List<int[]>();

        public static int GetIndex(int x, int y)
        {
            int index = 0;
            for (int i = 0; i < mygame.GameObjects.Count; i++)
            {
                if (mygame.GameObjects[i].XPosition == x && mygame.GameObjects[i].YPosition == y)
                {
                    index = i;
                }
            }
            return index;
            
        }
        public static int GetWallIndex(int x, int y)
        {
            int index = 0;
            for (int i = 0; i < mygame.Walls.Count; i++)
            {
                if (mygame.Walls[i].XPosition == x && mygame.Walls[i].YPosition == y)
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
            Console.WriteLine(" Do you want AI? (Y/N)");
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Y)
            {
                haveAI = true;
            }
            else haveAI = false;

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
                

                for (int i = 0; i < TimeList.Count; i++)
                {
                    TimeMethod(intList[i][0], intList[i][1], intList[i][2], intList[i][3], intList[i][4], intList[i][5], TimeList[i]);
                }

                stopwatch.Stop();

                elapsedTime = (int)stopwatch.ElapsedMilliseconds;
            }

        }
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
                GameObjects[i].Draw(1,1);
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

            Console.SetCursorPosition(XPosition,YPosition);
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
                Program.GetIndex(xPos, yPos);
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

                int index = Program.GetIndex(xPos, yPos);

                Program.TimeList.Add(Program.mygame.GameObjects[index].Blow);

                int[] list1 = { 1000, 0, 1, index, 0, 0 };

                Program.intList.Add(list1);

                layBomb = false;

                //lägg till timer

                index = Program.GetIndex(latestBoom.XPosition, latestBoom.YPosition);

                Program.TimeList.Add(Program.mygame.GameObjects[index].Blow);

                int[] list = { 1000, 500, 10, index, 0, 0 };

                Program.intList.Add(list);
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
            Debug.WriteLine("removed");
            int index = Program.GetIndex(xPos,yPos);
            if (index != 0)
            {
                Program.mygame.GameObjects[index].Destroy(index, false);
            }
                CrossBomb(xPos, yPos, "██");
            
        }
        public void BOOOOM()
        {
            CrossBomb(xPos, yPos, "  ");
        }
        public void CrossBomb(int xposition, int yposition, string toWrite)
        {


        //    int i = 1;
        //    int j = 1;
        //    bool oneside = false;
        //    int index = 0;
        //    while (CollisionCheck(xposition - i, yposition))
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.SetCursorPosition(xposition - i, yposition);
        //        Console.Write(toWrite);

            //        if (!oneside)
            //        {
            //            i *= -1;
            //            if (i > 0)
            //            {
            //                i++;
            //            }
            //        }
            //        if (!CollisionCheck(xposition - i, yposition) && !oneside)
            //        {
            //            if (toWrite != "  ")
            //            {
            //                int mult = 1;
            //                if (i < 0) mult = -1;

            //                index = Program.GetWallIndex(xposition - i - mult, yposition);
            //                for (int x = 0; x < Program.mygame.Walls.Count; x++)
            //                {


            //                    if (Program.mygame.Walls[x].XPosition == xposition - i - mult && Program.mygame.Walls[x].YPosition == yposition && Program.mygame.Walls[x].CanBlow)
            //                    {
            //                        index = x;
            //                    }
            //                }
            //                Debug.WriteLine("index1 = " + index + " xpos = " + Program.mygame.Walls[index].XPosition + " ypos =  " + Program.mygame.Walls[index].YPosition);
            //                if (index != 0) Program.mygame.Walls[index].Destroy(index, true);
            //            }
            //            i *= -1;
            //            if (i > 0)
            //            {
            //                i--;
            //            }
            //            oneside = true;
            //        }
            //        if (oneside)
            //        {
            //            if (i < 0) i--;
            //            if (i > 0) i++;
            //        }
            //    }

            //    if (toWrite != "  ")
            //    {
            //        index = 0;
            //        for (int x = 0; x < Program.mygame.Walls.Count; x++)
            //        {
            //            int mult = 1;
            //            if (i < 0) mult = -1;
            //            if (Program.mygame.Walls[x].XPosition == xposition - i - mult && Program.mygame.Walls[x].YPosition == yposition && Program.mygame.Walls[x].CanBlow)
            //            {
            //                index = x;
            //            }
            //        }
            //        Debug.WriteLine("index2 = " + index + " xpos = " + Program.mygame.Walls[index].XPosition + " ypos =  " + Program.mygame.Walls[index].YPosition);
            //        if (index != 0) Program.mygame.Walls[index].Destroy(index, true);

            //    }
            //    oneside = false;
            //    while (CollisionCheck(xposition, yposition - j))
            //    {
            //        Console.ForegroundColor = ConsoleColor.Red;
            //        Console.SetCursorPosition(xposition, yposition - j);
            //        Console.Write(toWrite);

            //        if (!oneside)
            //        {
            //            j *= -1;
            //            if (j > 0)
            //            {
            //                j++;
            //            }
            //        }
            //        if (!CollisionCheck(xposition, yposition - j) && !oneside)
            //        {
            //            if (toWrite != "  ")
            //            {
            //                for (int x = 0; x < Program.mygame.Walls.Count; x++)
            //                {
            //                    if (Program.mygame.Walls[x].XPosition == xposition && Program.mygame.Walls[x].YPosition == yposition - j && Program.mygame.Walls[x].CanBlow)
            //                    {
            //                        index = x;
            //                    }
            //                }
            //                Debug.WriteLine("index3 = " + index + " xpos = " + Program.mygame.Walls[index].XPosition + " ypos =  " + Program.mygame.Walls[index].YPosition);
            //                if (index != 0) Program.mygame.Walls[index].Destroy(index, true);
            //            }

            //            j *= -1;
            //            if (i > 0)
            //            {
            //                j--;
            //            }
            //            oneside = true;
            //        }
            //        if (oneside)
            //        {
            //            if (j < 0) j--;
            //            if (j > 0) j++;
            //        }
            //    }
            //    if (toWrite != "  ")
            //    {
            //        index = 0;
            //        for (int x = 0; x < Program.mygame.Walls.Count; x++)
            //        {

            //            if (Program.mygame.Walls[x].XPosition == xposition && Program.mygame.Walls[x].YPosition == yposition - j && Program.mygame.Walls[x].CanBlow)
            //            {
            //                index = x;
            //            }
            //        }
            //        Debug.WriteLine("index4 = " + index + " xpos = " + Program.mygame.Walls[index].XPosition + " ypos =  " + Program.mygame.Walls[index].YPosition);
            //        if (index != 0) Program.mygame.Walls[index].Destroy(index, true);
            //    }
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


                int index = Program.GetIndex(XPosition, YPosition);

                Program.TimeList.Add(Program.mygame.GameObjects[index].RemoveBlow);

                int[] list = { 1000, 1000, 1, index, 0, 0 };

                Program.intList.Add(list);
            }
        }

        public override void RemoveBlow()
        {
            Program.mygame.GameObjects[Program.GetIndex(XPosition, YPosition)].Destroy(Program.GetIndex(XPosition, YPosition), false);
            Debug.WriteLine("deathto guy");
            BOOOOM();
        }
    }

    // Klass för timer
    //class TimerClass
    //{
    //    BOOM boom;
    //    Player player;
    //    PoweupsSpawn power;

    //    int bombCoolDown;

    //    public TimerClass()
    //    {
    //        boom = Program.mygame.player.latestBoom;
    //        player = Program.mygame.player;
    //        bombCoolDown = 2000;
    //    }

    //    // Metoder för att timers ska användas i olika tillfällen. 

    //    //sätter ut explosion
    //    public void StartBoom(BOOM boom)
    //    {


    //        Timer time = new Timer(boom.RemoveBoom, null, 200, 200);
    //        Debug.WriteLine("pass");
    //        Timer time2 = new Timer(boom.BOOOOM, null, bombCoolDown + 10000, Timeout.Infinite);

    //    }
    //    //cooldown
    //    public void BoomCooldown()
    //    {

    //        Timer time = new Timer(player.PlayerBoomCooldown, null, bombCoolDown, Timeout.Infinite);
    //    }

    //    public void PowerupCooldown()
    //    {
    //        Timer time = new Timer(power.SpawnPowerup, null, 0, 10000);
    //    }

    //}

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
            while(wait)
            {
                //timer.PowerupCooldown();
                wait = false;
            }

            if(PowerNumber == 1)
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
        int number;
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

    // Sätter de tre punkter man kan spawna på istället för hårdkodat.
    class Spawn
    {
        AI aI;
        int number;
        int spawnOne;

        public Spawn()
        {
            number = aI.rng.Next(1, 4);

        }

        public void Point()
        {
            if(number == 1)
            {
            }
        }
    }
}