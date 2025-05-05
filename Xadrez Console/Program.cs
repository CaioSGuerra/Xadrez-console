using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PartidaDeXadrez partida = new PartidaDeXadrez();

            while (!partida.terminada)
            {
                try
                {
                    Console.Clear();
                    Tela.imprimirPartida(partida);

                    Console.WriteLine();
                    Console.WriteLine("Turno: " + partida.turno);
                    Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.lerPosicaoXadez().toPosicao();
                    partida.validarPosicaoOrigem(origem);

                    bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tab, posicoesPossiveis);

                    Console.WriteLine();
                    Console.Write("Destinho: ");
                    Posicao destino = Tela.lerPosicaoXadez().toPosicao();
                    partida.validarPosicaoDeDestino(origem, destino);

                    partida.realizaJogada(origem, destino);
                }
                catch (TabuleiroException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
        }
    }
}
