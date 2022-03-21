using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Client.GameObjects.ChessBoard;

namespace Client.GameObjects.ChessPieces
{
    public class Queen : ChessPiece
    {
        public Queen(Cell cell, PieceColor pieceColor, int sheetIndex) : base(cell, pieceColor, sheetIndex)
        {
        }

        public override Cell[] GetPossibleMoves(ChessBoard chessBoard)
        {
            List<Cell> cells = new List<Cell>();

            AddNeighborRecursively(cells, chessBoard, X, Y, -1, 0);
            AddNeighborRecursively(cells, chessBoard, X, Y, 1, 0);
            AddNeighborRecursively(cells, chessBoard, X, Y, 0, -1);
            AddNeighborRecursively(cells, chessBoard, X, Y, 0, 1);

            AddNeighborRecursively(cells, chessBoard, X, Y, -1, -1);
            AddNeighborRecursively(cells, chessBoard, X, Y, 1, 1);
            AddNeighborRecursively(cells, chessBoard, X, Y, -1, 1);
            AddNeighborRecursively(cells, chessBoard, X, Y, 1, -1);

            return cells.ToArray();
        }
    }
}
