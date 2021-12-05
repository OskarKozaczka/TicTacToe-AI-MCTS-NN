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
            var random = new Random();
            this.GameID = GameID;
            Board = new int[10,10];
            Array.Clear(Board, 0, Board.Length);
            var move = AIModel.GetMove(Board);
            if (random.Next(1,3) == 1) Board[random.Next(0,9), random.Next(0, 9)] = -1;
        }

        public object MakeMove(Move move)
        {
            WriteMoveToJournal(move);
            if (Board[move.y, move.x] == 0) Board[move.y, move.x] = 1; else throw new InvalidMoveException();

            var AIMove = new Move();
            var AImoveInt = GetAIMove();
            AIMove.y = AImoveInt / 10;
            AIMove.x = AImoveInt % 10;

            if (CheckForWinner(move, 1) || CheckForWinner(AIMove , -1)) 
            {
                Thread thread = new(EndGame);
                thread.Start();
                return "game is over";
            }
            return AImoveInt;
        }

        private bool CheckForWinner(Move move, int symbol)
        {
            var horizontal = 1;
            for (int i = 1; i < move.x; i++) if (Board[move.y, move.x - i] == symbol) horizontal++; else break; //left
            for (int i = 1; i < 10-move.x; i++) if (Board[move.y, move.x + i] == symbol) horizontal++; else break; //right
            if(horizontal == 5) return true;

            var vertical = 1;
            for (int i = 1; i <= move.y; i++) if (Board[move.y - i, move.x ] == symbol) vertical++; else break; //up
            for (int i = 1; i < 10 - move.y; i++) if (Board[move.y + i, move.x ] == symbol) vertical++; else break; //down
            if (vertical == 5) return true;

            var diagonal1 = 1;
            for (int i = 1; i < Min(move.x,move.y); i++) if (Board[move.y - i, move.x - i] == symbol) diagonal1++; else break; //up and left
            for (int i = 1; i < Min(10-move.x, 10-move.y); i++) if (Board[move.y + i, move.x + i] == symbol) diagonal1++; else break; // down and right
            if (diagonal1 == 5) return true;

            var diagonal2 = 1;
            for (int i = 1; i < Min(move.x, 10 - move.y); i++) if (Board[move.y + i, move.x - i] == symbol) diagonal2++; else break; //down and left
            for (int i = 1; i < Min(10 - move.x, move.y); i++) if (Board[move.y - i, move.x + i] == symbol) diagonal2++; else break; //up and right
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
