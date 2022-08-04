using System;
using System.Collections.Generic;
using BoardEntities;
using ChessPieces;

namespace Chess
{
    internal class Screen
    {
        //Chama a função para criar o tabuleiro e exibe informações da partida. Verifica quando a partida acaba.
        public static void PrintBoard(ChessMatch match)
        {
            CreateBoard(match.board);
            Console.WriteLine();
            PrintCatchedPieces(match);
            Console.WriteLine();
            Console.WriteLine("Turn: " + match.turn);

            if (!match.Finished)
            {
                Console.WriteLine("Waiting for play: " + match.currentPlayer);
                if (match.check)
                {
                    Console.WriteLine("CHECK!");
                }
            }
            else
            {
                Console.WriteLine("CHECKMATE!");
                Console.WriteLine("Winner: " + match.currentPlayer);
            }
        }

        //Cria o layout que mostra as peças capturadas de cada lado.
        public static void PrintCatchedPieces(ChessMatch match)
        {
            Console.WriteLine("Captured Pieces: ");
            Console.Write("White: ");
            PrintGroup(match.CatchedPieces(Color.White));
            Console.WriteLine();
            Console.Write("Black: ");
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintGroup(match.CatchedPieces(Color.Black));
            Console.ForegroundColor = temp;
            Console.WriteLine();
        }

        //Preenche o layout com as peças capturadas.
        public static void PrintGroup(HashSet<Piece> group)
        {
            Console.Write("[");
            foreach (Piece x in group)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        // Cria uma matriz com o número de linhas e colunas definidos no construtor da classe ChessMatch 
        public static void CreateBoard(Board board)
        {
            for(int i = 0; i < board.Lines; i++)
            {
                Console.Write((8 - i) + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    PrintPiece(board.Piece(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        //Recria a matriz marcando os movimentos possíveis para a peça selecionada
        //(funciona apenas ao selecionar uma peça, depois a matriz retorna para sua cor normal).
        public static void CreateBoard(Board board, bool[,] possibleMoves)
        {
            ConsoleColor originalBackground = Console.BackgroundColor;
            ConsoleColor newBackground = ConsoleColor.DarkGray;

            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write((8 - i) + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    if(possibleMoves[i, j])
                    {
                        Console.BackgroundColor = newBackground;
                    }
                    else
                    {
                        Console.BackgroundColor = originalBackground;
                    }

                    PrintPiece(board.Piece(i, j));
                    Console.BackgroundColor = originalBackground;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = originalBackground;
        }

        //Lê o texto digitado pelo usuário e pega o primeiro dígito da string como a posição da coluna [0]
        // e o segundo [1] como o número de linha
        public static ChessPosition ReadPosition()
        {
            string s = Console.ReadLine();
            char column = s[0];
            int line = int.Parse(s[1] + "");
            return new ChessPosition(column, line);
        }

        //Coloca as peças no tabuleiro. Caso não haja uma peça na posição, um hífen é colocado,
        //indicando a casa vazia. Caso tenha uma peça, a estrutura switch-case irá analisar a cor informada e
        //irá imprimir a peça com essa cor.
        //! O programa está programado apenas para aceitar as cores brancas e pretas,
        //qualquer outra pode causar erro!
        public static void PrintPiece(Piece piece)
        {
            if(piece == null)
            {
                Console.Write("- ");
            }
            else 
            { 
                ConsoleColor aux = Console.ForegroundColor;
                switch (piece.Color)
                {
                    case Color.White:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(piece);
                        Console.ForegroundColor = aux;
                        break;
                    case Color.Black:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(piece);
                        Console.ForegroundColor = aux;
                        break;
                    case Color.Yellow:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(piece);
                        Console.ForegroundColor = aux;
                        break;
                    case Color.Green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(piece);
                        Console.ForegroundColor = aux;
                        break;
                    case Color.Red:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(piece);
                        Console.ForegroundColor = aux;
                        break;
                    case Color.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(piece);
                        Console.ForegroundColor = aux;
                        break;
                }
                Console.Write(" ");
            }
        }
    }
}
