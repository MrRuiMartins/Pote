Helper functions
 - RankOf(square)
 - FileOf(square)

Perft on just knight moves. Perft(1) with the knight

Add Moves of:
 - knights
 - king
 - sliders (bishop, rook, queen)
 - pawn
 - castling

Perft on total board:
 - Perft(depth)
    if (depth == 0) return 1; // number of positions at leaf node
    
    moves = GetMoves()
    foreach move in moves
        MakeMove
        positions += Perft (depth - 1)
        UndoMove

    return positions
