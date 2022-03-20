using System;
using System.Collections.Generic;

namespace project.Src.MCTS
{
    public class Node
    {
        const int boardSize = GameManager.BoardSize;

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

            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (board[x,y] == 0) possibilities.Add(i);
                    i++;
                }
            }

            possibilities.ForEach(x => children[x] = new Node(toPlay * -1));

        }



        internal Node SelectChild(out int action)
        {
            Random random = new Random();

            var node = children[random.Next(children.Length)];
            while (node is null)
            {
                node = children[random.Next(children.Length)];
            }

            action = Array.IndexOf(children, node);
            return node;
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
