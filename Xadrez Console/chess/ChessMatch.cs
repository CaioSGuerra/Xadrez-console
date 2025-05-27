using tabuleiro;

namespace xadrez
{
    internal class ChessMatch
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool Finished { get; set; }
        private HashSet<Piece> Pieces;
        private HashSet<Piece> Captured;
        public bool Check { get; private set; }
        public Piece EnPassantVulnerable { get; private set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Finished = false;
            Check = false;
            EnPassantVulnerable = null;
            Pieces = new HashSet<Piece>();
            Captured = new HashSet<Piece>();
            colocarPecas();
        }

        public Piece MovementExecute(Position origin, Position destination)
        {
            Piece p = Board.RemovePiece(origin);
            p.IncreaseMovementQuantity();
            Piece CapturedPiece = Board.RemovePiece(destination);
            Board.PutPiece(p, destination);

            if (CapturedPiece != null)
            {
                Captured.Add(CapturedPiece);
            }

            // #jogada especial roque pequeno
            if (p is King && destination.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Line, origin.Column + 3);
                Position destinyT = new Position(origin.Line, origin.Column + 1);
                Piece T = Board.RemovePiece(originT);
                T.IncreaseMovementQuantity();
                Board.PutPiece(T, destinyT);
            }

            // #jogada especial roque grande
            if (p is King && destination.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Line, origin.Column - 4);
                Position destinyT = new Position(origin.Line, origin.Column - 1);
                Piece T = Board.RemovePiece(originT);
                T.IncreaseMovementQuantity();
                Board.PutPiece(T, destinyT);
            }

            // #jogada especial EnPassant
            if (p is Peon)
            {
                if (origin.Column != destination.Column && CapturedPiece == null)
                {
                    Position posP;
                    if (p.Color == Color.White)
                    {
                        posP = new Position(destination.Line + 1, destination.Column);
                    }
                    else
                    {
                        posP = new Position(destination.Line - 1, destination.Column);
                    }
                    CapturedPiece = Board.RemovePiece(posP);
                    Captured.Add(CapturedPiece);
                }
            }
            return CapturedPiece;
        }

        public void UndoMovement(Position origin, Position destination, Piece CapturedPiece)
        {
            Piece p = Board.RemovePiece(destination);
            p.DecreaveMovementQuantity();
            if (CapturedPiece != null)
            {
                Board.PutPiece(CapturedPiece, destination);
                Captured.Remove(CapturedPiece);
            }
            Board.PutPiece(p, origin);

            // #jogada especial roque pequeno
            if (p is King && destination.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Line, origin.Column + 3);
                Position destinyT = new Position(origin.Line, origin.Column + 1);
                Piece T = Board.RemovePiece(originT);
                T.IncreaseMovementQuantity();
                Board.PutPiece(T, destinyT);
            }

            // #jogada especial roque grande
            if (p is King && destination.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Line, origin.Column - 4);
                Position destinyT = new Position(origin.Line, origin.Column - 1);
                Piece T = Board.RemovePiece(originT);
                T.IncreaseMovementQuantity();
                Board.PutPiece(T, destinyT);
            }

            // #jogada especial EnPassant
            if (p is Peon)
            {
                if (origin.Column != destination.Column && CapturedPiece == EnPassantVulnerable)
                {
                    Piece peon = Board.RemovePiece(destination);
                    Position posP;
                    if (p.Color == Color.White)
                    {
                        posP = new Position(3, destination.Column);
                    }
                    else
                    {
                        posP = new Position(4, destination.Column);
                    }
                    Board.PutPiece(peon, posP);
                }
            }
        }

        public void DoMove(Position origin, Position destination)
        {
            Piece capturedPiece = MovementExecute(origin, destination);

            if (InCheck(CurrentPlayer))
            {
                UndoMovement(origin, destination, capturedPiece);
                throw new BoardException("Você não pode se colocar em Check!");
            }

            Piece p = Board.Piece(destination);

            //#jogodaespecial promocao
            if (p is Peon)
            {
                if ((p.Color == Color.White && destination.Line == 0) || (p.Color == Color.Black && destination.Line == 7))
                {
                    p = Board.RemovePiece(destination);
                    Pieces.Remove(p);
                    Piece dama = new Queen(Board, p.Color);
                    Board.PutPiece(dama, destination);
                    Pieces.Add(dama);
                }

            }

            if (InCheck(Opponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            if (CheckmateTest(Opponent(CurrentPlayer)))
            {
                Finished = true;
            }
            else
            {
                Turn++;
                ChangePlayer();
            }



            // #jogada especial EnPassant
            if (p is Peon && (destination.Line == origin.Line - 2 || destination.Line == origin.Line + 2))
            {
                EnPassantVulnerable = p;
            }
            else
            {
                EnPassantVulnerable = null;
            }
        }

        public void ValidateOriginPosition(Position pos)
        {
            if (Board.Piece(pos) == null)
            {
                throw new BoardException("Não existe peça na posição de origin escolhida!");
            }
            if (CurrentPlayer != Board.Piece(pos).Color)
            {
                throw new BoardException("A Piece de origin escolhida não é sua!");
            }
            if (!Board.Piece(pos).TherePossibleMovement())
            {
                throw new BoardException("Não há movimentos possíveis para a peça de origin escolhida!");
            }
        }

        public void ValidateDestinyPosition(Position origem, Position destino)
        {
            if (!Board.Piece(origem).PossibleMovement(destino))
            {
                throw new BoardException("Posição de destination inválida!");
            }
        }

        private void ChangePlayer()
        {
            if (CurrentPlayer == Color.White)
            {
                CurrentPlayer = Color.Black;
            }
            else
            {
                CurrentPlayer = Color.White;
            }
        }

        public HashSet<Piece> CapturedPieces(Color cor)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in Captured)
            {
                if (x.Color == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Piece> InGamePieces(Color cor)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in Pieces)
            {
                if (x.Color == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(CapturedPieces(cor));
            return aux;
        }

        private Color Opponent(Color cor)
        {
            if (cor == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        private Piece King(Color cor)
        {
            foreach (Piece x in InGamePieces(cor))
            {
                if (x is King)
                {
                    return x;
                }
            }
            return null;
        }

        public bool InCheck(Color cor)
        {
            Piece R = King(cor);

            if (R == null)
            {
                throw new BoardException("Não tem King da cor " + cor + " no tabuleiro!");
            }
            foreach (Piece x in InGamePieces(Opponent(cor)))
            {
                bool[,] mat = x.PossibleMovements();
                if (mat[R.Position.Line, R.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckmateTest(Color cor)
        {
            if (!InCheck(cor))
            {
                return false;
            }
            foreach (Piece x in InGamePieces(cor))
            {
                bool[,] mat = x.PossibleMovements();
                for (int i = 0; i < Board.Line; i++)
                {
                    for (int j = 0; j < Board.Column; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origem = x.Position;
                            Position destino = new Position(i, j);
                            Piece pecaCapturada = MovementExecute(origem, destino);
                            bool testeXeque = InCheck(cor);
                            UndoMovement(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void InsertNewPiece(char coluna, int linha, Piece peca)
        {
            Board.PutPiece(peca, new ChessPosition(coluna, linha).toPosicao());
            Pieces.Add(peca);
        }

        private void colocarPecas()
        {
            //Coluna 1
            InsertNewPiece('a', 1, new Tower(Board, Color.White));
            InsertNewPiece('b', 1, new Horse(Board, Color.White));
            InsertNewPiece('c', 1, new Bishop(Board, Color.White));
            InsertNewPiece('d', 1, new Queen(Board, Color.White));
            InsertNewPiece('e', 1, new King(Board, Color.White, this));
            InsertNewPiece('f', 1, new Bishop(Board, Color.White));
            InsertNewPiece('g', 1, new Horse(Board, Color.White));
            InsertNewPiece('h', 1, new Tower(Board, Color.White));
            //Coluna 2
            InsertNewPiece('a', 2, new Peon(Board, Color.White, this));
            InsertNewPiece('b', 2, new Peon(Board, Color.White, this));
            InsertNewPiece('c', 2, new Peon(Board, Color.White, this));
            InsertNewPiece('d', 2, new Peon(Board, Color.White, this));
            InsertNewPiece('e', 2, new Peon(Board, Color.White, this));
            InsertNewPiece('f', 2, new Peon(Board, Color.White, this));
            InsertNewPiece('g', 2, new Peon(Board, Color.White, this));
            InsertNewPiece('h', 2, new Peon(Board, Color.White, this));

            //Coluna 8
            InsertNewPiece('a', 8, new Tower(Board, Color.Black));
            InsertNewPiece('b', 8, new Horse(Board, Color.Black));
            InsertNewPiece('c', 8, new Bishop(Board, Color.Black));
            InsertNewPiece('d', 8, new Queen(Board, Color.Black));
            InsertNewPiece('e', 8, new King(Board, Color.Black, this));
            InsertNewPiece('f', 8, new Bishop(Board, Color.Black));
            InsertNewPiece('g', 8, new Horse(Board, Color.Black));
            InsertNewPiece('h', 8, new Tower(Board, Color.Black));
            //Coluna 7
            InsertNewPiece('a', 7, new Peon(Board, Color.Black, this));
            InsertNewPiece('b', 7, new Peon(Board, Color.Black, this));
            InsertNewPiece('c', 7, new Peon(Board, Color.Black, this));
            InsertNewPiece('d', 7, new Peon(Board, Color.Black, this));
            InsertNewPiece('e', 7, new Peon(Board, Color.Black, this));
            InsertNewPiece('f', 7, new Peon(Board, Color.Black, this));
            InsertNewPiece('g', 7, new Peon(Board, Color.Black, this));
            InsertNewPiece('h', 7, new Peon(Board, Color.Black, this));
        }
    }
}
