﻿using Newtonsoft.Json;
using project.Src.MCTS;
using System;
using System.IO;
using System.Threading;
using static System.Math;
using project.Models;

namespace project
{
    public class Game
    {
        public const int BoardSize  = GameManager.BoardSize;  
        private const int SymbolsInRow = GameManager.SymbolsInRow;

        int simNum { get; set; }
        public string GameID { get; set; }
        public int[,] Board { get; set; }

        public Game(string GameID, int simNum = 3000)
        {
            this.simNum = simNum;
            var random = new Random();
            this.GameID = GameID;
            Board = new int[BoardSize, BoardSize];
            Array.Clear(Board, 0, Board.Length);
            if (random.Next(0, 2) == 0) 
            {
                MakeAIMove(out _);
                DataManager.WriteMoveToJournal(Board, GameID);
            }
        }

        public MoveResponseModel MakeMove(Move move)
        {
            var moveResponse = new MoveResponseModel();

            if (Board[move.Y, move.X] == 0) Board[move.Y, move.X] = 1; else throw new InvalidMoveException();
            DataManager.WriteMoveToJournal(Board, GameID);

            CheckForGameEnd(ref moveResponse);

            if (moveResponse.GameStateMessage != "") return moveResponse;

            moveResponse.MoveID = MakeAIMove(out Move AIMove);
            DataManager.WriteMoveToJournal(Board, GameID);

            CheckForGameEnd(ref moveResponse);

            return moveResponse;

            void CheckForGameEnd(ref MoveResponseModel moveResponse)
            {
                if (CheckForWinner(Board, 1))
                {
                    EndGame(1);
                    moveResponse.GameStateMessage = "You won!";
                }

                if (CheckForDraw(Board))
                {
                    EndGame(0);
                    moveResponse.GameStateMessage = "Draw";
                }

                if (CheckForWinner(Board, -1))
                {
                    EndGame(-1);
                    moveResponse.GameStateMessage = "You Lost :(";
                }
            }
        }

        public int MakeAIMove(out Move AIMove)
        {
            AIMove = new Move();
            var AImoveInt = GetAIMove();

            AIMove.Y = AImoveInt / BoardSize;
            AIMove.X = AImoveInt % BoardSize;

            if (Board[AIMove.Y, AIMove.X] == 0) Board[AIMove.Y, AIMove.X] = -1; else throw new InvalidMoveException();
            return AImoveInt;
        }

        public static bool CheckForWinner(int[,] Board, int symbol)
        {
            for (int y = 0; y < BoardSize; y++)
            {
                var symbolCount = 0;
                for (int x = 0; x < BoardSize; x++)
                {
                    if (Board[y, x] == symbol) symbolCount++; else symbolCount = 0;
                    if (symbolCount >= SymbolsInRow) return true;
                }
            }

            for (int x = 0; x < BoardSize; x++)
            {
                var symbolCount = 0;
                for (int y = 0; y < BoardSize; y++)
                {
                    if (Board[y, x] == symbol) symbolCount++; else symbolCount = 0;
                    if (symbolCount >= SymbolsInRow) return true;
                }
            }

            for (int x = 0; x <= BoardSize-SymbolsInRow; x++)
            {
                for (int y = 0; y <= BoardSize - SymbolsInRow; y++)
                {
                    var symbolCount = 0;
                    for (int i = 0; i < BoardSize-x-y; i++)
                    {
                        if (Board[y+i, x+i] == symbol) symbolCount++; else symbolCount = 0;
                        if (symbolCount >= SymbolsInRow) return true;
                    }

                }
            }

            for (int x = 0; x <= BoardSize - SymbolsInRow; x++)
            {
                for (int y = BoardSize-1; y >= SymbolsInRow - 1; y--)
                {
                    var symbolCount = 0;
                    for (int i = 0; i < BoardSize - x - (BoardSize - y -1); i++)
                    {
                        if (Board[y - i, x + i] == symbol) symbolCount++; else symbolCount = 0;
                        if (symbolCount >= SymbolsInRow) return true;
                    }

                }
            }

            return false;
        }

        public static bool CheckForDraw(int[,] Board)
        {
            for (int y = 0; y < BoardSize; y++)
                for (int x = 0; x < BoardSize; x++)
                {
                    if (Board[y, x] == 0) return false;
                }
            return true;
        }

        public int[,] GetBoard()
        {
            return Board;
        }

        public int GetAIMove(bool useNetwork = true)
        {
            //return AI.GetMove(Board.Clone() as int[,]);
            var _mcts = new MCTS(useNetwork);
            var board = Board.Clone() as int[,];

            for (int y = 0; y < BoardSize; y++)
                for (int x = 0; x < BoardSize; x++)
                {
                    board[y, x] = board[y, x] * -1;
                }

            var root = _mcts.Run(board,1, simNum);
            //Console.WriteLine("Number of Simulations: " + root.visitCount);
            var bestValue = -2f;
            var bestMove = -1;
            foreach (var child in root.children)
            {
                if (child is not null && child.visitCount> bestValue)
                {
                    bestValue = child.visitCount;
                    bestMove = Array.IndexOf(root.children, child);
                }
            }
            return bestMove;
        }

        public void EndGame(object Winner)
        {
            DataManager.UpdateGameResult(GameID, (int)Winner);
            DataManager.MoveGameToDB(GameID);
            GameManager.DisposeGame(GameID);
            ValueNetwork.LoadModel();
        }
    }
}
