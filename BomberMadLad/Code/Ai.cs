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
        //håller alla möjliga vägar.

        public bool shouldBomb = true;

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
            
            {
                while (allNodes.Count > 0)
                {
                    //välj ny bestnode
                    int LowestScore = allNodes.Min(x => x.Fcost);
                    bestNode = allNodes.First(x => x.Fcost == LowestScore);

                    Chosen.Add(bestNode);

                    allNodes.Remove(bestNode);

                    if (bestNode.nodePos[0] == targetPosition[0] && bestNode.nodePos[1] == targetPosition[1])
                    {
                        break;

                    }

                    List<Node> kids = new List<Node>();

                    if (CollisionCheck(bestNode.nodePos[0] - 2, bestNode.nodePos[1]))
                    {
                        kids.Add(new Node(bestNode.goalPos, new int[] { bestNode.nodePos[0] - 2, bestNode.nodePos[1] }));

                    }
                    if (CollisionCheck(bestNode.nodePos[0] + 2, bestNode.nodePos[1]))
                    {
                        kids.Add(new Node(bestNode.goalPos, new int[] { bestNode.nodePos[0] + 2, bestNode.nodePos[1] }));

                    }
                    if (CollisionCheck(bestNode.nodePos[0], bestNode.nodePos[1] + 1))
                    {
                        kids.Add(new Node(bestNode.goalPos, new int[] { bestNode.nodePos[0], bestNode.nodePos[1] + 1 }));

                    }
                    if (CollisionCheck(bestNode.nodePos[0], bestNode.nodePos[1] - 1))
                    {
                        kids.Add(new Node(bestNode.goalPos, new int[] { bestNode.nodePos[0], bestNode.nodePos[1] - 1 }));

                    }


                    List<Node> nextGen = kids;


                    g++;

                    for (int i = 0; i < nextGen.Count; i++)
                    {
                        bool beenUsed = false;
                        for (int j = 0; j < Chosen.Count; j++)
                        {
                            if (Chosen[j].nodePos[0] == nextGen[i].nodePos[0] && Chosen[j].nodePos[1] == nextGen[i].nodePos[1])
                            {
                                beenUsed = true;
                            }
                        }
                        if (!beenUsed)
                        {
                            for (int j = 0; j < allNodes.Count; j++)
                            {
                                if (allNodes[j].nodePos[0] == nextGen[i].nodePos[0] && allNodes[j].nodePos[1] == nextGen[i].nodePos[1])
                                {
                                    beenUsed = true;
                                }
                            }
                            if (!beenUsed)
                            {
                                nextGen[i].Gcost = g;
                                nextGen[i].Hcost = (Math.Abs(nextGen[i].goalPos[0] - nextGen[i].nodePos[0])) / 2 + Math.Abs(nextGen[i].goalPos[1] - nextGen[i].nodePos[1]);
                                nextGen[i].Fcost = nextGen[i].Gcost + nextGen[i].Hcost;
                                nextGen[i].Dad = bestNode;

                                allNodes.Add(nextGen[i]);
                            }
                            else if (g + nextGen[i].Hcost < nextGen[i].Fcost)
                            {
                                nextGen[i].Gcost = g;
                                nextGen[i].Fcost = g + nextGen[i].Hcost;
                                nextGen[i].Dad = bestNode;
                            }
                        }
                    }
                }
                startInt = 1;
            }

            final = Chosen.Last();
            
            if (allNodes.Count == 0)
            {
                for (int i = startInt; i < Chosen.Count; i++)
                {
                    if (Chosen[i].Hcost < final.Hcost && layBomb || Chosen[i].Hcost > final.Hcost && !layBomb)
                    {
                        final = Chosen[i];
                    }
                    if (Adam.Hcost < final.Hcost && layBomb || Adam.Hcost > final.Hcost && !layBomb)
                    {
                        final = Adam;
                    }

                }
                shouldBomb = true;
            }
            else
            {
                shouldBomb = false;
            }
            
            List<int[]> TemporaryList = new List<int[]>();

            while (final.Dad != null)
            {
                TemporaryList.Add(final.nodePos);
                final = final.Dad;
            }
            

            TemporaryList.Reverse();
            
           
            
            
            return TemporaryList;
            
        }

        public bool layBomb = true;

        bool temp = false;

        public List<int[]> bombPoints = new List<int[]>();

        Random rng = new Random();
        Move control = new Move();

        public Ai(int xpoz, int ypoz)
        {
            XPosition = xpoz;
            YPosition = ypoz;
        }

        public override void Action1()
        {

            layBomb = true;

        }


        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("██");
        }

        public override void Update()
        {
            if (!temp)
            {
                
                temp = true;

                int index = TimerClass.GetCharIndex(XPosition, YPosition);

                TimerClass.AddTimer(index, 500, 500, 10000, Program.mygame.Characters[index].Action2);
            }
        }
        
        public override void Action2()
        {
            if (posList.Count != 0)
            {
               
                int oldX = XPosition;
                int oldY = YPosition;

                XPosition = posList[0][0];
                YPosition = posList[0][1];

                Draw(0, 0);

                Delete(oldX, oldY);
            }
            else if (shouldBomb && layBomb)
            {
                Action3();

                layBomb = false;

                int index = TimerClass.GetCharIndex(XPosition, YPosition);

                TimerClass.AddTimer(index, 3000, 3000, 1, Program.mygame.Characters[index].Action1);


            }

            posList = FindPath(new int[] { Program.mygame.player.XPosition, Program.mygame.player.YPosition });
            
        }

        public override void Action3()
        {
            control.LayBomb(XPosition, YPosition);
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

        public Node Dad;

        public bool beenUsed = false;

        //håller alla möjliga vägar.
        

       

        public int[] goalPos = new int[2];

        int[] startPos = new int[2];

        public int[] nodePos = new int[2];
        //programet väljer den rutan av åtta som har lägst Fcost, om det finns flera likadana så tar den den med minst Hcost.
        

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
            throw new NotImplementedException();
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
    
