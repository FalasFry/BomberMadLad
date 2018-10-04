using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


//Uppgift1: Skriv en ny klass Enemy, som ärver Gameobject och som vandrar runt slumpmässigt på spelplanen. Skapa en new Enemy och lägg till i GameObjects-listan.
//Uppgift2: Skriv en ny klass Player, som ärver Gameobject och som kan styras med tangenterna WASD. Skapa en new Player och lägg till i GameObjects-listan.
namespace GridGame
{
    class Program
    {
        public static Game mygame = new Game(Console.LargestWindowWidth - 13, Console.LargestWindowHeight-12);
        public static bool haveAI;
        static void Main(string[] args)
        {

            Console.WriteLine(" Do you want AI? (Y/N)"); 
            ConsoleKey input = Console.ReadKey(true).Key;
            if(input == ConsoleKey.Y)           
            {
                haveAI = true;
            }

            Console.SetWindowSize(Console.LargestWindowWidth- 10, Console.LargestWindowHeight- 10);
            mygame.DrawBoard();
            while (true)
            {

                mygame.UpdateBoard();
            }
        }
        public Game findGame()
        {
            return mygame;
        }
    }

    class Game
    {
        public List<GameObject> GameObjects = new List<GameObject>();
        public Random rnd = new Random();
        public Game(int xSize, int ySize)
        {
            for (int i = 0; i < ySize + 2; i++)
            {
                for (int j = 0; j < xSize + 2; j++)
                {
                    if (j == 0 || i == 0 || i == ySize + 1 || j == xSize + 1)
                    {
                        GameObjects.Add(new Wall(j, i));
                    }
                    if (i <= ySize/2 && j <= xSize/4)
                    {
                        GameObjects.Add(new Wall(j * 4, i * 2));
                    }
                }
            }
            GameObjects.Add(new Player());
            if(Program.haveAI == true)
            {
                GameObjects.Add(new AI());
            }
        }

        public void DrawBoard()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Draw(1, 1);
            }
            
        }

        public void UpdateBoard()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update();
            }

        }
    }

    abstract class GameObject
    {
        public int XPosition;
        public int YPosition;
        public abstract void Draw(int xBoxSize, int yBoxSize);
        public abstract void Update();
        
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
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(startX, startY);
            Console.Write("██");
        }

        public override void Update()
        {

        }
    }

    class Player : GameObject
    {
        Game game;

        int xPos = 10;
        int yPos = 10;

        public Player()
        {

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
            if (!CollisionCheck())
            {
                xPos = oldX;
                yPos = oldY;
            }
            else Delete(oldX, oldY);
        }

        public void Delete(int oldX, int oldY)
        {
            Console.SetCursorPosition(oldX, oldY);
            Console.Write("  ");
        }

        public bool CollisionCheck()
        {
            for (int i = 0; i < Program.mygame.GameObjects.Count; i++)
            {
                if (Program.mygame.GameObjects[i].YPosition == yPos)
                {
                    if (Program.mygame.GameObjects[i].XPosition == xPos - 1 || Program.mygame.GameObjects[i].XPosition == xPos || Program.mygame.GameObjects[i].XPosition == xPos + 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    class AI : GameObject
    {
        public Random rng = new Random();
        int xPos;
        int yPos;
        
        public AI()
        {
            xPos = rng.Next(5, 241);
            yPos = rng.Next(5, 60);

            if(xPos == 5)
            {
                xPos = 6;
            }
            if(yPos == 5)
            {
                yPos = 6;
            }
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("██");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public override void Update()
        {
            int dir = rng.Next(1, 5);
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
                xPos++;
            }
            if (dir == 4)
            {
                xPos--;
            }
        }
    }
}