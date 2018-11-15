using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class Move
    {
        // Går Upp.
        public void Up(GameObject obj)
        {
            obj.YPosition--;
        }
        // Går ner.
        public void Down(GameObject obj)
        {
            obj.YPosition++;
        }
        // Går Vänster.
        public void Left(GameObject obj)
        {
            obj.XPosition -= 2;
        }
        // Går Höger.
        public void Right(GameObject obj)
        {
            obj.XPosition += 2;
        }
        // Skapar en ny bomb och lägger ut den.
        public void LayBomb(int XPosition, int YPosition)
        {
            BOOM latestBoom = new BOOM(XPosition, YPosition);

            // Lägg till i gameobjects
            Program.mygame.GameObjects.Add(latestBoom);


            // Lägg till timer
            int index = TimerClass.GetIndex(latestBoom.XPosition, latestBoom.YPosition);

            TimerClass.AddTimer(index, 1000, 500, 10, Program.mygame.GameObjects[index].Action2);
        }
    }
}
