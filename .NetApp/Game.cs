using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace project
{
    public class Game
    {
        public string GameID { get; set; }

        public int[][] Board { get; set; }

        public Game(string GameID)
        {
            this.GameID = GameID;
        }

        public static String CreateNewGame()
        {
            Random rnd = new Random();
            var Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(Chars[rnd.Next() % 61]);
            }
            var GameID = sb.ToString();

            return GameID;
        }
    }
}
