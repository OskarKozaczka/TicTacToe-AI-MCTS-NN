using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace project
{
    public class GameManager
    {

        public const int BoardSize = 5;
        public const int SymbolsInRow = 4;

        private static readonly Dictionary<string, Game> GamesDict = new();

        public static string CreateNewGame()
        {
            var GameID = RandomStringGenerator();

            try
            {
                GamesDict.Add(GameID, new Game(GameID));
            }
            catch
            {
                throw new GameAlreadyExistsException();
            }

            return GameID;
        }


        public static string RandomStringGenerator()
        {
            const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random rnd = new();
            StringBuilder sb = new();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(Chars[rnd.Next(0,61)]);
            }

            return sb.ToString();
        }

        public static void DisposeGame(string gameID)
        {
            GamesDict.Remove(gameID);
        }

        public static string GetBoard(string id)
        {
            return JsonConvert.SerializeObject(GamesDict[id].GetBoard());
        }

        public static object GetMove(string id, Move value)
        {
            return GamesDict[id].MakeMove(value);
        }
    }
}
