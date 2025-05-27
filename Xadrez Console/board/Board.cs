namespace tabuleiro
{
    internal class Board
    {
        public int Line { get; set; }
        public int Column { get; set; }
        private Piece[,] ChessPieces;

        public Board(int line, int column)
        {
            Line = line;
            Column = column;
            ChessPieces = new Piece[line, column];
        }

        public Piece Piece(int line, int column)
        {
            return ChessPieces[line, column];
        }

        public Piece Piece(Position pos)
        {
            return ChessPieces[pos.Line, pos.Column];
        }

        public bool TheresPiece(Position pos)
        {
            ValidatePosition(pos);
            return Piece(pos) != null;
        }

        public void PutPiece(Piece p, Position pos)
        {
            if (TheresPiece(pos))
            {
                throw new BoardException("Já existe uma peça nessa posição!");
            }
            ChessPieces[pos.Line, pos.Column] = p;
            p.Position = pos;
        }

        public Piece RemovePiece(Position pos)
        {
            if (Piece(pos) == null)
            {
                return null;
            }
            Piece aux = Piece(pos);
            aux.Position = null;
            ChessPieces[pos.Line, pos.Column] = null;
            return aux;
        }
        public bool ValidPosition(Position pos)
        {
            if (pos.Line < 0 || pos.Line >= Line || pos.Column < 0 || pos.Column >= Column)
            {
                return false;
            }
            return true;
        }

        public void ValidatePosition(Position pos)
        {
            if (!ValidPosition(pos))
            {
                throw new BoardException("Posição Invalida!");
            }
        }
    }
}
