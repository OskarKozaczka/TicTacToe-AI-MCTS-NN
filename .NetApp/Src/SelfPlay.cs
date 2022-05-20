using System.IO;

namespace project.Src
{
    public class SelfPlay
    {

        public const int BoardSize = GameManager.BoardSize;
        public static void Run(int runs)
        {
            ValueNetwork.ConsumeMovesFromDB();
            while (true)
            {
                var win = 0;
                var loss = 0;
                var draw = 0;
                for (int i = 0; i < runs; i++)
                {
                    var game = new Game(GameManager.RandomStringGenerator(), simNum: 2000);
                    while (true)
                    {
                        var Move = new Move();
                        var AImoveInt = game.GetAIMove(true);

                        Move.Y = AImoveInt / BoardSize;
                        Move.X = AImoveInt % BoardSize;

                        game.MakeMove(Move);

                        if (Game.CheckForWinner(game.Board, 1))
                        {
                            win++;
                            break;
                        }

                        if (Game.CheckForWinner(game.Board, -1))
                        {
                            loss++;
                            break;
                        }
                        if (Game.CheckForDraw(game.Board))
                        {
                            draw++;
                            break;
                        }
                    }
                }
                Console.WriteLine("win:{0}, draw:{2}, loss:{1}", win, loss, draw);
                ValueNetwork.SaveModelAs(win,draw,loss) ;
                ValueNetwork.ConsumeMovesFromDB();
            }
        }
    }
}
