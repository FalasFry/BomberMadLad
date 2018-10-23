using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberMadLad
{
    class TimerClass
    {
        public static List<Action> TimeList = new List<Action>();

        public static List<int[]> intList = new List<int[]>();

        public static void AddTimer(int index, int timetostart, int intervalTime, int timeAmount, int starttime, Action method)
        {

            TimeList.Add(method);

            int[] list = { timetostart, intervalTime, timeAmount, index, 0, starttime };

            intList.Add(list);
        }

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

        public static void TimeMethod(int timeToStart, int intervalTime, int timeAmount, int index, int f, int starttime, Action method)
        {
            if (starttime == 0)
            {
                starttime = elapsedTime;
            }
            bool continuee = true;
            for (int i = 0; i < TimeList.Count; i++)
            {
                if (TimeList[i] == method && continuee)
                {
                    TimeList.RemoveAt(i);
                    intList.RemoveAt(i);
                    continuee = false;
                }
            }

            if (f < timeAmount)
            {
                if (elapsedTime - starttime >= timeToStart && f == 0 || elapsedTime - starttime > intervalTime && f != 0)
                {
                    f++;
                    starttime = elapsedTime;
                    method();

                }
                TimeList.Add(method);

                int[] list = { timeToStart, intervalTime, timeAmount, index, f, starttime };

                intList.Add(list);
            }
        }
        public static int elapsedTime = 0;
    }
}
