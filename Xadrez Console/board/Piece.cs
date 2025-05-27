namespace tabuleiro
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int MovementQuantity { get; protected set; }

        public Board Board { get; protected set; }

        public Piece(Board board, Color color)
        {
            Position = null;
            Board = board;
            Color = color;
            MovementQuantity = 0;
        }

        public void IncreaseMovementQuantity()
        {
            MovementQuantity++;
        }
        public void DecreaveMovementQuantity()
        {
            MovementQuantity--;
        }

        public bool TherePossibleMovement()
        {
            bool[,] mat = PossibleMovements();
            for (int i = 0; i < Board.Line; i++)
            {
                for (int j = 0; j < Board.Column; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PossibleMovement(Position pos)
        {
            return PossibleMovements()[pos.Line, pos.Column];
        }

        public abstract bool[,] PossibleMovements();


    }
}
