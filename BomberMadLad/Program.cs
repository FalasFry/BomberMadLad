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
        public static Game mygame = new Game(Console.LargestWindowWidth - 13, Console.LargestWindowHeight - 12);
        public static bool haveAI;
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.WriteLine(" Do you want AI? (Y/N)");
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Y)
            {
                haveAI = true;
            }
            else haveAI = false;

            Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight - 10);
            mygame.DrawBoard();
            while (true)
            {
                mygame.DrawStuff();
                mygame.UpdateBoard();
            }
        }
    }

    class Game
    {
        public List<GameObject> GameObjects = new List<GameObject>();
        public List<GameObject> Walls = new List<GameObject>();
        public Player player = new Player();
        public Game(int xSize, int ySize)
        {
            for (int i = 0; i < ySize + 2; i++)
            {
                for (int j = 0; j < xSize + 2; j++)
                {
                    if (j == 0 || i == 0 || i == ySize + 1 || j == xSize + 1)
                    {
                        Walls.Add(new Wall(j, i));
                    }
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

            if (Program.haveAI == true)
            {
                GameObjects.Add(new AI());
            }

            foreach (GameObject gameObject in Walls)
            {
                gameObject.Draw(1, 1);
            }
        }
        public void DrawStuff()
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Draw(1,1);
            }
        }
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

        public void Delete(int oldX, int oldY)
        {
            Console.SetCursorPosition(oldX, oldY);
            Console.Write("  ");
        }

        public bool CollisionCheck(int xPos, int yPos)
        {
            for (int i = 0; i < Program.mygame.Walls.Count; i++)
            {
                if (Program.mygame.Walls[i].YPosition == yPos)
                {
                    if (Program.mygame.Walls[i].XPosition == xPos - 1 || Program.mygame.Walls[i].XPosition == xPos || Program.mygame.Walls[i].XPosition == xPos + 1)
                    {
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
        public BOOM latestBoom;

        public Player()
        {
            layBomb = true;
        }
        
        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(xPos, yPos);
            Console.Write("██");
        }

        public override void Update()
        {
            Draw(0, 0);
            int oldX = xPos;
            int oldY = yPos;

            ConsoleKey input = Console.ReadKey(true).Key;

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
            if (!CollisionCheck(xPos, yPos))
            {
                xPos = oldX;
                yPos = oldY;
            }
            else Delete(oldX, oldY);
            if (input == ConsoleKey.Spacebar)
            Draw(0, 0);
            if (input == ConsoleKey.Spacebar && layBomb)
            {
                TimerClass timer = new TimerClass();
                // Big Boom
                latestBoom = new BOOM(xPos, yPos);
                Program.mygame.GameObjects.Add(latestBoom);
                timer.BoomCooldown();
                layBomb = false;
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

            if (xPos == 5)
            {
                xPos = 6;
            }
            if (yPos == 5)
            {
                yPos = 6;
            }
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("██");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public override void Update()
        {

            int oldX = xPos;
            int oldY = yPos;
            int dir = rng.Next(1, 5);
            bool moved = false;

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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("██");
        }

        public override void Update()
        {

        }

        public void RemoveBoom(object o)
        {
            Program.mygame.GameObjects.RemoveAt(index);
            Console.SetCursorPosition(xPos, yPos);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("  ");
            Debug.Write("boom");
        }
    }

    class TimerClass
    {
        BOOM boom;
        Player player;
        int fiveSeconds;
        public TimerClass()
        {
            boom = Program.mygame.player.latestBoom;
            player = Program.mygame.player;
            fiveSeconds = 2000;
        }

        public void StartBoom(BOOM boom)
        {
            
            Timer time = new Timer(boom.RemoveBoom, null, fiveSeconds, Timeout.Infinite);
        }
        public void BoomCooldown()
        {
            
            Timer time = new Timer(player.PlayerBoomCooldown, null, fiveSeconds, Timeout.Infinite);
        }
    }
}