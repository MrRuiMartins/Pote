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
            
            var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";
            var fen2 = "rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2";
            var board1 = new Chessboard(fen);
            board1.LoadFen(fen);
            Console.WriteLine("--------------------------------");
            Console.WriteLine(board1.PrintBoard());
            Console.WriteLine("--------------------------------");
            
            board1.LoadFen(fen2);
            Console.WriteLine(board1.PrintBoard());
            Console.WriteLine("--------------------------------");

        }
    }
}