using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Numpy;
using Keras.Models;
using Keras.Layers;
using Keras;
using System.Threading;
using System.Threading.Tasks;
using Python.Runtime;

namespace project
{
    public static class AIModel
    {
        private static BaseModel Model;
        private static NDarray test;

        public static void start()
        {

            NDarray x;
            NDarray y;
            dataPreparation(out x, out y);
            createModel();
            //model.Fit(x, y, batch_size: 1, epochs: 100, verbose: 1);
            //SaveModel();
            var journal = Directory.GetFiles("data/journal");
            test = np.array(JsonConvert.DeserializeObject<int[,]>(File.ReadAllLines(journal[0])[0].Split(';')[0])).reshape(1, 10,10);
            Console.WriteLine(Model.Predict(test));

        }

        public static Sequential createModel()
        {
                var model = new Sequential();
                model.Add(new Dense(100, activation: "relu", input_shape: new Shape(10, 10)));
                model.Add(new Flatten());
                model.Add(new Dense(100, activation: "softmax"));
                model.Compile(optimizer: "sgd", loss: "binary_crossentropy", metrics: new string[] { "accuracy" });
                model.Summary();
                Model = model;
                return model; 
        }

        public class Move
        {
            public int x { set; get; }
            public int y { set; get; }
        }

        public static void dataPreparation(out NDarray x,out NDarray y)
        {
            var features = new List<NDarray>();
            var labels = new List<NDarray>();
            var emptyTable = new int[10, 10];

            Array.Clear(emptyTable, 0, emptyTable.Length);
            try
            {
                var journal = Directory.GetFiles("data/journal");
                foreach (var entry in journal)
                {
                    var lines = File.ReadAllLines(entry);
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            features.Add(np.array(JsonConvert.DeserializeObject<int[,]>(line.Split(';')[0])));
                            Move move = JsonConvert.DeserializeObject<Move>(line.Split(';')[1]);
                            var copy = emptyTable.Clone() as int[,];
                            copy[move.y, move.x] = 1;
                            labels.Add(np.array(copy).reshape(100));
                        }

                    }
                }
          
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("Journal is empty", e);
            }
            catch (Exception e)
            {
                throw new WrongDataException("There was error reading the journal", e);
            }
            
            x = np.array(features.ToArray());
            y = np.array(labels.ToArray());
        }

        public static void SaveModel()
        {
            File.WriteAllText("data/model/model.json", Model.ToJson());
            Model.SaveWeight("data/model/model.h5");
        }

        public static void LoadModel()
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
                    Model = createModel();
                    SaveModel();
                }
        }

        public static int GetMove(int[,] Board)
        {
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                {
                    Board[x, y] = Board[x, y] * -1;
                }
            
            var journal = Directory.GetFiles("data/journal");
            List<float> resultL;
            
            using (Py.GIL())
            {
                NDarray result;
                var test = np.array(Board).reshape(1, 10, 10);
                if (Model == null) LoadModel();
                result = Model.Predict(test);
                resultL = result.GetData<float>().ToList();
            }
            
            var AImove = resultL.IndexOf(resultL.Max());
            return AImove;
        }

        public static void TrainModel()
        {
            using (Py.GIL())
            {
                NDarray x;
                NDarray y;
                dataPreparation(out x, out y);
                LoadModel();
                Model.Compile(optimizer: "sgd", loss: "binary_crossentropy", metrics: new string[] { "accuracy" });
                if (Directory.GetFiles("data/journal").Any())
                {
                    Model.Fit(x, y, batch_size: 1, epochs: 100, verbose: 1);
                }
                
                SaveModel();
            }
            
        }
    }
}
   
