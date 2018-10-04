using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Uppgift1: Skriv en ny klass Enemy, som ärver Gameobject och som vandrar runt slumpmässigt på spelplanen. Skapa en new Enemy och lägg till i GameObjects-listan.
//Uppgift2: Skriv en ny klass Player, som ärver Gameobject och som kan styras med tangenterna WASD. Skapa en new Player och lägg till i GameObjects-listan.
namespace GridGame
{

    class Program
    {
        static void Main(string[] args)
        {
            Game myGame = new Game(15, 6);
            while (true)
            {
                myGame.UpdateBoard();
                myGame.DrawBoard();
                Console.ReadKey();
            }


        }
    }

    class Game
    {

        List<GameObject> GameObjects = new List<GameObject>();

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
                gameObject.Draw(5, 3);
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
            Console.SetCursorPosition(startX, startY);
            Console.Write("█████");
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("█████");
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write("█████");
        }

        public override void Update()
        {

        }
    }

}