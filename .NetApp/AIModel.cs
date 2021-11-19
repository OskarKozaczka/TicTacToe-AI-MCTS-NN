using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Numpy;
using Keras.Models;
using Keras.Layers;
using Keras;

namespace project
{
    public class AIModel
    {
        public static void start()
        {

            var model = createModel();
            NDarray x;
            NDarray y;
            dataPreparation(out x, out y);
            model.Fit(x, y, batch_size: 2, epochs: 100, verbose: 1);
            saveModel(model);
            var test = np.array(JsonConvert.DeserializeObject<int[,]>(File.ReadAllLines("data/journal/OOHSD.txt")[0].Split(';')[0])).reshape(1, 10, 10);
            model.Predict(test);
        }

        public static Sequential createModel()
        {
            
            var model = new Sequential();
            model.Add(new Dense(100, activation: "relu", input_shape: new Shape(10, 10)));
            model.Add(new Dense(64, activation: "relu"));
            model.Add(new Dense(10, activation: "softmax"));

            model.Compile(optimizer: "sgd", loss: "binary_crossentropy", metrics: new string[] { "accuracy" });
            model.Summary();
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
                var lines = File.ReadAllLines("data/journal/OOHSD.txt");
                foreach (var line in lines)
                {
                    features.Add(np.array(JsonConvert.DeserializeObject<int[,]>(line.Split(';')[0])));
                    Move move = JsonConvert.DeserializeObject<Move>(line.Split(';')[1]);
                    var copy = emptyTable.Clone() as int[,];
                    copy[move.y, move.x] = 1;
                    labels.Add(np.array(copy));
                }
            }
            catch(Exception e)
            {
                throw new WrongDataFormatException("There was error reading the journal", e);
            }
            


            x = np.array(features.ToArray());
            y = np.array(labels.ToArray());
        }
        public static void saveModel(Sequential model)
        {
            string json = model.ToJson();
            File.WriteAllText("model.json", json);
            model.SaveWeight("model.h5");
        }

        public static BaseModel LoadModel()
        {
            var loaded_model = Sequential.ModelFromJson(File.ReadAllText("model.json"));
            loaded_model.LoadWeight("model.h5");
            return loaded_model;
        }

    }
}
   
