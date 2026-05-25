namespace Engine.Core
{
    public class Chessboard
    {
        private const byte EMPTY_SQUARE = 0;
        private const byte PAWN_WHITE = 1;
        private const byte KNIGHT_WHITE = 2;
        private const byte BISHOP_WHITE = 3;
        private const byte ROOK_WHITE = 4;
        private const byte QUEEN_WHITE = 5;
        private const byte KING_WHITE = 6;
        private const byte PAWN_BLACK = 129;
        private const byte KNIGHT_BLACK = 130;
        private const byte BISHOP_BLACK = 131;
        private const byte ROOK_BLACK = 132;
        private const byte QUEEN_BLACK = 133;
        private const byte KING_BLACK = 134;
        

        private static byte[] START_POSITION =
        [
            ROOK_WHITE, KNIGHT_WHITE, BISHOP_WHITE, QUEEN_WHITE, KING_WHITE, BISHOP_WHITE, KNIGHT_WHITE, ROOK_WHITE, // rank 1
            PAWN_WHITE, PAWN_WHITE, PAWN_WHITE, PAWN_WHITE, PAWN_WHITE, PAWN_WHITE, PAWN_WHITE, PAWN_WHITE, // rank 2
            EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, // rank 3
            EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, // rank 4
            EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, // rank 5
            EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE, // rank 6
            PAWN_BLACK, PAWN_BLACK, PAWN_BLACK, PAWN_BLACK, PAWN_BLACK, PAWN_BLACK, PAWN_BLACK, PAWN_BLACK, // rank 7
            ROOK_BLACK, KNIGHT_BLACK, BISHOP_BLACK, QUEEN_BLACK, KING_BLACK, BISHOP_BLACK, KNIGHT_BLACK, ROOK_BLACK// rank 8
        ];

        /* Byte:
        1 -> Pawn (P/p)
        2 -> Knight (N/n)
        3 -> Bishop (B/b)
        4 -> Rook (R/r)
        5 -> Queen (Q/q)
        6 -> King (K/k)
        bit 7 = Black/White
        */
        private byte[] board;
        /* Missing
        - turn (white or black)
        - castling rights
        - en passant square
        */
        
        public Chessboard()
        {
            board = START_POSITION;
        }

        public byte GetSquare(int rank, int file)
        {
            return board[rank * 8 + file];
        }

        public void SetSquare(int rank, int file, byte piece)
        {
            board[rank * 8 + file] = piece;
        }

        public string PrintBoard()
        {
            var printedBoard = string.Empty;
            
            for (int rank = 7; rank >=0; rank--)
            {
                printedBoard += $"{rank + 1} ";
                for (int file = 0; file < 8; file++)
                {
                    var square = board[rank * 8 + file];
                    printedBoard += PrintSquare(square);
                }
                printedBoard += " \n";
            }
            printedBoard += "  abcdefgh\n";

            return printedBoard;
        }

        private char PrintSquare(byte square)
        {
            switch(square)
            {
                case PAWN_WHITE:
                    return 'P';
                case KNIGHT_WHITE:
                    return 'N';
                case BISHOP_WHITE:
                    return 'B';
                case ROOK_WHITE:
                    return 'R';
                case QUEEN_WHITE:
                    return 'Q';
                case KING_WHITE:
                    return 'K';

                case PAWN_BLACK:
                    return 'p';
                case KNIGHT_BLACK:
                    return 'n';
                case BISHOP_BLACK:
                    return 'b';
                case ROOK_BLACK:
                    return 'r';
                case QUEEN_BLACK:
                    return 'q';
                case KING_BLACK:
                    return 'k';

                case EMPTY_SQUARE:
                    return ' ';
                default:
                    throw new Exception($"Invalid byte in board: {square}");
            }
            
        }
    }


    /*

    1. Board Representation: This is your very first decision and your first class/struct. You need to decide how to store the board (e.g., a simple 8x8 array, 0x88, or Bitboards). Hint: Bitboards are the modern standard and highly recommended for performance, though they have a steeper learning curve.

    2. Board State Management: Alongside the physical pieces, you need to track game state variables: side to move, castling rights (kingside/queenside for both colors), en passant target square, and the half-move clock (for the 50-move rule).

    3. printBoard(): Write this immediately. A simple ASCII representation of the board in your terminal. If you can't see what's happening, you can't debug it.

    4. parseFen(string fen): You mentioned generateFen(), but parsing is actually what you need first. FEN (Forsyth-Edwards Notation) allows you to set up specific board positions instantly. This is crucial because you will need to load specific edge-case positions to test your move generator later.
    */
}
