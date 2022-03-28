using Client.GameStates;
using Networking.JsonObjects;
using System.Collections.Generic;
using static Client.GameStates.ChessBoard;

namespace Client.GameObjects
{
    public enum PieceName
    { 
        King,
        Queen, 
        Bishop, 
        Knight, 
        Rook,
        Pawn
    }

    public abstract class ChessPiece : SpriteGameObject
    {
        protected ChessColor pieceColor;
        protected Cell cell;

        public ChessPiece(Cell cell, ChessColor pieceColor, int sheetIndex) : base("spr_chess_pieces@6x2", sheetIndex: sheetIndex)
        {
            this.pieceColor = pieceColor;
            Cell = cell;
            cell.ChessPiece = this;
            this.position = cell.Position;
            this.Origin = Center;
            this.scale = 0.53125f;
        }

        public ChessColor PieceColor
        {
            get => pieceColor;
        }

        public Cell Cell
        {
            get => cell; 
            set 
            {
                cell = value;
                position = cell.Position;
            }
        }

        public int X
        {
            get => cell.X;
        }

        public int Y
        {
            get => cell.Y;
        }

        protected void AddNeighborRecursively(List<Cell> cells, ChessBoard chessBoard, int x, int y, int xDir, int yDir)
        {
            Cell? cell = chessBoard.GetCellAt(x + xDir, y + yDir);
            if (cell.HasValue)
            {
                if (cell.Value.ChessPiece == null || cell.Value.ChessPiece.PieceColor != pieceColor)
                {
                    cells.Add(cell.Value);

                    if (cell.Value.ChessPiece == null)
                        AddNeighborRecursively(cells, chessBoard, x + xDir, y + yDir, xDir, yDir);
                }
            }
        }

        protected void AddNeighbor(List<Cell> cells, Cell? cell)
        {
            if (cell.HasValue)
            {
                if (cell.Value.ChessPiece?.PieceColor != pieceColor)
                {
                    cells.Add(cell.Value);
                }
            }
        }

        public virtual Cell[] GetCheckMoves(ChessBoard chessBoard)
        {
            return GetPossibleMoves(chessBoard);
        }

        public abstract Cell[] GetPossibleMoves(ChessBoard chessBoard);
    }
}
