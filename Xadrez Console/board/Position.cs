namespace tabuleiro
{
    internal class Position
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public Position(int linha, int coluna)
        {
            Line = linha;
            Column = coluna;
        }

        public void ValueSet(int line, int column)
        {
            Line = line;
            Column = column;
        }
        public override string ToString()
        {
            return Line +
                ", " +
                Column;
        }
    }
}
