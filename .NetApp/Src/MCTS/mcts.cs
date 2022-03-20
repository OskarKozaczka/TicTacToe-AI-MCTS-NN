﻿using System.Collections.Generic;

namespace project.Src.MCTS
{
    public class mcts
    {
        const int boardSize = 5;

        public Node Run(int[,] Board, int toPlay, int simNum)
        {
            var root = new Node(toPlay);
            root.Expand(Board, toPlay);

            for (int i = 0; i < simNum; i++)
            {
                var node = root;
                var path = new List<Node> { node };

                var action = 0;

                while(node.isExpanded)
                {
                    action = node.SelectChild();
                    node = node.children[action];
                    path.Add(node);
                }
                var parent = path[^2];
                var board = parent.board;
                var nextBoard = board.Clone() as int[,];
                nextBoard[action / boardSize , action % boardSize] = 1;

                for (int y = 0; y < 5; y++)
                    for (int x = 0; x < 5; x++)
                    {
                        nextBoard[y, x] = nextBoard[y, x] * -1;
                    }

                float ?value = 0f;
                var Move = new Move();
                Move.X = action % boardSize;
                Move.Y = action / boardSize;
                if (Game.CheckForWinner(nextBoard, Move, 1))
                {
                    value = 1;
                }
                else if (Game.CheckForWinner(nextBoard, Move, -1))
                {
                    value = -1;
                }
                else if (Game.CheckForDraw(nextBoard))value = 0;
                else value = null;

                if (value is null)
                {
                    value = 0;
                    node.Expand(nextBoard, parent.toPlay * -1);
                }
                node.BackPropagate(path, value.Value, parent.toPlay * -1);
            }

            return root;

        }


    }
}
