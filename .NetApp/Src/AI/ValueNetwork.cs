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

        const int batch_size = 100;
        const int epochs = 100;
        const int verbose = 0;
        const int layers = 10;

        private static BaseModel Model;
        public static void CreateModel()
        {
            var model = new Sequential();
            model.Add(new Input(shape: new Shape(5, 5, 1)));
            for (int i = 0; i < layers; i++)
            {
                model.Add(new Conv2D(128, (3, 3).ToTuple(), activation: "tanh", padding: "same", data_format: "channels_last", kernel_regularizer:"l2"));
                model.Add(new BatchNormalization());
            }
            model.Add(new Conv2D(1, (1, 1).ToTuple(), activation: "tanh", padding: "same", data_format: "channels_last", kernel_regularizer: "l2"));
            model.Add(new BatchNormalization());
            model.Add(new Flatten());
            model.Add(new Dense(128, activation: "tanh"));
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
                return Model.PredictOnBatch(np.array(board).reshape(1, 5, 5)).GetData<float>().FirstOrDefault();
            }
        }
    }
}

