using Newtonsoft.Json;
using project.Src.MCTS;
using System;
using System.IO;
using System.Threading;
using static System.Math;

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
            if (random.Next(0, 2) == 0) MakeAIMove(out _);
        }

        public object MakeMove(Move move)
        {
            WriteMoveToJournal(move);
            if (Board[move.Y, move.X] == 0) Board[move.Y, move.X] = 1; else throw new InvalidMoveException();
            var AImoveInt = MakeAIMove(out Move AIMove);

            if (CheckForWinner(Board, move, 1) || CheckForWinner(Board, AIMove, -1))
            {
                Thread thread = new(EndGame);
                thread.Start();
                return "game is over";
            }
            return AImoveInt;
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

        public static bool CheckForWinner(int[,] Board, Move move, int symbol)
        {
            var horizontal = 1;
            for (int i = 1; i < move.X; i++) if (Board[move.Y, move.X - i] == symbol) horizontal++; else break; //left
            for (int i = 1; i < BoardSize-move.X; i++) if (Board[move.Y, move.X + i] == symbol) horizontal++; else break; //right
            if(horizontal == SymbolsInRow) return true;

            var vertical = 1;
            for (int i = 1; i <= move.Y; i++) if (Board[move.Y - i, move.X ] == symbol) vertical++; else break; //up
            for (int i = 1; i < BoardSize - move.Y; i++) if (Board[move.Y + i, move.X ] == symbol) vertical++; else break; //down
            if (vertical == SymbolsInRow) return true;

            var diagonal1 = 1;
            for (int i = 1; i < Min(move.X,move.Y); i++) if (Board[move.Y - i, move.X - i] == symbol) diagonal1++; else break; //up and left
            for (int i = 1; i < Min(BoardSize-move.X, BoardSize-move.Y); i++) if (Board[move.Y + i, move.X + i] == symbol) diagonal1++; else break; // down and right
            if (diagonal1 == SymbolsInRow) return true;

            var diagonal2 = 1;
            for (int i = 1; i < Min(move.X, BoardSize - move.Y); i++) if (Board[move.Y + i, move.X - i] == symbol) diagonal2++; else break; //down and left
            for (int i = 1; i < Min(BoardSize - move.X, move.Y); i++) if (Board[move.Y - i, move.X + i] == symbol) diagonal2++; else break; //up and right
            if (diagonal2 == SymbolsInRow) return true;

            return false;
        }

        public int[,] GetBoard()
        {
            return Board;
        }


        public void WriteMoveToJournal(Move move)
        {
            File.AppendAllText($"data/journal/{GameID}.txt", JsonConvert.SerializeObject(Board) + ";" + JsonConvert.SerializeObject(move) + "\n");
        }

        public int GetAIMove()
        {
            //return AI.GetMove(Board.Clone() as int[,]);
            var _mcts = new mcts();
            var root = _mcts.Run(Board.Clone() as int[,],-1, 2000);
            var bestValue = 0;
            var bestMove = 0;
            foreach (var child in root.children)
            {
                if (child is not null && child.value > bestValue)
                {
                    bestValue = (int)child.value;
                    bestMove = Array.IndexOf(root.children, child);
                }
            }
            return bestMove;
        }

        public void EndGame()
        {
            AI.ConsumeMovesFromJournal(GameID);
            DataManager.MoveGameToDB(GameID);
            GameManager.DisposeGame(GameID);
            AI.LoadModel();
        }
    }
}
