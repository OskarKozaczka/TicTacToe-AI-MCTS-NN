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

        public void MakeMove(MoveModel move)
        {
            Board[move.y, move.x] = 1;
            File.AppendAllText($"data/journal/{GameID}.csv", JsonConvert.SerializeObject(Board) +";"+ JsonConvert.SerializeObject(move));
        }

        public int[,] GetBoard()
        {
            return Board;
        }
    }
}
