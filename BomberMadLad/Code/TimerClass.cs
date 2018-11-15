using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BomberMadLad
{
    class TimerClass
    {
        public static List<Action> TimeList = new List<Action>();

        public static List<int[]> intList = new List<int[]>();

        // Lägg till en timer med värdena som skickas in.
        // Index används i method om man behöver komma åt ett specifikt gameobject.
        public static void AddTimer(int index, int timetostart, int intervalTime, int timeAmount, Action method)
        {
            // Alla listor får ett unikt värde så att systemet inte blandar ihop likadana methods med olika tider.
            // Om det inte finns några timers så blir värdet 0
            int indexNum = 0;
            if (intList.Count != 0)
            {
                indexNum = intList[intList.Count - 1][6] + 1;
            }
            // Lägg till metoden i sin lista
            TimeList.Add(method);

            // Lägg till värdena vi skickade in innan
            // Två av värdena är 0, en av dem är ett index för hur många ggr metoden körts innan vilket alltid är 0 i en ny timer
            // Den andra håller reda på tiden men den fixas i Timemethod.
            int[] list = { timetostart, intervalTime, timeAmount, index, 0, 0, indexNum };
            
            intList.Add(list);
            
        }

        // Hitta index för gameobjects.
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

        // Hitta index för väggar.
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

        // Körs varje update.
        public static void TimeMethod()
        {
            //körs en gång för alla aktiva timers.
            for (int i = 0; i < TimeList.Count; i++)
            {
                //sätt värdena för timer[i]
                int timeUntilFirstCall = intList[i][0];
                int timeBetweenCalls= intList[i][1];
                int amountOfCalls = intList[i][2];
                int index = intList[i][3];
                int callIndex = intList[i][4];
                int startTime = intList[i][5];
                int indexNum = intList[i][6];
                Action methodToCall = TimeList[i];

                //om metoden inte körts innan så sätter vi tiden från stopwatch då den skapades
                if (startTime == 0)
                {
                    startTime = elapsedTime;
                }
                

                //om metoden inte har körts alla ggr än
                if (callIndex < amountOfCalls)
                {
                    //har ingen aning om vad detta är eller varför det funkar
                    if (elapsedTime - startTime >= timeUntilFirstCall && callIndex == 0 || elapsedTime - startTime > timeBetweenCalls && callIndex != 0)
                    {
                        callIndex++;
                        startTime = elapsedTime;
                        methodToCall();

                    }

                    int[] list = { timeUntilFirstCall, timeBetweenCalls, amountOfCalls, index, callIndex, startTime, indexNum };

                    intList[i] = list;
                }
                //om metoden körts tillräckligt många ggr tas den bort.
                else
                {

                    TimeList.RemoveAt(i);
                    intList.RemoveAt(i);
                }
            }
        }

        public static int elapsedTime = 0;
    }
}
