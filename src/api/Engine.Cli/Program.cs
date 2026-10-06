using System.Threading.Tasks.Dataflow;
using Engine.Core;

namespace Engine.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            Console.WriteLine();
            var startPosition = new Chessboard();
            Console.WriteLine(startPosition.PrintBoard());
            Console.WriteLine("--------------------------------");
            */
            
            
            // var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";
            // var fen = "rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2";
            // var fen = "1nbk1b1r/r2p1ppp/p7/1pN1Q3/4P3/4B3/PPP2PPP/R3KB1R b KQ - 0 11";
            var fen = "r1bqkb1r/pp2pppp/2np1n2/8/3NP3/2N5/PPP2PPP/R1BQKB1R w KQkq - 2 6";
            var board1 = new Chessboard(fen);
            
            Console.WriteLine("--------------------------------");
            Console.WriteLine(board1.PrintBoard());
            Console.WriteLine("--------------------------------");
            
            // board1.LoadFen(fen2);
            // Console.WriteLine(board1.PrintBoard());
            // Console.WriteLine("--------------------------------");

            board1.GetMoves();
            
            
        }
    }
}