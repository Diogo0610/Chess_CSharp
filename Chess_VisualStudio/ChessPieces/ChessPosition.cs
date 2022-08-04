using BoardEntities;

namespace ChessPieces
{
    internal class ChessPosition
    {
        public char Column { get; set; }
        public int Line { get; set; }

        public ChessPosition(char column, int line)
        {
            Column = column;
            Line = line;
        }

       // Converte a linha e a coluna informadas para números aceitos pela matriz criada,
       //não forçando o usuário a precisar coverter a posição desejada para uma que a matriz aceite.
        public Position toPosition()
        {
            return new Position(8 - Line, Column - 'a');
        }

        public override string ToString()
        {
            return "" + Column + Line;
        }
    }
}
