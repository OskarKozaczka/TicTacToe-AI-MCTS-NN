using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project
{
    public class GameModel:IGame
    {
        public string GameID { get; set; }

        public int[][] Board { get; set; }

        public GameModel(string GameID)
        {
            this.GameID = GameID;
        }

    }
}
