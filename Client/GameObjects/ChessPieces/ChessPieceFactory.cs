using static Client.GameObjects.ChessBoard;

namespace Client.GameObjects.ChessPieces
{
    public class ChessPieceFactory
    {
        public enum PieceSheetIndex
        {            
            WhiteKing,
            WhiteQueen,            
            WhiteBishop,
            WhiteKnight,
            WhiteRook,            
            WhitePawn,
            BlackKing,
            BlackQueen,
            BlackBishop,
            BlackKnight,
            BlackRook,
            BlackPawn
        }

        public static ChessPiece CreateChessPiece(Cell cell, PieceColor color, PieceName pieceName)
        {
            int sheetIndex = (int)GetPieceSheetIndex(color, pieceName);            
            switch (pieceName)
            {
                case PieceName.Pawn:
                default:                    
                    return new Pawn(cell, color, sheetIndex);
                case PieceName.King:
                    return new King(cell, color, sheetIndex);
                case PieceName.Queen:
                    return new Queen(cell, color, sheetIndex);
                case PieceName.Rook:
                    return new Rook(cell, color, sheetIndex);
                case PieceName.Bishop:
                    return new Bishop(cell, color, sheetIndex);
                case PieceName.Knight:
                    return new Knight(cell, color, sheetIndex);
            }
        }

        private static PieceSheetIndex GetPieceSheetIndex(PieceColor color, PieceName pieceName)
        {
            switch (pieceName)
            {
                case PieceName.King:
                    return color == PieceColor.Black ? PieceSheetIndex.BlackKing : PieceSheetIndex.WhiteKing;
                case PieceName.Queen:
                    return color == PieceColor.Black ? PieceSheetIndex.BlackQueen : PieceSheetIndex.WhiteQueen;
                case PieceName.Bishop:
                    return color == PieceColor.Black ? PieceSheetIndex.BlackBishop : PieceSheetIndex.WhiteBishop;
                case PieceName.Knight:
                    return color == PieceColor.Black ? PieceSheetIndex.BlackKnight : PieceSheetIndex.WhiteKnight;
                case PieceName.Rook:
                    return color == PieceColor.Black ? PieceSheetIndex.BlackRook : PieceSheetIndex.WhiteRook;
                case PieceName.Pawn:
                default:
                    return color == PieceColor.Black ? PieceSheetIndex.BlackPawn : PieceSheetIndex.WhitePawn;
            }
        }
    }
}
