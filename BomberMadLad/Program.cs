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
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth- 10, Console.LargestWindowHeight- 10);
            Game myGame = new Game(Console.LargestWindowWidth - 13, Console.LargestWindowHeight - 12);
            while (true)
            {
                myGame.UpdateBoard();
                myGame.DrawBoard();
                Console.CursorVisible = false;
            }
        }
    }

    // SteffeFanWas Here
    class Game
    {
        public List<GameObject> GameObjects = new List<GameObject>();
        
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
                }
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
            Debug.Write("drew");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(xPos, yPos);
            Console.Write("██");
        }

        public override void Update()
        {
            Draw(0,0);
            int oldX = xPos;
            int oldY = yPos;
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.W)
            {
                    yPos -= 1;
            }
            if (input == ConsoleKey.S)
            {
                    yPos += 1;
            }
            if (input == ConsoleKey.D)
            {
                    xPos += 1;
            }
            if (input == ConsoleKey.A)
            {
                    xPos -= 1;
            }
            if (!CollisionCheck())
            {
                xPos = oldX;
                yPos = oldY;
            }
            Delete(oldX, oldY);
        }
        public void Delete(int oldX, int oldY)
        {
            Console.SetCursorPosition(oldX, oldY);
            Console.Write("  ");
        }

        public bool CollisionCheck()
        {
            for (int i = 0; i < 1; i++)
            {
                if (game.GameObjects[i].XPosition == xPos && game.GameObjects[i].YPosition == yPos)
                {
                    return false;
                }
            }
            return true;
        }
    }

    class AI : GameObject
    {
        public AI()
        {
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}