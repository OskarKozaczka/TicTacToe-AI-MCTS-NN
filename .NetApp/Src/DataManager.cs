using Newtonsoft.Json;
using Numpy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace project
{
    public class DataManager
    {

        private readonly static int boardSize = GameManager.BoardSize;

        internal static TraningData ReadAndPrepareData(string gameID)
        {
            List<NDarray> features = new(), labels = new();
            var emptyArray = new int[boardSize, boardSize]; Array.Clear(emptyArray, 0, emptyArray.Length);

            try
            {
                foreach (var line in File.ReadAllLines($"data/journal/{gameID}.txt"))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var board = JsonConvert.DeserializeObject<int[,]>(line.Split(';')[0]);
                        var result = int.Parse(line.Split(';')[1]);

                        features.Add(np.array(board));
                        labels.Add(np.array(result));

                        //for (int y = 0; y < boardSize; y++)
                        //    for (int x = 0; x < boardSize; x++)
                        //    {
                        //        board[y, x] = board[y, x] * -1;
                        //    }
                        //features.Add(np.array(board));
                        //labels.Add(np.array(-result));

                    }
                }
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("There is no entry with given ID", e);
            }
            catch (Exception e)
            {
                throw new WrongDataException("There was error reading the journal", e);
            }

            TraningData data = new();
            data.features = np.array(features.ToArray());
            data.labels = np.array(labels.ToArray());

            return data;
        }

        internal static TraningData ReadAndPrepareDataFromDB()
        {
            List<NDarray> features = new(), labels = new();
            var emptyArray = new int[boardSize, boardSize]; Array.Clear(emptyArray, 0, emptyArray.Length);

            try
            {
                foreach (var file in Directory.EnumerateFiles($"data/DB"))
                    foreach (var line in File.ReadAllLines(file))
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var board = JsonConvert.DeserializeObject<int[,]>(line.Split(';')[0]);

                            var myBoard = new int[boardSize, boardSize];
                            var enemyBoard = new int[boardSize, boardSize];

                            for (int y = 0; y < boardSize; y++)
                                for (int x = 0; x < boardSize; x++)
                                {
                                    if (board[y, x] == 1) myBoard[y, x] = 1;
                                    else if (board[y, x] == -1) enemyBoard[y, x] = 1;
                                }

                            var result = int.Parse(line.Split(';')[1]);

                            if (result==0) continue;

                            features.Add(np.array(new NDarray[] {myBoard,enemyBoard}));
                            labels.Add(np.array(result));
                        }
                    }
            }
            catch (Exception e) { }
            TraningData data = new();
            data.features = np.array(features.ToArray());
            data.labels = np.array(labels.ToArray());

            return data;
        }

        internal static void WriteMoveToJournal(int[,] Board, string GameID)
        {
            File.AppendAllText($"data/journal/{GameID}.txt", JsonConvert.SerializeObject(Board) + ";\n");
        }

        internal static void UpdateGameResult(string GameID, int result)
        {
            var filePath = $"data/journal/{GameID}.txt";
            var lines = File.ReadAllLines(filePath);
            var newLines = lines.Select(line => line + result.ToString()).ToList();
            File.Delete(filePath);
            File.WriteAllLines(filePath, newLines);
        }

        internal static void MoveGameToDB(string gameID)
        {
            var index = 1;

            while (File.Exists($"data/DB/{gameID}({index}).txt")) index++;
            try
            {
                File.Move($"data/journal/{gameID}.txt", $"data/DB/{gameID}({index}).txt");
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("There is no game with given ID in journal", e);
            }
        }
        private static int[,] MoveToArray(Move move)
        {
            var emptyTable = new int[boardSize, boardSize]; Array.Clear(emptyTable, 0, emptyTable.Length);
            var copy = emptyTable.Clone() as int[,];
            copy[move.Y, move.X] = 1;
            return copy;
        }
    }
}
