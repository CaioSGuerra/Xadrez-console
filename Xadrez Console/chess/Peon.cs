using tabuleiro;

namespace xadrez
{
    internal class Peon : Piece
    {
        private ChessMatch Match;

        public Peon(Board board, Color color, ChessMatch match) : base(board, color)
        {
            Match = match;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool CanMove(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p == null || p.Color != Color;
        }

        private bool TheresEnemie(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p != null && p.Color != Color;
        }

        private bool Free(Position pos)
        {
            return Board.Piece(pos) == null;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Line, Board.Column];

            Position pos = new Position(0, 0);


            if (Color == Color.White)
            {
                // acima
                pos.ValueSet(Position.Line - 1, Position.Column);
                if (Board.ValidPosition(pos) && Free(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }
                // acima primeira jogada
                pos.ValueSet(Position.Line - 2, Position.Column);
                if (Board.ValidPosition(pos) && Free(pos) && MovementQuantity == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }
                // ne
                pos.ValueSet(Position.Line - 1, Position.Column + 1);
                if (Board.ValidPosition(pos) && TheresEnemie(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }
                // no
                pos.ValueSet(Position.Line - 1, Position.Column - 1);
                if (Board.ValidPosition(pos) && TheresEnemie(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                // #jogada especial EnPassant
                if (Position.Line == 3)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if (Board.ValidPosition(left) && TheresEnemie(left) && Board.Piece(left) == Match.EnPassantVulnerable)
                    {
                        mat[left.Line - 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.ValidPosition(right) && TheresEnemie(right) && Board.Piece(right) == Match.EnPassantVulnerable)
                    {
                        mat[right.Line - 1, right.Column] = true;
                    }
                }
            }
            else
            {
                // abaixo
                pos.ValueSet(Position.Line + 1, Position.Column);
                if (Board.ValidPosition(pos) && Free(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }
                // abaixo primeira jogada
                pos.ValueSet(Position.Line + 2, Position.Column);
                if (Board.ValidPosition(pos) && Free(pos) && MovementQuantity == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }
                // se
                pos.ValueSet(Position.Line + 1, Position.Column + 1);
                if (Board.ValidPosition(pos) && TheresEnemie(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }
                // so
                pos.ValueSet(Position.Line + 1, Position.Column - 1);
                if (Board.ValidPosition(pos) && TheresEnemie(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                // #jogada especial EnPassant
                if (Position.Line == 4)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if (Board.ValidPosition(left) && TheresEnemie(left) && Board.Piece(left) == Match.EnPassantVulnerable)
                    {
                        mat[left.Line + 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.ValidPosition(right) && TheresEnemie(right) && Board.Piece(right) == Match.EnPassantVulnerable)
                    {
                        mat[right.Line + 1, right.Column] = true;
                    }
                }

            }
            return mat;
        }
    }
}
