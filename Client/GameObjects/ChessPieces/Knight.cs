﻿using Client.GameStates;
using Networking.JsonObjects;
using System.Collections.Generic;
using static Client.GameStates.ChessBoard;

namespace Client.GameObjects.ChessPieces
{
    public class Knight : ChessPiece
    {
        public Knight(Cell cell, ChessColor pieceColor, int sheetIndex) : base(cell, pieceColor, sheetIndex)
        {
        }

        public override Cell[] GetPossibleMoves(ChessBoard chessboard)
        {
            List<Cell> cells = new List<Cell>();

            AddCell(cells, chessboard.GetCellAt(X - 2, Y - 1));
            AddCell(cells, chessboard.GetCellAt(X - 2, Y + 1));

            AddCell(cells, chessboard.GetCellAt(X - 1, Y - 2));
            AddCell(cells, chessboard.GetCellAt(X - 1, Y + 2));

            AddCell(cells, chessboard.GetCellAt(X + 2, Y - 1));
            AddCell(cells, chessboard.GetCellAt(X + 2, Y + 1));

            AddCell(cells, chessboard.GetCellAt(X + 1, Y - 2));
            AddCell(cells, chessboard.GetCellAt(X + 1, Y + 2));

            return cells.ToArray();
        }
        
        private void AddCell(List<Cell> cells, Cell? cell)
        {
            if (cell.HasValue)
            {
                if (cell.Value.ChessPiece?.PieceColor != pieceColor)
                {
                    cells.Add(cell.Value);
                }
            }
        }
    }
}
