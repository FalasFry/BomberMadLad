using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    abstract class GameObject
    {
        public bool CanBlow { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

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
}
