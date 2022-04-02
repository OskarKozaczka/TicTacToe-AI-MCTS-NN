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
    public class ValueNetwork
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
                model.Add(new Conv2D(64, new Tuple<int, int>(3, 3), activation: "tanh", padding: "same",data_format: "channels_last"));
                model.Add(new BatchNormalization());
            }
            model.Add(new Conv2D(1, new Tuple<int, int>(1, 1), activation: "tanh", padding: "same", data_format: "channels_last"));
            model.Add(new BatchNormalization());
            model.Add(new Flatten());
            model.Add(new Dense(64, activation: "tanh"));
            model.Add(new Dense(1, activation: "tanh"));
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
                    Model.Compile(optimizer: "adam", loss: "mean_squared_error", metrics: new string[] { "accuracy" });
                    SaveModel();
                }
            }
        }

        public static float MakePrediction(int[,] board)
        {
            using (Py.GIL())
            {
                return Model.PredictOnBatch(np.array(board).reshape(1, 5, 5)).GetData<float>().FirstOrDefault();
            }
        }
    }
}

