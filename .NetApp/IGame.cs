using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project
{
    interface IGame
    {
        string GameID { get; set; }

        int[][] Board { get; set; }
    }
}
