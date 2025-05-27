using tabuleiro;

namespace xadrez
{
    internal class King : Piece
    {
        private ChessMatch Match;

        public King(Board board, Color color, ChessMatch match) : base(board, color)
        {
            Match = match;
        }

        public override string ToString()
        {
            return "R";
        }

        private bool CanMove(Position position)
        {
            Piece p = Board.Piece(position);
            return p == null || p.Color != Color;
        }

        private bool RockTest(Position position)
        {
            Piece p = Board.Piece(position);
            return p != null && p is Tower && p.Color == Color && p.MovementQuantity == 0;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Line, Board.Column];

            Position position = new Position(0, 0);

            // acima
            position.ValueSet(Position.Line - 1, Position.Column);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
            }
            // ne
            position.ValueSet(Position.Line - 1, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
            }
            // direita
            position.ValueSet(Position.Line, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
            }
            // se
            position.ValueSet(Position.Line + 1, Position.Column + 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
            }
            // abaixo
            position.ValueSet(Position.Line + 1, Position.Column);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
            }
            // so
            position.ValueSet(Position.Line + 1, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
            }
            // esquerda
            position.ValueSet(Position.Line, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
            }
            // no
            position.ValueSet(Position.Line - 1, Position.Column - 1);
            if (Board.ValidPosition(position) && CanMove(position))
            {
                mat[position.Line, position.Column] = true;
            }

            //#jogada especial roque
            if (MovementQuantity == 0 && !Match.Check)
            {
                // #jogada especial roque pequeno
                Position posT1 = new Position(Position.Line, Position.Column + 3);
                if (RockTest(posT1))
                {
                    Position p1 = new Position(Position.Line, Position.Column + 1);
                    Position p2 = new Position(Position.Line, Position.Column + 2);
                    if (Board.Piece(p1) == null && Board.Piece(p2) == null)
                    {
                        mat[Position.Line, Position.Column + 2] = true;
                    }
                }
                // #jogada especial roque grande
                Position posT2 = new Position(Position.Line, Position.Column - 4);
                if (RockTest(posT2))
                {
                    Position p1 = new Position(Position.Line, Position.Column - 1);
                    Position p2 = new Position(Position.Line, Position.Column - 2);
                    Position p3 = new Position(Position.Line, Position.Column - 3);
                    if (Board.Piece(p1) == null && Board.Piece(p2) == null && Board.Piece(p3) == null)
                    {
                        mat[Position.Line, Position.Column - 2] = true;
                    }
                }
            }
            return mat;
        }
    }
}
