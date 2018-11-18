using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BomberMadLad
{
    class Ai : GameObject
    {
        //en bool som säger åt AI om den borde lägga ut en bomb
        public bool shouldBomb = false;

        //lista med positioner AI ska gå
        List<int[]> posList = new List<int[]>();


        public List<int[]> FindPath(int[] targetPosition)
        {

            int startInt = 0;

            Node final = null;

            //lista med alla nodes

             List<Node> allNodes = new List<Node>();

            //lista med valda nodes

             List<Node> Chosen = new List<Node>();

            //skapa första node

            Node Adam = new Node(targetPosition, new int[] { XPosition, YPosition });

            Adam.Hcost = (Math.Abs(Adam.goalPos[0] - Adam.nodePos[0])) / 2 + Math.Abs(Adam.goalPos[1] - Adam.nodePos[1]);

            //kommer hålla den före detta bästa noden
            Node bestNode = null;

            //ger varje generation ett Gcost
            int g = 0;
            
            //lägg till temp i listan av alla nodes
            allNodes.Add(Adam);

            //sålänge det finns oanvända positioner att sätta ut nodes
            while (allNodes.Count > 0)
            {
                //hitta node med bäst Fcost

                int LowestScore = allNodes.Min(x => x.Fcost);
                bestNode = allNodes.First(x => x.Fcost == LowestScore);

                //lägg till i listan på valda nodes
                Chosen.Add(bestNode);

                //ta bort från listan med nodes som kan väljas
                allNodes.Remove(bestNode);

                //om vi hitta målet så bryts while loopen
                if (bestNode.nodePos[0] == targetPosition[0] && bestNode.nodePos[1] == targetPosition[1])
                {
                    break;
                }

                //lista med möjliga barn
                List<Node> kids = new List<Node>();

                //kolla om var och en av de fyra positionerna är lediga och lägg isåfall dit en ny node

                if (Collision.Wall(bestNode.nodePos[0] - 2, bestNode.nodePos[1]))
                {
                    kids.Add(new Node(bestNode.goalPos, new int[] { bestNode.nodePos[0] - 2, bestNode.nodePos[1] }));
                }
                if (Collision.Wall(bestNode.nodePos[0] + 2, bestNode.nodePos[1]))
                {
                    kids.Add(new Node(bestNode.goalPos, new int[] { bestNode.nodePos[0] + 2, bestNode.nodePos[1] }));
                }
                if (Collision.Wall(bestNode.nodePos[0], bestNode.nodePos[1] + 1))
                {
                    kids.Add(new Node(bestNode.goalPos, new int[] { bestNode.nodePos[0], bestNode.nodePos[1] + 1 }));

                }
                if (Collision.Wall(bestNode.nodePos[0], bestNode.nodePos[1] - 1))
                {
                    kids.Add(new Node(bestNode.goalPos, new int[] { bestNode.nodePos[0], bestNode.nodePos[1] - 1 }));

                }


                List<Node> nextGen = kids;

                //ge nästa generation extra g värde
                g++;

                //för varje barn..
                for (int i = 0; i < nextGen.Count; i++)
                {
                    //bool som kollar om noden redan finns i en lista
                    bool beenUsed = false;

                    //för alla använda nodes..
                    for (int j = 0; j < Chosen.Count; j++)
                    {
                        //om det redan finns
                        if (Chosen[j].nodePos[0] == nextGen[i].nodePos[0] && Chosen[j].nodePos[1] == nextGen[i].nodePos[1])
                        {
                            beenUsed = true;
                        }
                    }
                    //om inte blivit vald innan..
                    if (!beenUsed)
                    {
                        //för varje node som inte använts..
                        for (int j = 0; j < allNodes.Count; j++)
                        {
                            //om den redan finns
                            if (allNodes[j].nodePos[0] == nextGen[i].nodePos[0] && allNodes[j].nodePos[1] == nextGen[i].nodePos[1])
                            {
                                beenUsed = true;
                            }
                        }
                        //annars om det är en helt ny node
                        if (!beenUsed)
                        {
                            //gcost är en större för varje generation
                            nextGen[i].Gcost = g;
                            //hcost är skillnaden i X + skillnaden i Y
                            nextGen[i].Hcost = (Math.Abs(nextGen[i].goalPos[0] - nextGen[i].nodePos[0])) / 2 + Math.Abs(nextGen[i].goalPos[1] - nextGen[i].nodePos[1]);
                            //Fcost är G + H
                            nextGen[i].Fcost = nextGen[i].Gcost + nextGen[i].Hcost;
                            //lägg till papanoden
                            nextGen[i].Dad = bestNode;

                            allNodes.Add(nextGen[i]);
                        }
                        //om positionen använts innan men den nya poängen är större
                        else if (g + nextGen[i].Hcost < nextGen[i].Fcost)
                        {
                            //uppdatera gCost
                            nextGen[i].Gcost = g;
                            //uppdatera Fcost
                            nextGen[i].Fcost = g + nextGen[i].Hcost;
                            //uppdtare pappa
                            nextGen[i].Dad = bestNode;
                        }
                    }
                }
                //om vi hittade väg till spelaren måste startint var 1
                startInt = 1;
            }

            //final är den sista noden som skapats eftersom while loopen bryts om vi hittat spelaren
            final = Chosen.Last();
            
            //om det inte finns än väg till spelaren
            if (allNodes.Count == 0)
            {
                //för varje node osm blivit vald tidigare..
                for (int i = startInt; i < Chosen.Count; i++)
                {
                    //hitta positionen med bäst Fcost om AI kan lägga bomb
                    if (Chosen[i].Hcost < final.Hcost && layBomb)
                    {
                        final = Chosen[i];
                    }
                    //om den bästa positionen är startpositionen så sätter vi bästa positionen till startpositionen
                    if (Adam.Hcost < final.Hcost && layBomb || Adam.Hcost > final.Hcost && !layBomb)
                    {
                        final = Adam;
                    }

                }
                //eftersom det inte finns en väg till spelaren måste vi bomba
                shouldBomb = true;
            }
            else
            {
                //om vi är i spelaren måste vi bomba
                if(XPosition == targetPosition[0] && YPosition == targetPosition[1])
                {
                    shouldBomb = true;
                }
                //annars ska vi inte bomba
                else
                {
                    shouldBomb = false;
                }
            }
            //om vi har lagt en bomb ska AI springa iväg
            if (!layBomb)
            {
                for (int i = startInt; i < Chosen.Count; i++)
                {
                    //hitta positionen längst bort
                    if (Chosen[i].Hcost > final.Hcost)
                    {
                        final = Chosen[i];

                    }
                }
            }

            //lista med positioner som vi skickar vidare
            List<int[]> TemporaryList = new List<int[]>();

            //sålänge noden som används inte är Adam
            while (final.Dad != null)
            {
                //lägg till noden i fråga
                TemporaryList.Add(final.nodePos);
                //använd pappan nästa gång
                final = final.Dad;
            }
            //omvänd listan
            TemporaryList.Reverse();
            //returnera lista med positioner
            return TemporaryList;
            
        }

        //bool som håller koll på om AI kan lägga bomber
        public bool layBomb = true;

        //används en gång
        bool temp = false;
        
        //sätt position
        public Ai(int xpoz, int ypoz)
        {
            XPosition = xpoz;
            YPosition = ypoz;
        }

        //bomber kan sättas ut igen
        public override void Action1()
        {
            layBomb = true;
        }

        //rita AI
        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("██");
        }

        public override void Update()
        {
            //körs en gång
            if (!temp)
            {
                temp = true;

                //starta en loop av AIs Action2 vilket är movement
                int index = TimerClass.GetCharIndex(XPosition, YPosition);
                TimerClass.AddTimer(index, 500, 500, 10000, Program.mygame.Characters[index].Action2);
            }
        }
        
        public override void Action2()
        {
            //om det finns positioner att gå till
            if (posList.Count != 0)
            {
                //spara gamla positionen
                int oldX = XPosition;
                int oldY = YPosition;

                //uppdatera position
                XPosition = posList[0][0];
                YPosition = posList[0][1];

                //rita ut
                Draw(0, 0);
                
                //ta bort gamla position
                Delete(oldX, oldY);
            }
            //om AI ska bomba
            else if (shouldBomb && layBomb)
            {
                //ropa på "sätt ut bomb action"
                Action3();

                //så att AI inte spammar bomber
                layBomb = false;
                
                //ropa på Action1 efter 3500 millisekunder för att sätta laybomb till true
                int index = TimerClass.GetCharIndex(XPosition, YPosition);
                
                TimerClass.AddTimer(index, 6000, 6000, 1, Program.mygame.Characters[index].Action1);
            }
            //hitta bästa rutten
            posList = FindPath(new int[] { Program.mygame.player.XPosition, Program.mygame.player.YPosition });
            
        }

        public override void Action3()
        {
            //lägg ut en bomb
            Move.LayBomb(XPosition, YPosition);
        }
    }

    class Node : GameObject
    {
        //Gcost är avståndet från startpunkten
        public int Gcost;
        //Hcost är avståndet från målet
        public int Hcost;
        //Fcost är Gcost + Hcost
        public int Fcost;
        //håller pappan
        public Node Dad;

        //målpositionen
        public int[] goalPos = new int[2];
        
        //håller position
        public int[] nodePos = new int[2];

        //sätt värden
        public Node(int[] goalPosition, int[] nodePosition)
        {
            goalPos = goalPosition;
            nodePos = nodePosition;
        }









        public override void Action1()
        {
            throw new NotImplementedException();
        }

        public override void Action2()
        {
            throw new NotImplementedException();
        }

        public override void Action3()
        { 
            TimerClass.AddTimer(0, 5000, 0, 1, Action2);
            Move.LayBomb(XPosition, YPosition);
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
    
