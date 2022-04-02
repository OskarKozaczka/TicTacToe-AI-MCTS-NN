using Newtonsoft.Json;
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

        public string GameID { get; set; }
        public int[,] Board { get; set; }

        public Game(string GameID)
        {
            var random = new Random();
            this.GameID = GameID;
            Board = new int[BoardSize, BoardSize];
            Array.Clear(Board, 0, Board.Length);
           // if (random.Next(0, 2) == 0) MakeAIMove(out _);
        }

        public MoveResponseModel MakeMove(Move move)
        {
            var moveResponse = new MoveResponseModel();

            DataManager.WriteMoveToJournal(Board,GameID);
            if (Board[move.Y, move.X] == 0) Board[move.Y, move.X] = 1; else throw new InvalidMoveException();

            if (CheckForWinner(Board, 1))
                {
                RunEndGame(1);
                moveResponse.GameStateMessage = "You won!";
            }

            if (CheckForDraw(Board))
            {
                RunEndGame(0);
                moveResponse.GameStateMessage = "Draw";
            }

            if (moveResponse.GameStateMessage != "") return moveResponse;

            moveResponse.MoveID = MakeAIMove(out Move AIMove);

            if (CheckForWinner(Board, -1))
            {
                RunEndGame(-1);
                moveResponse.GameStateMessage = "You Lost :(";
            }
            

            if (CheckForDraw(Board))
            {
                RunEndGame(0);
                moveResponse.GameStateMessage = "Draw";
            }

            return moveResponse;

            void RunEndGame(int winner){

            Thread thread = new(EndGame);
            thread.Start(winner);
            }
            
        }

        private int MakeAIMove(out Move AIMove)
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

        public int GetAIMove()
        {
            //return AI.GetMove(Board.Clone() as int[,]);
            var _mcts = new MCTS();
            var board = Board.Clone() as int[,];

            for (int y = 0; y < BoardSize; y++)
                for (int x = 0; x < BoardSize; x++)
                {
                    board[y, x] = board[y, x] * -1;
                }

            var root = _mcts.Run(board,1, 2000);
            Console.WriteLine("Number of Simulations: " + root.visitCount);
            var bestValue = -1f;
            var bestMove = 0;
            foreach (var child in root.children)
            {
                if (child is not null && child.value > bestValue)
                {
                    bestValue = child.value;
                    bestMove = Array.IndexOf(root.children, child);
                }
            }
            return bestMove;
        }

        public void EndGame(object Winner)
        {
            DataManager.UpdateGameResult(GameID, (int)Winner);
            ValueNetwork.ConsumeMovesFromJournal(GameID);
            DataManager.MoveGameToDB(GameID);
            GameManager.DisposeGame(GameID);
        }
    }
}
