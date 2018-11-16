using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BomberMadLad
{
    abstract class GameObject
    {
        public bool CanBlow { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public abstract void Draw(int xBoxSize, int yBoxSize);
        public abstract void Update();
        public abstract void Action1();
        public abstract void Action2();
        public abstract void Action3();

        //ta bort en ruta på positionen som skickas in
        public void Delete(int oldX, int oldY)
        {
            Console.SetCursorPosition(oldX, oldY);
            Console.Write("  ");
        }

        // Raderar bomberna så att dem inte syns eller har colision.
        public void Destroy(int index, bool walls)
        {
            if (walls)
            {
                if (index > Program.mygame.GameObjects.Count && Program.mygame.Walls[index].CanBlow)
                {
                    Delete(Program.mygame.Walls[index].XPosition, Program.mygame.Walls[index].YPosition);
                    Wall replacement = new Wall(0,0,false);
                    Program.mygame.Walls[index] = replacement;
                }
            }
            else
            {
                // Suddar bort väggen vi har och sedan flyttar den till ett hörn så den inte har collision.
                Delete(Program.mygame.GameObjects[index].XPosition, Program.mygame.GameObjects[index].YPosition);
                GameObject replacement = new Wall(0,0, false);
                Program.mygame.GameObjects[index] = replacement;
            }
            
        }
    }
}
