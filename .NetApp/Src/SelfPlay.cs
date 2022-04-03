namespace project.Src
{
    public class SelfPlay
    {

        public const int BoardSize = GameManager.BoardSize;
        public static void Run(int runs)
        {
            for (int i = 0; i < runs; i++)
            {
                var game = new Game(GameManager.RandomStringGenerator(),useNetwork: false, MaxTime: 100);
                while (!Game.CheckForWinner(game.Board, 1) && !Game.CheckForWinner(game.Board, -1) && !Game.CheckForDraw(game.Board))
                {
                    var Move = new Move();
                    var AImoveInt = game.GetAIMove();

                    Move.Y = AImoveInt / BoardSize;
                    Move.X = AImoveInt % BoardSize;

                    game.MakeMove(Move);
                }
            }
        }
    }
}
