using Engine.Core;

namespace Engine.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine();
            var startPosition = new Chessboard();
            Console.WriteLine(startPosition.PrintBoard());
            Console.WriteLine("--------------------------------");

        }
    }
}