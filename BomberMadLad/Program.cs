using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;


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
            Console.WriteLine(" Do you want AI? (Y/N)");
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Y)
            {
                haveAI = true;
            }
            else haveAI = false;

            //ändra storlek på konsolfönstret till största möjliga
            Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight - 10);

            //kalla på drawboard metoden för att rita ut alla saker i walls listan. den körs bara en gång eftersom väggarna inte behöver uppdateras
            mygame.DrawBoard();

            //denna metoden körs hela tiden
            while (true)
            {
                //ritar ut alla gameobjects i listan GameObjects
                mygame.DrawStuff();

                //kallar på update i alla GameObjects
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
                        Walls.Add(new Wall(j, i));
                    }
                    //räkna ut koordinaterna för mönster. (OBS RÖR INGET DET FUNKAR)
                    if (i <= ySize / 2 && j <= xSize / 4)
                    {
                        Walls.Add(new Wall(j * 4, i * 2));
                    }
                }
            }

            GameObjects.Add(player);
            
        }

        public void DrawBoard()
        {

            //rita ut väggar
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
        public int XPosition;
        public int YPosition;
        public abstract void Draw(int xBoxSize, int yBoxSize);
        public abstract void Update();

        //ta bort en ruta på positionen som skickas in
        public void Delete(int oldX, int oldY)
        {
            Console.SetCursorPosition(oldX, oldY);
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
    }
    
    class Wall : GameObject
    {
        public Wall(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            int startX = XPosition * xBoxSize;
            int startY = YPosition * yBoxSize;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(startX, startY);
            Console.Write("██");
        }

        public override void Update()
        {

        }
    }

    class Player : GameObject
    {

        int xPos = 10;
        int yPos = 10;

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
        }

        public override void Update()
        {
            //återställ oldX + old Y
            int oldX = xPos;
            int oldY = yPos;

            //kollar efter spelarens input. OBS måste bytas ut för att inte bli turnbased eftersom readkey väntar på knapptryck
            ConsoleKey input = Console.ReadKey(true).Key;

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

            //????
            if (input == ConsoleKey.Spacebar)
            Draw(0, 0);

            //lägg bomb
            if (input == ConsoleKey.Spacebar && layBomb)
            {
                TimerClass timer = new TimerClass();
                // Big Boom
                
                latestBoom = new BOOM(xPos, yPos);

                //lägg till i gameobjects
                Program.mygame.GameObjects.Add(latestBoom);

                timer.BoomCooldown();
                layBomb = false;

                //starta timer till explosion
                timer.StartBoom(latestBoom);
            }
        }

        public void PlayerBoomCooldown(object o)
        {
            layBomb = true;
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

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("██");
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
        TimerClass timer;
        int index;
        int xPos;
        int yPos;


        public BOOM(int playerPosX, int playerPosY)
        {
            timer = new TimerClass();
            index = Program.mygame.GameObjects.Count;
            xPos = playerPosX;
            yPos = playerPosY;

        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("██");
        }

        public override void Update()
        {

        }

        //sätter ut en explosion (röd 3x3 fyrkant)
        public void RemoveBoom(object o)
        {
            // Kors Sidan
            /*Program.mygame.GameObjects.RemoveAt(index);
            Console.SetCursorPosition(xPos-2, yPos);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("████████████");
            Console.SetCursorPosition(xPos, yPos + 1 );
            Console.Write("██");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(xPos , yPos - 1);
            Console.Write("██");*/

            // Kors Upp
            Program.mygame.GameObjects.RemoveAt(index);
            Console.SetCursorPosition(xPos - 2, yPos);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("██████");
            Console.SetCursorPosition(xPos, yPos + 1);
            Console.Write("██");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(xPos, yPos - 1);
            Console.Write("██");
            Console.SetCursorPosition(xPos, yPos + 2);
            Console.Write("██");
            Console.SetCursorPosition(xPos, yPos + 3);
            Console.Write("██");
            Console.SetCursorPosition(xPos, yPos + 4);
            Console.Write("██");

            // Default 3x3
            /*Program.mygame.GameObjects.RemoveAt(index);
            Console.SetCursorPosition(xPos - 2, yPos);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("██████");
            Console.SetCursorPosition(xPos - 2, yPos + 1);
            Console.Write("██████");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(xPos - 2, yPos - 1);
            Console.Write("██████");*/

        }
        //tar bort explosionen och ersätter den med spaces. (FÅR IGENTLIGEN INTE TA SÖNDER VÄGGAR)
        public void BOOOOM(object o)
        {
            // Kors Sida
            /*Console.SetCursorPosition(xPos, yPos + 1);
            Console.Write("  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(xPos, yPos - 1);
            Console.Write("  ");
            Console.SetCursorPosition(xPos-2, yPos);
            Console.Write("            ");
            Debug.Write("boom");*/

            // Kors Upp
            Console.SetCursorPosition(xPos - 2, yPos);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("      ");
            Console.SetCursorPosition(xPos, yPos + 1);
            Console.Write("  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(xPos, yPos - 1);
            Console.Write("  ");
            Console.SetCursorPosition(xPos, yPos + 2);
            Console.Write("  ");
            Console.SetCursorPosition(xPos, yPos + 3);
            Console.Write("  ");
            Console.SetCursorPosition(xPos, yPos + 4);
            Console.Write("  ");

            // Default 3x3
            /*
            Console.SetCursorPosition(xPos - 2, yPos);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("      ");
            Console.SetCursorPosition(xPos - 2, yPos + 1);
            Console.Write("      ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(xPos - 2, yPos - 1);
            Console.Write("      ");*/
        }

    }

    // Klass för timer
    class TimerClass
    {
        BOOM boom;
        Player player;

        //???
        int bombCoolDown;

        public TimerClass()
        {
            boom = Program.mygame.player.latestBoom;
            player = Program.mygame.player;
            bombCoolDown = 2000;
        }

        // Metoder för att timers ska användas i olika tillfällen. 

        //sätter ut explosion
        public void StartBoom(BOOM boom)
        {
            Timer time = new Timer(boom.RemoveBoom, null, bombCoolDown, Timeout.Infinite);
            Timer time2 = new Timer(boom.BOOOOM, null, bombCoolDown + 500, Timeout.Infinite);
            
        }
        //cooldown
        public void BoomCooldown()
        {
            
            Timer time = new Timer(player.PlayerBoomCooldown, null, bombCoolDown, Timeout.Infinite);
        }
        
    }

    class PowerUps
    {
        Player player;
        Game game;

        public PowerUps()
        {
            game = Program.mygame;
            player = game.player;
        }
    }
}