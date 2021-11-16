using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace project
{
    public class GameManager
    {

        public static Dictionary<string,Game> GamesDict = new();

        public static string CreateNewGame()
        {
            Random rnd = new();
            var Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder sb = new();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(Chars[rnd.Next() % 61]);
            }
            var GameID = sb.ToString();

            try
            {
                GamesDict.Add(GameID, new Game(GameID));
            }
            catch
            {
                Console.WriteLine("Game with this ID already exists");
            }

            return GameID;
        }

    }
}
