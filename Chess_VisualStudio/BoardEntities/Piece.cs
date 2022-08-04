namespace BoardEntities
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int NumMovements { get; protected set; }
        public Board Board { get; protected set; }

        public Piece()
        {
        }

        public Piece(Board board, Color color)
        {
            Position = null;
            Board = board;
            Color = color;
            NumMovements = 0;
        }

        public abstract bool[,] PossibleMoves();

        public void IncreaseNumMovements()
        {
            NumMovements++;
        }

        public void DecreaseNumMovements()
        {
            NumMovements--;
        }

        public bool HasPossibleMoves()
        {
            bool[,] mat = PossibleMoves();
            for(int i = 0; i < Board.Lines; i++)
            {
                for(int j = 0; j < Board.Columns; j++)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanMoveFor(Position pos)
        {
            return PossibleMoves()[pos.Line, pos.Column];
        }

    }
}
