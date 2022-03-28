using Client.GameStates;
using Networking.JsonObjects;
using System.Collections.Generic;
using static Client.GameStates.ChessBoard;

namespace Client.GameObjects.ChessPieces
{
    public class Pawn : ChessPiece
    {
        private readonly int yDirection;        

        public Pawn(Cell cell, ChessColor pieceColor, int sheetIndex) : base(cell, pieceColor, sheetIndex)
        {
            yDirection = pieceColor == ChessColor.White ? -1 : 1;            
        }

        private bool IsAtStartLocation
        { 
            get => pieceColor == ChessColor.White ? cell.Y == 6 : cell.Y == 1;
        }

        public override Cell[] GetCheckMoves(ChessBoard chessBoard)
        {
            List<Cell> cells = new List<Cell>();

            Cell? diagonalLeft = chessBoard.GetCellAt(X - yDirection, Y + yDirection); 
            Cell? diagonalRight = chessBoard.GetCellAt(X + yDirection, Y + yDirection);

            if (diagonalLeft.HasValue)
                cells.Add(diagonalLeft.Value);

            if (diagonalRight.HasValue)
                cells.Add(diagonalRight.Value);
            
            AddDiagonalNeighbor(cells, chessBoard.GetCellAt(X + yDirection, Y + yDirection));

            return cells.ToArray();
        }

        public override Cell[] GetPossibleMoves(ChessBoard chessBoard)
        {
            List<Cell> cells = new List<Cell>();

            Cell? cellInFront = chessBoard.GetCellAt(X, Y + yDirection);
            bool isOccupied = cellInFront?.ChessPiece != null;

            if (!isOccupied)
            {
                if (cellInFront.HasValue)
                    cells.Add(cellInFront.Value);

                if (IsAtStartLocation)
                {
                    Cell? cellInFrontPlusOne = chessBoard.GetCellAt(X, Y + yDirection * 2);

                    if (cellInFrontPlusOne.HasValue)
                        cells.Add(cellInFrontPlusOne.Value);
                }
            }

            AddDiagonalNeighbor(cells, chessBoard.GetCellAt(X - yDirection, Y + yDirection));
            AddDiagonalNeighbor(cells, chessBoard.GetCellAt(X + yDirection, Y + yDirection));
            return cells.ToArray();
        }

        private void AddDiagonalNeighbor(List<Cell> cells, Cell? diagonal)
        {
            if (diagonal?.ChessPiece != null)
            {
                if (diagonal.Value.ChessPiece.PieceColor != this.PieceColor)
                {
                    cells.Add(diagonal.Value);
                }
            }         
        }
    }
}
