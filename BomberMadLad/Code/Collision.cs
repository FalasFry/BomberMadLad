using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    static class Collision
    {
        //kolla kollision på inskickade koordinater
        public static bool Wall(int xPos, int yPos)
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

        // Kollar om spelaren kolliderar med det vi skickade in.
        public static bool Char(int xPos, int yPos)
        {
            //för varje karraktär som finns
            for (int i = 0; i < Program.mygame.Characters.Count; i++)
            {
                //om y positionen är samma
                if (Program.mygame.Characters[i].YPosition == yPos)
                {
                    // Om x positionen eller dens grannar är samma.
                    if (Program.mygame.Characters[i].XPosition == xPos - 1 || Program.mygame.Characters[i].XPosition == xPos || Program.mygame.Characters[i].XPosition == xPos + 1)
                    {
                        //WE GOT COLLISION
                        return false;
                    }
                }
            }
            return true;
        }

        // Hämtar index från ett gameobject.
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

        // Hämtar index från väggarna.
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
    }
}
