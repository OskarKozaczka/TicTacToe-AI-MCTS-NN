using System;
using System.Collections.Generic;

namespace project.Src.MCTS
{
    public class Node
    {
        internal int[,] board { get; set; }
        internal float valueSum { get; set; }
        internal int visitCount { get; set; }
        internal float value { get { return valueSum / visitCount; } }
        internal Node[] children { get; set; }
        internal int toPlay { get; set; }
        internal bool isExpanded { get; set; }


    public Node(int toPlay)
        {
            this.toPlay = toPlay;
            this.isExpanded = false;
            this.children = new Node[25];
        }


        internal void Expand(int[,] board, int toPlay)
        {
            this.isExpanded = true;
            this.toPlay = toPlay;
            this.board = board;

            var possibilities = new List<int>();

            var i = 0;

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (board[x,y] == 0) possibilities.Add(i);
                    i++;
                }
            }

            possibilities.ForEach(x => children[x] = new Node(toPlay * -1));

        }



        internal int SelectChild()
        {
            Random random = new Random();

            var i = random.Next(children.Length);
            while (children[i] is null)
            {
                i = random.Next(children.Length);
            }
            return i;
        }

        internal void BackPropagate(List<Node> path, float value,int toPlay)
        {
            path.Reverse();

            foreach (var node in path)
            {
                node.valueSum += toPlay == node.toPlay ? value : -value;
                node.visitCount++;
            }
        }
    }
}
