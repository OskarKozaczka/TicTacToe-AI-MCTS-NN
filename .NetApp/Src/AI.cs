using Keras;
using Keras.Layers;
using Keras.Models;
using Numpy;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace project
{
    public class AI
    {

        const int batch_size = 100;
        const int epochs = 1;
        const int verbose = 1;
        const int layers = 5;


        private static BaseModel Model;

        public static void CreateModel()
        {
            var model = new Sequential();
            model.Add(new Input(shape: new Shape(5, 5, 1)));
            for (int i = 0; i < layers; i++)
            {
                model.Add(new Conv2D(25, new Tuple<int, int>(3, 3), activation: "tanh", padding: "same"));
                model.Add(new BatchNormalization());
            }
            model.Add(new Flatten());
            model.Add(new Dense(25, activation: "softmax"));
            model.Summary();
            Model = model;
        }

        public static void ConsumeMovesFromJournal(string gameID)
        {
            using (Py.GIL())
            {
                var data = DataManager.ReadAndPrepareData(gameID);
                Model.Fit(data.features, data.labels, batch_size: batch_size, epochs: epochs, verbose: verbose);
                SaveModel();
            }
        }

        public static void SaveModel()
        {
            File.WriteAllText("data/model/model.json", Model.ToJson());
            Model.SaveWeight("data/model/model.h5");
        }

        public static void LoadModel()
        {
            using (Py.GIL())
            {
                try
                {
                    var loadedModel = Sequential.ModelFromJson(File.ReadAllText("data/model/model.json"));
                    loadedModel.LoadWeight("data/model/model.h5");
                    Model = loadedModel;
                }
                catch
                {
                    Console.WriteLine("Model was not found, creating a new one");
                    CreateModel();
                }
                finally
                {
                    Model.Compile(optimizer: "sgd", loss: "categorical_crossentropy", metrics: new string[] { "accuracy" });
                    SaveModel();
                }
            }

        }

        public static int GetMove(int[,] Board)
        {
            for (int y = 0; y < 5; y++)
                for (int x = 0; x < 5; x++)
                {
                    Board[x, y] = Board[x, y] * -1;
                }
            int Move = -1;
            var index = 0;
            while (Move == -1 || Board[Move / 5, Move % 5] != 0)
            {
                Move = AIPredict(Board, index);
                index++;
            }

            return Move;
        }

        private static int AIPredict(int[,] Board, int index)
        {
            using (Py.GIL())
            {
                List<float> resultL;
                NDarray result;

                result = Model.Predict(np.array(Board).reshape(1, 5, 5));
                resultL = result.GetData<float>().ToList();
                var resultSorted = new List<float>(resultL);
                resultSorted.Sort();
                resultSorted.Reverse();
                var AImove = -1;
                try
                {
                    AImove = resultL.IndexOf(resultSorted[index]);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new GameIsOverException();
                }

                return AImove;
            }

        }
    }
}

