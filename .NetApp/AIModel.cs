using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project
{
    public class AIModel
    {

        void test()
        {
            var ctx = new MLContext();
            IDataView trainingData = ctx.Data.LoadFromTextFile<GameData>("/data/journal/*.csv", hasHeader: false, separatorChar: ';');
        }
    }

    public class GameData
    {
        [LoadColumn(1)]
        public int[,] Board { get; set; }

        [LoadColumn(2)]
        [ColumnName("Label")]
        public float Move { get; set; }
    }
}
