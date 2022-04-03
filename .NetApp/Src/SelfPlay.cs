﻿namespace project.Src
{
    public class SelfPlay
    {

        public const int BoardSize = GameManager.BoardSize;
        public static void Run(int runs)
        {

            while (true)
            {
                var win = 0;
                var loss = 0;
                var draw = 0;
                for (int i = 0; i < runs; i++)
                {
                    var game = new Game(GameManager.RandomStringGenerator(), MaxTime: 100);
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
                Console.WriteLine("win:{0}, loss:{1}, draw:{2}", win, loss, draw);
                ValueNetwork.LoadModel();
            }
        }
    }
}
