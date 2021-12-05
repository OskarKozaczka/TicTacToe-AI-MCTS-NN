using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using static System.Math;

namespace project
{
    public class Game
    {
        public string GameID { get; set; }

        public int[,] Board { get; set; }

        public Game(string GameID)
        {
            this.GameID = GameID;
            Board = new int[10, 10];
            Array.Clear(Board, 0, Board.Length);

        }

        public object MakeMove(Move move)
        {
            WriteMoveToJournal(move);
            if (Board[move.Y, move.X] == 0) Board[move.Y, move.X] = 1; else throw new InvalidMoveException();
            if (CheckForWinner(move))
            {
                Thread thread = new(EndGame);
                thread.Start();
                return "game is over";
            }
            return GetAIMove();
        }

        private bool CheckForWinner(Move move)
        {
            var horizontal = 1;
            for (int i = 1; i < move.X; i++) if (Board[move.Y, move.X - i] == 1) horizontal++; else break; //left
            for (int i = 1; i < 10 - move.X; i++) if (Board[move.Y, move.X + i] == 1) horizontal++; else break; //right
            if (horizontal == 5) return true;

            var vertical = 1;
            for (int i = 1; i <= move.Y; i++) if (Board[move.Y - i, move.X] == 1) vertical++; else break; //up
            for (int i = 1; i < 10 - move.Y; i++) if (Board[move.Y + i, move.X] == 1) vertical++; else break; //down
            if (vertical == 5) return true;

            var diagonal1 = 1;
            for (int i = 1; i <= Min(move.X, move.Y); i++) if (Board[move.Y - i, move.X - i] == 1) diagonal1++; else break; //up and left
            for (int i = 1; i < Min(10 - move.X, 10 - move.Y); i++) if (Board[move.Y + i, move.X + i] == 1) diagonal1++; else break; // down and right
            if (diagonal1 == 5) return true;

            var diagonal2 = 1;
            for (int i = 1; i <= Min(move.X, 10 - move.Y); i++) if (Board[move.Y + i, move.X - i] == 1) diagonal2++; else break; //down and left
            for (int i = 1; i < Min(10 - move.X, move.Y); i++) if (Board[move.Y - i, move.X + i] == 1) diagonal2++; else break; //up and right
            if (diagonal2 == 5) return true;

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
            var AImove = AIModel.GetMove(Board.Clone() as int[,]);
            var AImoveS = AImove.ToString();
            if (AImoveS.Length == 1) AImoveS = "0" + AImoveS;
            Board[(int)char.GetNumericValue(AImoveS[0]), (int)char.GetNumericValue(AImoveS[1])] = -1;
            return AImove;
        }

        public void EndGame()
        {
            AIModel.ConsumeMovesFromJournal(GameID);
            DataManager.MoveGameToDB(GameID);
            GameManager.DisposeGame(GameID);
            AIModel.LoadModel();
        }
    }
}
