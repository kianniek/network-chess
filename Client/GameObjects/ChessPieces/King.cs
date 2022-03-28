using Client.GameStates;
using Networking.JsonObjects;
using System.Collections.Generic;
using static Client.GameStates.ChessBoard;

namespace Client.GameObjects.ChessPieces
{
    public class King : ChessPiece
    {
        public King(Cell cell, ChessColor pieceColor, int sheetIndex) : base(cell, pieceColor, sheetIndex)
        {

        }

        public override Cell[] GetPossibleMoves(ChessBoard chessboard)
        {
            List<Cell> cells = new List<Cell>();

            AddNeighbor(cells, chessboard.GetCellAt(X - 1, Y));
            AddNeighbor(cells, chessboard.GetCellAt(X + 1, Y));
            AddNeighbor(cells, chessboard.GetCellAt(X, Y - 1));
            AddNeighbor(cells, chessboard.GetCellAt(X, Y + 1));
            AddNeighbor(cells, chessboard.GetCellAt(X - 1, Y + 1));
            AddNeighbor(cells, chessboard.GetCellAt(X + 1, Y + 1));
            AddNeighbor(cells, chessboard.GetCellAt(X - 1, Y - 1));
            AddNeighbor(cells, chessboard.GetCellAt(X + 1, Y - 1));

            return cells.ToArray();
        }
    }
}