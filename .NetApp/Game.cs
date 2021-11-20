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

        public int MakeMove(MoveModel move)
        {
            File.AppendAllText($"data/journal/{GameID}.txt", JsonConvert.SerializeObject(Board) + ";" + JsonConvert.SerializeObject(move) + "\n");
            Board[move.y, move.x] = 1;
            return AIModel.GetMove(Board);
        }

        public int[,] GetBoard()
        {
            return Board;
        }
    }
}
