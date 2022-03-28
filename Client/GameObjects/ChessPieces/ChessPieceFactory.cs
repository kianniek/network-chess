using Client.GameStates;
using Networking.JsonObjects;
using static Client.GameStates.ChessBoard;

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

        public static ChessPiece CreateChessPiece(Cell cell, ChessColor color, PieceName pieceName)
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

        private static PieceSheetIndex GetPieceSheetIndex(ChessColor color, PieceName pieceName)
        {
            switch (pieceName)
            {
                case PieceName.King:
                    return color == ChessColor.Black ? PieceSheetIndex.BlackKing : PieceSheetIndex.WhiteKing;
                case PieceName.Queen:
                    return color == ChessColor.Black ? PieceSheetIndex.BlackQueen : PieceSheetIndex.WhiteQueen;
                case PieceName.Bishop:
                    return color == ChessColor.Black ? PieceSheetIndex.BlackBishop : PieceSheetIndex.WhiteBishop;
                case PieceName.Knight:
                    return color == ChessColor.Black ? PieceSheetIndex.BlackKnight : PieceSheetIndex.WhiteKnight;
                case PieceName.Rook:
                    return color == ChessColor.Black ? PieceSheetIndex.BlackRook : PieceSheetIndex.WhiteRook;
                case PieceName.Pawn:
                default:
                    return color == ChessColor.Black ? PieceSheetIndex.BlackPawn : PieceSheetIndex.WhitePawn;
            }
        }
    }
}
