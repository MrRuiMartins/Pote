using System.Runtime.InteropServices;

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
        private byte[] board = START_POSITION;
        private Player player; 
        private string castlingAvailability = "KQkq";
        private string enPassantSquare = "-";
        private int halfmoveClock;
        private int fullmoveNumber;
        
        public Chessboard()
        {
            board = START_POSITION;
            player = Player.White;
            castlingAvailability = "KQkq";
            enPassantSquare = "-";
            halfmoveClock = 0;
            fullmoveNumber = 1;

        }

        public Chessboard(byte[] position, Player player)
        {
            this.board = position;
            this.player = player;
            this.castlingAvailability = "KQkq";
            this.enPassantSquare = "-";
            this.halfmoveClock = 0;
            this.fullmoveNumber = 1;
        }

        public Chessboard(string fen)
        {
            this.LoadFen(fen);
        }

        public byte GetSquare(int rank, int file)
        {
            return board[rank * 8 + file];
        }

        public void SetSquare(int rank, int file, byte piece)
        {
            board[rank * 8 + file] = piece;
        }

        public void GetMoves()
        {
            var boards = GetKnightMoves();
            foreach(var b in boards)
            {
                Console.WriteLine(b.PrintBoard());
            }
        }

        // TODO: So far only returns the moves of the first knight found.
        // TODO: There should be a difference between GetMoves and MakeMove.
        //       Currently we do GetKnightMoves, and it finds a move, copies 
        // the chessboard into a new one, and makes the move in the new board.
        // But I think it should be another way of "getting" the moves without 
        // making the move, no?
        public List<Chessboard> GetKnightMoves()
        {
            // scan the board until a knight of the current player is found
            for (int i = 0; i < 63; i++)
            {
                var squareContent = board[i];
                if (IsKnightOfPlayer(squareContent))
                {
                    // found a position with a knight
                    // find the max 8 possible moves and return them
                    return MakeKnightMoves(i);
                }
            }
            return new List<Chessboard>();
        }

        public List<Chessboard> MakeKnightMoves(int squareNumber)
        {
            List<Chessboard> boards = new List<Chessboard>();
            var rank = GetRankOfSquare(squareNumber);
            var file = GetFileOfSquare(squareNumber);
            // possible knight moves are:
             // - rank +- 1, file +- 2
             // - rank +- 2, file +- 1
            if (IsValidRankAndFile(rank + 1, file + 2) &&
                !HasPieceOfCurrentPlayer(GetPositionOf(rank + 1, file + 2)))
            {
                boards.Add(MakeMove(squareNumber, GetPositionOf(rank + 1, file + 2)));
            }
            if (IsValidRankAndFile(rank + 1, file - 2) &&
                !HasPieceOfCurrentPlayer(GetPositionOf(rank + 1, file - 2)))
            {
                boards.Add(MakeMove(squareNumber, GetPositionOf(rank + 1, file - 2)));
            }
            if (IsValidRankAndFile(rank - 1, file + 2) &&
                !HasPieceOfCurrentPlayer(GetPositionOf(rank - 1, file + 2)))
            {
                boards.Add(MakeMove(squareNumber, GetPositionOf(rank - 1, file + 2)));
            }
            if (IsValidRankAndFile(rank - 1, file - 2) &&
                !HasPieceOfCurrentPlayer(GetPositionOf(rank - 1, file - 2)))
            {
                boards.Add(MakeMove(squareNumber, GetPositionOf(rank - 1, file - 2)));

            }
            if (IsValidRankAndFile(rank + 2, file + 1) &&
                !HasPieceOfCurrentPlayer(GetPositionOf(rank + 2, file + 1)))
            {
                boards.Add(MakeMove(squareNumber, GetPositionOf(rank + 2, file + 1)));
            }
            if (IsValidRankAndFile(rank + 2, file - 1) &&
                !HasPieceOfCurrentPlayer(GetPositionOf(rank + 2, file - 1)))
            {
                boards.Add(MakeMove(squareNumber, GetPositionOf(rank + 2, file - 1)));
            }
            if (IsValidRankAndFile(rank - 2, file + 1) &&
                !HasPieceOfCurrentPlayer(GetPositionOf(rank - 2, file + 1)))
            {
                boards.Add(MakeMove(squareNumber, GetPositionOf(rank - 2, file + 1)));
            }
            if (IsValidRankAndFile(rank - 2, file - 1) &&
                !HasPieceOfCurrentPlayer(GetPositionOf(rank - 2, file - 1)))
            {
                boards.Add(MakeMove(squareNumber, GetPositionOf(rank - 2, file - 1)));
            }

            return boards;
        }

        /// <summary>
        /// Moves the piece from start square to end square.
        /// board.
        /// </summary>
        /// <param name="startSquare"></param>
        /// <param name="endSquare"></param>
        /// <exception cref="Exception"></exception>
        public Chessboard MakeMove(int startSquare, int endSquare)
        {
            if (startSquare < 0 || startSquare > 63 || endSquare < 0 || endSquare > 63)
            {
                throw new Exception($"Either start square {startSquare} or end square {endSquare} is out of bounds.");
            }

            if (!HasPieceOfCurrentPlayer(startSquare))
            {
                throw new Exception($"Cannot make move. Start square {startSquare} does not have a piece current player.");
            }

            Chessboard newBoard = Copy();
            newBoard.board[endSquare] = newBoard.board[startSquare];
            newBoard.board[startSquare] = EMPTY_SQUARE;
            newBoard.player = newBoard.player == Player.White? Player.Black : Player.White;

            return newBoard;
        }

        public bool IsValidRankAndFile(int rank, int file)
        {
            return rank >= 0 && rank <= 7 && file >=0 && file <= 7; 
        }

        /// <summary>
        /// Returns true if the specific square has a piece belonging to the current player; otherwise, false.
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        public bool HasPieceOfCurrentPlayer(int square)
        {
            if (player == Player.White)
            {
                return board[square] == PAWN_WHITE ||
                    board[square] == KNIGHT_WHITE ||
                    board[square] == BISHOP_WHITE ||
                    board[square] == ROOK_WHITE ||
                    board[square] == QUEEN_WHITE ||
                    board[square] == KING_WHITE;
            } else
            {
                return board[square] == PAWN_BLACK ||
                    board[square] == KNIGHT_BLACK ||
                    board[square] == BISHOP_BLACK ||
                    board[square] == ROOK_BLACK ||
                    board[square] == QUEEN_BLACK ||
                    board[square] == KING_BLACK;
            }
        }

        public bool IsKnightOfPlayer(byte pos)
        {
            if (player == Player.White)
            {
                return pos == KNIGHT_WHITE;
            } else
            {
                return pos == KNIGHT_BLACK;
            }
        }

        public Chessboard Copy()
        {
            return new Chessboard((byte[])this.board.Clone(), this.player);
        }

        public void LoadFen(string fen)
        {
            // rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1
            // 1 rank for every / starting from rank 8
            // b -> black to move
            // KQkq -> castling rights
            // e3 (empty is -) -> en passant square
            // 0 -> Halfmove clock
            // 1 -> Fullmove number

            var fenParts = fen.Split(' ');

            var position = fenParts[0];
            this.board = loadFenBoardPosition(position);

        
            var playerToMove = fenParts[1];
            this.player = playerToMove == "w"? Player.White : Player.Black;

            var castlingRights = fenParts[2];
            this.castlingAvailability = castlingRights;

            var enPassantSquare = fenParts[3];
            this.enPassantSquare = enPassantSquare;

            var halfmoveClock = fenParts[4];
            this.halfmoveClock = Int32.Parse(halfmoveClock);

            var fullmoveNumber = fenParts[5];
            this.fullmoveNumber = Int32.Parse(fullmoveNumber);

            
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

        /*
            int square : a square on the board from 0 to 63.
            The rank is the horizontal lines.
            e.g. Pawns on rank 2 and 7 in start position.
            Returns the rank of the square from 0 to 7.
        */
        public int GetRankOfSquare(int square)
        {
            // square 0 is a white rook -> that's rank 0
            // square 1 is a white rook -> that's rank 0
            // square 63 -> rank 7
            return square / 8;
        }

        /// <summary>
        /// Gets the position (0 to 63) of the specified rank and file.
        /// </summary>
        /// <param name="rank">0 to 7</param>
        /// <param name="file">0 to 7</param>
        /// <returns>
        /// Returns the position of the rank and file (0 to 63).
        /// </returns>
        public int GetPositionOf(int rank, int file)
        {
            if (rank < 0 || rank > 7 || file < 0 || file > 7)
            {
                throw new Exception($"Position rank {rank} file {file} is out of bounds.");
            }
            return 8 * rank + file;
        }

        /*
            int square : a square on the board from 0 to 63
            The file is the vertical lines. 
            e.g. Rooks on file 1 and 8 in start position.
            Returns the file of the square from 0 to 7.
        */
        public int GetFileOfSquare(int square)
        {
            // square 0 is a white rook -> that's file 1
            // square 1 is a white rook -> that's file 2
            // square 23 -> rank 3
            return square % 8;
        }

        /*
          input: "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";
          output: byte[] = [ROOK_BLACK, KNNIGHT_BLACK,... EMPTY_SQUARE, EMPTY_SQUARE, EMPTY_SQUARE..., PAWN_WHITE]
        */
        private byte[] loadFenBoardPosition(string fenPosition)
        {
            var bytePosition = new byte[64];            
            int index = 0;

            var rows = fenPosition.Split('/');

            for (int i = rows.Length - 1; i >= 0 ; i--)
            {
                var row = rows[i];
                foreach(char c in row)
                {
                    if (Char.IsNumber(c))
                    {
                        int curr = 0;
                        double numEmptySquares = Char.GetNumericValue(c);
                        while (curr < numEmptySquares)
                        {
                            curr++;
                            bytePosition[index++] = EMPTY_SQUARE;
                        }
                    } else
                    {
                        bytePosition[index++] = ParseFenCharToByte(c);
                    }
                }
            }
            
            return bytePosition;
        }

        private byte ParseFenCharToByte(char c)
        {
            switch(c)
            {
                case 'P':
                    return PAWN_WHITE;
                case 'N':
                    return KNIGHT_WHITE;
                case 'B':
                    return BISHOP_WHITE;
                case 'R':
                    return ROOK_WHITE;
                case 'Q':
                    return QUEEN_WHITE;
                case 'K':
                    return KING_WHITE;
                
                case 'p':
                    return PAWN_BLACK;
                case 'n':
                    return KNIGHT_BLACK;
                case 'b':
                    return BISHOP_BLACK;
                case 'r':
                    return ROOK_BLACK;
                case 'q':
                    return QUEEN_BLACK;
                case 'k':
                    return KING_BLACK;

                case ' ':
                    return EMPTY_SQUARE;
                default:
                    throw new Exception($"Invalid char in fen: {c}");
            }
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
