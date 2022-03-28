using Client.GameStates;
using Networking.JsonObjects;
using System.Collections.Generic;
using static Client.GameStates.ChessBoard;

namespace Client.GameObjects.ChessPieces
{
    public class Rook : ChessPiece
    {
        public Rook(Cell cell, ChessColor pieceColor, int sheetIndex) : base(cell, pieceColor, sheetIndex)
        {
        }

        public override Cell[] GetPossibleMoves(ChessBoard chessBoard)
        {
            List<Cell> cells = new List<Cell>();

            AddNeighborRecursively(cells, chessBoard, X, Y, -1, 0);
            AddNeighborRecursively(cells, chessBoard, X, Y, 1, 0);
            AddNeighborRecursively(cells, chessBoard, X, Y, 0, -1);
            AddNeighborRecursively(cells, chessBoard, X, Y, 0, 1);

            return cells.ToArray();
        }
    }
}
