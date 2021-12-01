using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Numpy;
using Keras.Models;
using Keras.Layers;
using Keras;
using Python.Runtime;

namespace project
{
    public static class AIModel
    {

        const int batch_size = 1;
        const int epochs = 100;
        const int verbose = 1;


        private static BaseModel Model;
        //private static NDarray test;

        //public static void Start()
        //{
        //    dataPreparation(out NDarray x, out NDarray y);
        //    createModel();
        //    //model.Fit(x, y, batch_size: 1, epochs: 100, verbose: 1);
        //    //SaveModel();
        //    var journal = Directory.GetFiles("data/journal");
        //    test = np.array(JsonConvert.DeserializeObject<int[,]>(File.ReadAllLines(journal[0])[0].Split(';')[0])).reshape(1, 10,10);
        //    Console.WriteLine(Model.Predict(test));

        //}

        public static void CreateModel()
        {
            var model = new Sequential();
            model.Add(new Input(shape: new Shape(10,10)));
            //model.Add(new Conv2D(100,new Tuple<int, int>(5,5),activation: "tanh"));
            model.Add(new Flatten());
            model.Add(new Dense(100, activation: "tanh"));
            model.Add(new Dense(100, activation: "softmax"));
            Model = model;
        }

        //public static void DataPreparation(out NDarray x,out NDarray y)
        //{
        //    var features = new List<NDarray>();
        //    var labels = new List<NDarray>();
        //    try
        //    {
        //        var journal = Directory.GetFiles("data/journal");
        //        foreach (var entry in journal)
        //        {
        //            var lines = File.ReadAllLines(entry);
        //            foreach (var line in lines)
        //            {
        //                if (!string.IsNullOrWhiteSpace(line))
        //                {
        //                    features.Add(np.array(JsonConvert.DeserializeObject<int[,]>(line.Split(';')[0])));
        //                }

        //            }
        //        }
          
        //    }
        //    catch (FileNotFoundException e)
        //    {
        //        throw new FileNotFoundException("Journal is empty", e);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new WrongDataException("There was error reading the journal", e);
        //    }

        //    x = np.array(features.ToArray());
        //    y = np.array(labels.ToArray());
        //}

        public static void ConsumeMovesFromJournal(string gameID)
        {
            using (Py.GIL())
            {
                var data = DataManager.ReadAndPrepareData(gameID);
                Model.Fit(data.features, data.labels, batch_size: batch_size, epochs: epochs, verbose: verbose);
                AIModel.SaveModel();
            }
        }

        static void SaveModel()
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
                    Model.Summary();
                }
            }

        }

        public static int GetMove(int[,] Board)
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    Board[x, y] = Board[x, y] * -1;
                }
            int Move = -1;
            var index = 0;
            while(Move == -1 || Board[Move/10,Move%10] != 0 )
            {
                Move = AIPredict(Board,index);
                index++;
            }

            return Move;
        }

        static int AIPredict(int[,] Board, int index)
        {
            using (Py.GIL())
            {
                List<float> resultL;
                NDarray result;

                result = Model.Predict(np.array(Board).reshape(1, 10, 10));
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

        //static void TrainModel()
        //{
        //    using (Py.GIL())
        //    {
        //        DataPreparation(out NDarray x, out NDarray y);
        //        LoadModel();
        //        if (Directory.GetFiles("data/journal").Any())
        //        {
        //            Console.WriteLine("Traning from {0} samples ", x.size);
        //            Model.Fit(x, y, batch_size: batch_size, epochs: epochs, verbose: verbose);
        //        }
                
        //        SaveModel();
        //    }
        //}
    }
}
   
