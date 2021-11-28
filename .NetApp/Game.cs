using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;


namespace project
{
    public class Game
    {
        public string GameID { get; set; }

        public int[,] Board { get; set; }

        public Game(string GameID)
        {
            this.GameID = GameID;
            Board = new int[10,10];
            Array.Clear(Board,0,Board.Length);
            
        }

        public object MakeMove(Move move)
        {
            WriteMoveToJournal(move);
            if (Board[move.y, move.x] == 0) Board[move.y, move.x] = 1; else throw new InvalidMoveException();
            if (CheckForWinner(move)) return "game is over";
            return GetAIMove();
        }

        private bool CheckForWinner(Move move)
        {
            var horizontal = 1;
            for (int i = 1; i < move.x; i++) if (Board[move.y, move.x - i] == 1) horizontal++; else break; //left
            for (int i = 1; i < 10-move.x; i++) if (Board[move.y, move.x + i] == 1) horizontal++; else break; //right
            if(horizontal == 5) return true;

            var vertical = 1;
            for (int i = 1; i < move.y; i++) if (Board[move.y - i, move.x ] == 1) vertical++; else break; //up
            for (int i = 1; i < 10 - move.y; i++) if (Board[move.y + i, move.x ] == 1) vertical++; else break; //down
            if (vertical == 5) return true;

            var diagonal1 = 1;
            for (int i = 1; i <= Math.Min(move.x,move.y); i++) if (Board[move.y - i, move.x - i] == 1) diagonal1++; else break; //up and left
            for (int i = 1; i < Math.Min(10-move.x, 10-move.y); i++) if (Board[move.y + i, move.x + i] == 1) diagonal1++; else break; // down and right
            if (diagonal1 == 5) return true;

            var diagonal2 = 1;
            for (int i = 1; i <= Math.Min(move.x, 10 - move.y); i++) if (Board[move.y + i, move.x - i] == 1) diagonal2++; else break; //down and left
            for (int i = 1; i < Math.Min(10 - move.x, move.y); i++) if (Board[move.y - i, move.x + i] == 1) diagonal2++; else break; //up and right
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
    }
}
