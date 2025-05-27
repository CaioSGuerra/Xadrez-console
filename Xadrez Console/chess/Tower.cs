using tabuleiro;

namespace xadrez
{
    internal class Tower : Piece
    {
        public Tower(Board tab, Color cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "T";
        }

        private bool CanMove(Position position)
        {
            Piece p = Board.Piece(position);
            return p == null || p.Color != Color;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Line, Board.Column];

            Position position = new Position(0, 0);

            // acima
            position.ValueSet(Position.Line - 1, Position.Column);
            while (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                {
                    break;
                }
                position.Line = position.Line - 1;
            }
            // abaixo
            position.ValueSet(Position.Line + 1, Position.Column);
            while (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                {
                    break;
                }
                position.Line = position.Line + 1;
            }
            // direita
            position.ValueSet(Position.Line, Position.Column + 1);
            while (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                {
                    break;
                }
                position.Column = position.Column + 1;
            }
            // esquerda
            position.ValueSet(Position.Line, Position.Column - 1);
            while (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                {
                    break;
                }
                position.Column = position.Column - 1;
            }
            return mat;
        }
    }
}
