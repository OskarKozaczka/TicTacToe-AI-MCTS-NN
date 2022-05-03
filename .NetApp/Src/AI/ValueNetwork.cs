using Keras;
using Keras.Layers;
using Keras.Models;
using Numpy;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Keras.Regularizers;

namespace project
{
    public class ValueNetwork
    {

        const int batch_size = 500;
        const int epochs = 1000;
        const int verbose = 0;
        const int layers = 3;
        public const int boardSize = GameManager.BoardSize;

        private static BaseModel Model;
        public static void CreateModel()
        {
            var model = new Sequential();
            model.Add(new Input(shape: new Shape(2, 5, 5)));
            model.Add(new Conv2D(64, (3, 3).ToTuple(), activation: "relu", padding: "same", data_format: "channels_last", kernel_regularizer: "l2"));
            model.Add(new Conv2D(64, (3, 3).ToTuple(), activation: "relu", padding: "same", data_format: "channels_last", kernel_regularizer: "l2"));
            model.Add(new Conv2D(64, (3, 3).ToTuple(), activation: "relu", padding: "same", data_format: "channels_last", kernel_regularizer: "l2"));
            model.Add(new Conv2D(2, (1, 1).ToTuple(), activation: "relu", padding: "same", data_format: "channels_last", kernel_regularizer: "l2"));
            model.Add(new Flatten());
            model.Add(new Dense(64, activation: "relu", kernel_regularizer: "l2"));
            model.Add(new Dense(1, activation: "tanh"));
            model.Summary();
            Model = model;
        }

        public static void ConsumeMovesFromJournal(string gameID)
        {
            using (Py.GIL())
            {
                var data = DataManager.ReadAndPrepareData(gameID);
                Model.Fit(data.features, data.labels, batch_size: batch_size, epochs: epochs, verbose: verbose,validation_split: 0.1f);
                SaveModel();
            }
        }

        public static void ConsumeMovesFromDB()
        {
            using (Py.GIL())
            {
                var data = DataManager.ReadAndPrepareDataFromDB();
                Model.Fit(data.features, data.labels, batch_size: batch_size, epochs: epochs, verbose: verbose, validation_split: 0.1f);
                SaveModel();
            }
        }

        public static void SaveModel()
        {
            try
            {
                File.WriteAllText("data/model/model.json", Model.ToJson());
                Model.SaveWeight("data/model/model.h5");
            }
            catch (Exception ex) { }
        }

        public static void LoadModel()
        {
            using (Py.GIL())
            {
                try
                {
                    if (!File.Exists("data/model/model.json"))
                    {
                        Console.WriteLine("Model was not found, creating a new one");
                        CreateModel();
                        SaveModel();
                    }
                    var loadedModel = Sequential.ModelFromJson(File.ReadAllText("data/model/model.json"));
                    loadedModel.LoadWeight("data/model/model.h5");
                    Model = loadedModel;
                    Model.Compile(optimizer: "adam", loss: "mean_squared_error");
                }
                catch (Exception ex) { }
            }
        }

        public static float MakePrediction(int[,] board)
        {
            using (Py.GIL())
            {
                var myBoard = new int[boardSize, boardSize];
                var enemyBoard = new int[boardSize, boardSize];

                for (int y = 0; y < boardSize; y++)
                    for (int x = 0; x < boardSize; x++)
                    {
                        if (board[y, x] == 1) myBoard[y, x] = 1;
                        else if (board[y, x] == -1) enemyBoard[y, x] = 1;
                    }

                var predictionInput = np.expand_dims(np.array(new NDarray[] { myBoard, enemyBoard }), 0);

                return Model.PredictOnBatch(predictionInput).GetData<float>().FirstOrDefault();
            }
        }
    }
}

