using Client.GameObjects;
using Client.GameObjects.ChessPieces;
using Client.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Networking.JsonObjects;
using System;

namespace Client.GameStates
{
    public class ChessBoard : GameObjectList, IRequestListener
    {
        public struct Cell
        {
            private ChessPiece chessPiece;

            public Vector2 Position { get; set; }
            public ChessPiece ChessPiece
            {
                get => chessPiece;
                set
                {
                    chessPiece = value;

                    if (chessPiece != null)
                    {
                        chessPiece.Cell = this;
                    }
                }
            }

            public int X { get; set; }

            public int Y { get; set; }

            public string Name { get; set; }

            public static bool operator ==(Cell cell1, Cell cell2) => cell1.Name == cell2.Name;
            public static bool operator !=(Cell cell1, Cell cell2) => cell1.Name != cell2.Name;

            public override string ToString()
            {
                return Name;
            }
        }

        private Cell[,] cells;
        private const int boardSize = 8;
        private const float cellSize = 106.25f;

        private ChessColor currentPlayer = ChessColor.White;
        private ChessColor ourColour;
        private readonly Cell NoSelectedCell = new Cell { Name = "No Cell" };
        private Cell selectedCell;
        private Cell[] possibleMoves;

        private Cell whiteKingLocation, blackKingLocation;
        private GameStateText gameStateText;

        private bool gameStarted = false;

        public ChessBoard() : base()
        {
            Initialize();
            Reset();
        }

        public void InitializeRequestHandlers()
        {
            new JoinGameRequestHandler(this);
            new MoveRequestHandeler(this);
            new StartGameRequestHandler(this);
        }

        public void ColorSelected(ChessColor clientColor)
        {
            //TODO: store which color the client plays with.
            ourColour = clientColor;
        }

        public void UpdateChessBord(Cell from, Cell to)
        {
            MoveChessPieceTo(from, to);
        }
        public void StartGame(bool gameStarted)
        {
            this.gameStarted = gameStarted;
        }

        private void Initialize()
        {
            cells = new Cell[boardSize, boardSize];
            possibleMoves = new Cell[0];

            selectedCell = NoSelectedCell;

            gameStateText = new GameStateText("GameFont");
            Add(new SpriteGameObject("spr_chessboard"));
            Add(gameStateText);
            CreateGrid();
        }

        public override void Reset()
        {
            base.Reset();

            CreateChessPieces();
        }

        private bool IsKingCheck(Cell kingCell, ChessColor kingColor)
        {
            foreach (GameObject gameObject in Children)
            {
                if (gameObject is ChessPiece)
                {
                    ChessPiece chessPiece = gameObject as ChessPiece;
                    if (chessPiece.PieceColor != kingColor)
                    {
                        Cell[] checkMoves = chessPiece.GetCheckMoves(this);
                        foreach (Cell possibleMove in checkMoves)
                        {
                            if (possibleMove == kingCell)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool MoveChessPieceTo(Cell from, Cell to)
        {
            ChessPiece tempFrom = cells[from.X, from.Y].ChessPiece;
            ChessPiece tempTo = cells[to.X, to.Y].ChessPiece;

            cells[to.X, to.Y].ChessPiece = from.ChessPiece;
            cells[from.X, from.Y].ChessPiece = null;

            if (from.ChessPiece != null && to.ChessPiece != null)
            {
                if (from.ChessPiece.PieceColor != to.ChessPiece.PieceColor)
                {
                    Remove(to.ChessPiece);
                }
            }

            //revert the last move if it was invalid (because the king is check).
            if (!CanMoveKing(tempFrom, to))
            {
                gameStateText.ShowCheckText("Invalid move.");

                cells[from.X, from.Y].ChessPiece = tempFrom;
                cells[to.X, to.Y].ChessPiece = tempTo;

                if (tempTo != null)
                {
                    Add(tempTo);
                }

                return false;
            }

            CheckGameState(to);

            this.selectedCell = NoSelectedCell;
            possibleMoves = new Cell[0];
            this.currentPlayer = currentPlayer == ChessColor.White ? ChessColor.Black : ChessColor.White;

            return true;
        }

        private void CheckGameState(Cell to)
        {
            if (cells[to.X, to.Y].ChessPiece.PieceColor == ChessColor.Black)
            {
                if (cells[to.X, to.Y].ChessPiece is King)
                    blackKingLocation = cells[to.X, to.Y];

                if (IsKingCheck(whiteKingLocation, ChessColor.White))
                {
                    gameStateText.ShowCheckText("White Check!");
                }
            }
            else
            {
                if (cells[to.X, to.Y].ChessPiece is King)
                    whiteKingLocation = cells[to.X, to.Y];

                if (IsKingCheck(blackKingLocation, ChessColor.Black))
                {
                    gameStateText.ShowCheckText("Black Check!");
                }
            }
        }

        private bool CanMoveKing(ChessPiece chessPiece, Cell to)
        {
            if (chessPiece is King)
            {
                if (IsKingCheck(to, chessPiece.PieceColor))
                    return false;
            }
            else if (currentPlayer == ChessColor.Black)
            {
                if (IsKingCheck(blackKingLocation, ChessColor.Black))
                {
                    return false;
                }
            }
            else
            {
                if (IsKingCheck(whiteKingLocation, ChessColor.White))
                {
                    return false;
                }
            }
            return true;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            Console.WriteLine(gameStarted);
            if (gameStarted)
            {
                if (inputHelper.MouseLeftButtonPressed())
                {
                    Cell? currentSelectedCell = GetCellAt(inputHelper.MousePosition);

                    for (int i = 0; i < possibleMoves.Length; i++)
                    {
                        Cell cell = possibleMoves[i];

                        if (cell == currentSelectedCell)
                        {
                            MoveChessPieceTo(this.selectedCell, cell);
                            return;
                        }
                    }

                    if (currentPlayer == ourColour)
                    {
                        if (currentSelectedCell?.ChessPiece != null)
                        {
                            Cell cell = currentSelectedCell.Value;
                            if (cell.ChessPiece.PieceColor == currentPlayer)
                            {
                                this.selectedCell = cell;
                                this.possibleMoves = cell.ChessPiece.GetPossibleMoves(this);
                            }
                        }
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (selectedCell != NoSelectedCell)
            {
                foreach (Cell cell in possibleMoves)
                {
                    DrawCell(spriteBatch, cell.Position - Vector2.One * cellSize / 2, Color.Green);
                }
                DrawCell(spriteBatch, selectedCell.Position - Vector2.One * cellSize / 2, Color.Green);
            }
        }

        private void DrawCell(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            DrawingHelper.DrawRectangle(new Rectangle((int)(position.X + 2), (int)(position.Y + 2), (int)(cellSize - 6), (int)(cellSize - 6)), spriteBatch, color);
        }

        public Cell? GetCellAt(Vector2 position)
        {
            Vector2 mousePosition = Vector2.Floor(position / cellSize);
            return GetCellAt((int)mousePosition.X, (int)mousePosition.Y);
        }

        public Cell? GetCellAt(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < boardSize && y < boardSize)
            {
                return cells[x, y];
            }
            return null;
        }

        private void CreateGrid()
        {
            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    Cell cell = new Cell { X = x, Y = y, Position = new Vector2(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2), Name = $"Cell [X: {x}, Y: {y}]" };
                    cells[x, y] = cell;
                }
            }
        }

        private void CreateChessPieces()
        {
            CreatePawns();
            CreateRooks();
            CreateKnights();
            CreateBishops();
            CreateKingAndQueen();
        }

        private void AddChessPiece(ChessPiece chesspiece)
        {
            base.Add(chesspiece);

            Vector2 position = Vector2.Floor(chesspiece.Position / cellSize);
            cells[(int)position.X, (int)position.Y].ChessPiece = chesspiece;
        }

        private void CreatePawns()
        {
            for (int i = 0; i < 8; i++)
            {
                AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[i, 1], ChessColor.Black, PieceName.Pawn));
                AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[i, 6], ChessColor.White, PieceName.Pawn));
            }
        }

        private void CreateRooks()
        {
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[0, 0], ChessColor.Black, PieceName.Rook));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[7, 0], ChessColor.Black, PieceName.Rook));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[0, 7], ChessColor.White, PieceName.Rook));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[7, 7], ChessColor.White, PieceName.Rook));
        }

        private void CreateKnights()
        {
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[1, 0], ChessColor.Black, PieceName.Knight));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[6, 0], ChessColor.Black, PieceName.Knight));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[1, 7], ChessColor.White, PieceName.Knight));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[6, 7], ChessColor.White, PieceName.Knight));
        }

        private void CreateBishops()
        {
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[2, 0], ChessColor.Black, PieceName.Bishop));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[5, 0], ChessColor.Black, PieceName.Bishop));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[2, 7], ChessColor.White, PieceName.Bishop));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[5, 7], ChessColor.White, PieceName.Bishop));
        }

        private void CreateKingAndQueen()
        {
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[4, 0], ChessColor.Black, PieceName.King));
            blackKingLocation = cells[4, 0];

            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[4, 7], ChessColor.White, PieceName.King));
            whiteKingLocation = cells[4, 7];

            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[3, 0], ChessColor.Black, PieceName.Queen));
            AddChessPiece(ChessPieceFactory.CreateChessPiece(cells[3, 7], ChessColor.White, PieceName.Queen));
        }
    }
}