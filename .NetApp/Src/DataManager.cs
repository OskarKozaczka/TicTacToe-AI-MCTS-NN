using Newtonsoft.Json;
using Numpy;
using System;
using System.Collections.Generic;
using System.IO;

namespace project
{
    public class DataManager
    {

        private readonly static int BoardSize = GameManager.BoardSize;

        internal static TraningData ReadAndPrepareData(string gameID)
        {
            List<NDarray> features = new(), labels = new();
            var emptyArray = new int[BoardSize, BoardSize]; Array.Clear(emptyArray, 0, emptyArray.Length);

            try
            {
                foreach (var line in File.ReadAllLines($"data/journal/{gameID}.txt"))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string board = line.Split(';')[0], move = line.Split(';')[1];
                        features.Add(np.array(JsonConvert.DeserializeObject<int[,]>(board)));
                        labels.Add(np.array(MoveToArray(JsonConvert.DeserializeObject<Move>(move))).reshape(BoardSize * BoardSize));
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

        static int[,] MoveToArray(Move move)
        {
            var emptyTable = new int[BoardSize, BoardSize]; Array.Clear(emptyTable, 0, emptyTable.Length);
            var copy = emptyTable.Clone() as int[,];
            copy[move.Y, move.X] = 1;
            return copy;
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
    }
}
