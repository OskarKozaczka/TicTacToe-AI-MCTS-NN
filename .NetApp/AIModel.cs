using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using Tensorflow;
using Tensorflow.NumPy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tensorflow.Keras.Layers;
using System.IO;
using Newtonsoft.Json;

namespace project
{
    public class AIModel
    {

        void test()
        {
            var layers = new LayersApi();
            var inputs = keras.Input(shape: (10, 10), name: "img");



            var x = layers.Conv2D(32, 3, activation: "relu").Apply(inputs);
            x = layers.Conv2D(64, 3, activation: "relu").Apply(x);
            var block_1_output = layers.MaxPooling2D(3).Apply(x);
            x = layers.Conv2D(64, 3, activation: "relu", padding: "same").Apply(block_1_output);
            x = layers.Conv2D(64, 3, activation: "relu", padding: "same").Apply(x);
            var block_2_output = layers.Add().Apply(new Tensors(x, block_1_output));
            x = layers.Conv2D(64, 3, activation: "relu", padding: "same").Apply(block_2_output);
            x = layers.Conv2D(64, 3, activation: "relu", padding: "same").Apply(x);
            var block_3_output = layers.Add().Apply(new Tensors(x, block_2_output));
            x = layers.Conv2D(64, 3, activation: "relu").Apply(block_3_output);
            x = layers.GlobalAveragePooling2D().Apply(x);
            x = layers.Dense(256, activation: "relu").Apply(x);
            x = layers.Dropout(0.5f).Apply(x);
            var outputs = layers.Dense(100).Apply(x);

            var model = keras.Model(inputs, outputs, name: "toy_resnet");
            model.summary();
            model.compile(optimizer: keras.optimizers.RMSprop(1e-3f),
            loss: keras.losses.CategoricalCrossentropy(from_logits: true),
            metrics: new[] { "acc" });


            var features = new List<string>();
            var labels = new List<string>();
            var lines = File.ReadAllLines("data/journal/*.csv");
            foreach(var line in lines )
            {
                features.Append(line.Split(';')[0]);
                labels.Append(line.Split(';')[1]);
            }

          model.fit(np.array(features.ToArray()), np.array(labels.ToArray()),
          batch_size: 1,
          epochs: 10,
          validation_split: 0.2f);


            var test = new Tensor(np.array(JsonConvert.DeserializeObject<int[,]>(File.ReadAllLines("data/journal/*.csv")[0].Split(';')[0])));
            model.predict(test);


        }
    }

}
