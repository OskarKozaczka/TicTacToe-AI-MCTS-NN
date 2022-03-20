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

            if (CheckForWinner(Board, move, 1))
                {
                RunEndGame();
                return "You won!";
                }

            if (CheckForDraw(Board))
            {
                RunEndGame();
                return "Draw";
            }

            var AImoveInt = MakeAIMove(out Move AIMove);

            if (CheckForWinner(Board, AIMove, -1))
            {
                RunEndGame();
                return "You Lost :(";
            }
            return AImoveInt;

            if (CheckForDraw(Board))
            {
                RunEndGame();
                return "Draw";
            }

            void RunEndGame(){

            Thread thread = new(EndGame);
            thread.Start();
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

        public static bool CheckForWinner(int[,] Board, Move move, int symbol)
        {
            //var horizontal = 1;
            //for (int i = 1; i <= move.X; i++) if (Board[move.Y, move.X - i] == symbol) horizontal++; else break; //left
            //for (int i = 1; i < BoardSize-move.X; i++) if (Board[move.Y, move.X + i] == symbol) horizontal++; else break; //right
            //if(horizontal >= SymbolsInRow) return true;

            //var vertical = 1;
            //for (int i = 1; i <= move.Y; i++) if (Board[move.Y - i, move.X ] == symbol) vertical++; else break; //up
            //for (int i = 1; i < BoardSize - move.Y; i++) if (Board[move.Y + i, move.X ] == symbol) vertical++; else break; //down
            //if (vertical >= SymbolsInRow) return true;

            //var diagonal1 = 1;
            //for (int i = 1; i <= Min(move.X, move.Y); i++) if (Board[move.Y - i, move.X - i] == symbol) diagonal1++; else break; //up and left
            //for (int i = 1; i < Min(BoardSize - move.X, BoardSize - move.Y); i++) if (Board[move.Y + i, move.X + i] == symbol) diagonal1++; else break; // down and right
            //if (diagonal1 >= SymbolsInRow) return true;

            //var diagonal2 = 1;
            //for (int i = 1; i <= Min(move.X, BoardSize - move.Y -1); i++) if (Board[move.Y + i, move.X - i] == symbol) diagonal2++; else break; //down and left
            //for (int i = 1; i <= Min(BoardSize - move.X -1, move.Y); i++) if (Board[move.Y - i, move.X + i] == symbol) diagonal2++; else break; //up and right
            //if (diagonal2 >= SymbolsInRow) return true;

            //return false;

            
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


        public void WriteMoveToJournal(Move move)
        {
            File.AppendAllText($"data/journal/{GameID}.txt", JsonConvert.SerializeObject(Board) + ";" + JsonConvert.SerializeObject(move) + "\n");
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
            var root = _mcts.Run(board,1, 200000);
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

        public void EndGame()
        {
          //  AI.ConsumeMovesFromJournal(GameID);
          //  DataManager.MoveGameToDB(GameID);
            GameManager.DisposeGame(GameID);
          //  AI.LoadModel();
        }
    }
}
