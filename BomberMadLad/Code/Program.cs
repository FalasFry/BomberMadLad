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
        // Skapa game utefter storleken på skärmen
        public static Game mygame = new Game(Console.LargestWindowWidth - 13, Console.LargestWindowHeight - 12);

        // Metod som startas när spelet gör det
        static void Main(string[] args)
        {
            // Sätter musen till osynlig
            Console.CursorVisible = false;
            
            // Skapar Meny.
            Menu.MainMenu();

            // Ändra storlek på konsolfönstret till största möjliga
            Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight - 9);

            // Kalla på drawboard metoden för att rita ut alla saker i walls listan. den körs bara en gång eftersom väggarna inte behöver uppdateras
            mygame.DrawBoard();

            Stopwatch stopwatch = new Stopwatch();

            // Denna metoden körs hela tiden
            while (true)
            {
                stopwatch.Start();

                // Ritar ut alla gameobjects i listan GameObjects
                mygame.DrawStuff();

                // Kallar på update i alla GameObjects
                mygame.UpdateBoard();

                // Kolla om någon timer ska kallas på
                TimerClass.TimeMethod();

                // Stoppa stopwatch
                stopwatch.Stop();

                // Uppdatera elapsedtime
                TimerClass.elapsedTime = (int)stopwatch.ElapsedMilliseconds;

                // Om det tar 5 minuter fölorar du.
                if(stopwatch.ElapsedMilliseconds == 300000)
                {
                    End end = new End();
                    end.GameOver(false);
                }
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
        public Player player = new Player(22,22);
        
        //ny AI
        public Ai ai = new Ai(Console.LargestWindowWidth - 22, 22);
        
        int X;
        int Y;

        //skapar game med måtten vi skickade in i Program
        public Game(int xSize, int ySize)
        {
            Y = ySize;
            X = xSize;
            Random drawWhiteWalls = new Random();

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
                    //mänster för vita väggar
                    else if ((j + 2) % 4 == 0 || (i + 1) % 2 == 0)
                    {
                        // De vita väggarna har en 33% chans att ritas ut.
                        if (drawWhiteWalls.Next(0, 3) == 1)
                        {
                            Walls.Add(new Wall(j, i, true));
                        }
                    }
                    //räkna ut koordinaterna för mönster.
                    if (i <= ySize / 2 && j <= X / 2)
                    {
                        Walls.Add(new Wall(j * 2, i * 2, false));
                    }
                }
            }
            //lägg till spelare
            Characters.Add(player);
        }
        //vilken BR vi är på
        int brIndex = 0;
        //hur långt in arenan kan gå
        int maxIndex = 20;

        //ropas på varje gång arenan blir mindre
        public void Br()
        {
            //öka brIndex
            brIndex++;

            //skapa en kvadrat av väggar
            for (int k = 0; k <= Y + 1; k++)
            {
                for (int j = 0; j <= X + 1; j++)
                {
                    Wall wall = null;
                    if (j == (X+1) - brIndex)
                    {
                        wall = (new Wall(j - brIndex, k, false));
                        wall.Draw(0, 0);
                        Walls.Add(wall);

                        // Om det är en karaktär i väggen som kommer in tar spelet slut.
                        if (!Collision.Char(wall.XPosition, wall.YPosition))
                        {
                            End end = new End();
                            if (wall.XPosition == Program.mygame.player.XPosition && wall.YPosition == Program.mygame.player.YPosition)
                            {
                                end.GameOver(false);
                            }
                            else
                            {
                                end.GameOver(true);
                            }
                        }
                    }

                    if (j == 0 + brIndex)
                    {
                        wall = (new Wall(j + brIndex, k, false));
                        wall.Draw(0, 0);
                        Walls.Add(wall);

                        // Om det är en karaktär i väggen som kommer in tar spelet slut.
                        if (!Collision.Char(wall.XPosition, wall.YPosition))
                        {
                            End end = new End();
                            if (wall.XPosition == Program.mygame.player.XPosition && wall.YPosition == Program.mygame.player.YPosition)
                            {
                                end.GameOver(false);
                            }
                            else
                            {
                                end.GameOver(true);
                            }
                        }
                    }

                    if (k == (Y+1) - brIndex)
                    {
                        wall = (new Wall(j, k, false));
                        wall.Draw(0, 0);
                        Walls.Add(wall);

                        // Om det är en karaktär i väggen som kommer in tar spelet slut.
                        if (!Collision.Char(wall.XPosition, wall.YPosition))
                        {
                            End end = new End();
                            if (wall.XPosition == Program.mygame.player.XPosition && wall.YPosition == Program.mygame.player.YPosition)
                            {
                                end.GameOver(false);
                            }
                            else
                            {
                                end.GameOver(true);
                            }
                        }
                    }

                    if (k == 0 + brIndex)
                    {
                        wall = (new Wall(j, k, false));
                        wall.Draw(0, 0);
                        Walls.Add(wall);

                        // Om det är en karaktär i väggen som kommer in tar spelet slut.
                        if (!Collision.Char(wall.XPosition, wall.YPosition))
                        {
                            End end = new End();
                            if (wall.XPosition == Program.mygame.player.XPosition && wall.YPosition == Program.mygame.player.YPosition)
                            {
                                end.GameOver(false);
                            }
                            else
                            {
                                end.GameOver(true);
                            }
                        }
                    }
                }
            }
        }
        
        // Rita ut alla väggar (körs bara en gång så här gör vi annat också)
        public void DrawBoard()
        {
            //starta timer för BR
            TimerClass.AddTimer(0, 10000, 10000, maxIndex, Br);

            int wallsIndex = 1;
            
            //lägg till AI bland characters
            Characters.Add(ai);
            
            
            //se till att inga karaktärer startar inuti en vägg genom att få dem att starta inuti ett +
            for (int i = 0; i < Characters.Count; i++)
            {
                wallsIndex = Collision.GetWallIndex(Characters[i].XPosition, Characters[i].YPosition);
                Walls[wallsIndex].Destroy(wallsIndex, true);
                wallsIndex = Collision.GetWallIndex(Characters[i].XPosition - 2, Characters[i].YPosition);
                Walls[wallsIndex].Destroy(wallsIndex, true);
                wallsIndex = Collision.GetWallIndex(Characters[i].XPosition + 2, Characters[i].YPosition);
                Walls[wallsIndex].Destroy(wallsIndex, true);
                wallsIndex = Collision.GetWallIndex(Characters[i].XPosition, Characters[i].YPosition - 1);
                Walls[wallsIndex].Destroy(wallsIndex, true);
                wallsIndex = Collision.GetWallIndex(Characters[i].XPosition, Characters[i].YPosition + 1);
                Walls[wallsIndex].Destroy(wallsIndex, true);

            }
            //rita ut alla väggar
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
        //eftersom det finns två sorters väggar har vi en bool för om dem går att spränga eller inte
        public Wall(int xPosition, int yPosition, bool Destroyable)
        {
            CanBlow = Destroyable;
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            //ändra färg beroende på vilken sorts vägg
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
        }

        public override void Action2()
        {
        }

        public override void Action3()
        {
        }
    }
    

}
