﻿using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace project.Src.MCTS
{
    public class MCTS
    {
        const int boardSize = GameManager.BoardSize;

        private bool useNetowork;

        public MCTS(bool useNetowork = false)
        {
            this.useNetowork = useNetowork;
        }

        public Node Run(int[,] Board, int toPlay, int MaxTime)
        {
            var root = new Node(toPlay);
            root.Expand(Board, toPlay);
            var stopwatch = Stopwatch.StartNew();
            while(stopwatch.ElapsedMilliseconds <= MaxTime)
            {
                var node = root;
                var path = new List<Node> { node };

                var action = 0;

                while(node.isExpanded)
                {
                    node = node.SelectChild(out action);
                    path.Add(node);
                }
                var parent = path[^2];
                var board = parent.board;
                var nextBoard = board.Clone() as int[,];
                nextBoard[action / boardSize , action % boardSize] = 1;

                for (int y = 0; y < boardSize; y++)
                    for (int x = 0; x < boardSize; x++)
                    {
                        nextBoard[y, x] = nextBoard[y, x] * -1;
                    }

                float ?value = 0f;
                var Move = new Move();
                Move.X = action % boardSize;
                Move.Y = action / boardSize;
                if (Game.CheckForWinner(nextBoard, 1)) value = -1;
                else if (Game.CheckForWinner(nextBoard, -1)) value = 1;
                else if (Game.CheckForDraw(nextBoard)) value = 0;
                else value = null;

                if (value is null)
                {
                    value = useNetowork ? -ValueNetwork.MakePrediction(nextBoard)/10 : 0;
                    //Console.WriteLine(timer.ElapsedMilliseconds);
                    node.Expand(nextBoard, -parent.toPlay);
                }
                node.BackPropagate(path, value.Value, parent.toPlay * -1);
            }
            stopwatch.Stop();
            return root;

        }


    }
}
