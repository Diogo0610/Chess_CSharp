using BoardEntities;
using BoardExceptions;

namespace ChessPieces
{
    internal class ChessMatch
    {
        public Board board { get; private set; }
        public int turn { get; private set; }
        public Color currentPlayer { get; private set; }
        public bool Finished { get; private set; }
        private HashSet<Piece> pieces;
        private HashSet<Piece> catched;
        public bool check { get; private set; }
        public Piece vunerableEnPassant { get; private set; }

        public ChessMatch()
        {
            board = new Board(8, 8);
            turn = 1;
            currentPlayer = Color.White;
            Finished = false;
            vunerableEnPassant = null;
            pieces = new HashSet<Piece>();
            catched = new HashSet<Piece>();
            check = false;
            PutPieces();
        }

        //Cuida da realização do movimento da peça, lendo qual é a origem da peça e a retirando da posição
        //para coloca-lá na posição de destino informada. Também remove as peças capturadas do tabuleiro e as
        //adiciona na HashSet, caso haja alguma na posição de destino.
        //Define o funcionamento das jogadas especiais, como o Roque menor e maior e o En Passant.
        public Piece PieceMovement(Position origin, Position destination)
        {
            Piece p = board.RemovePiece(origin);
            p.IncreaseNumMovements();
            Piece catchedPiece = board.RemovePiece(destination);
            board.PutPiece(p, destination);
            
            if(catchedPiece != null)
            {
                catched.Add(catchedPiece);
            }

            //#Castling kingside
            if (p is King && destination.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Line, origin.Column + 3);
                Position destinationT = new Position(origin.Line, origin.Column + 1);
                Piece t = board.RemovePiece(originT);
                t.IncreaseNumMovements();
                board.PutPiece(t, destinationT);
            }
            //#Castling queenside
            if (p is King && destination.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Line, origin.Column - 4);
                Position destinationT = new Position(origin.Line, origin.Column - 1);
                Piece t = board.RemovePiece(originT);
                t.IncreaseNumMovements();
                board.PutPiece(t, destinationT);
            }

            //#En passant
            if(p is Pawn)
            {
                if(origin.Column != destination.Column && catchedPiece == null)
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
                    catchedPiece = board.RemovePiece(posP);
                    catched.Add(catchedPiece);
                }
            }

            return catchedPiece;
        }

        //Desfaz o movimento do jogador atual casa tenha realizado um movimento inválido.
        public void ReturnMove(Position origin, Position destination, Piece catchedPiece)
        {
            Piece p = board.RemovePiece(destination);
            p.DecreaseNumMovements();
            if(catchedPiece != null)
            {
                board.PutPiece(catchedPiece, destination);
                catched.Remove(catchedPiece);
            }
            board.PutPiece(p, origin);

            //#Castling kingside
            if (p is King && destination.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Line, origin.Column + 3);
                Position destinationT = new Position(origin.Line, origin.Column + 1);
                Piece t = board.RemovePiece(destinationT);
                t.DecreaseNumMovements();
                board.PutPiece(t, originT);
            }
            //#Castling queenside
            if (p is King && destination.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Line, origin.Column - 4);
                Position destinationT = new Position(origin.Line, origin.Column - 1);
                Piece t = board.RemovePiece(destinationT);
                t.DecreaseNumMovements();
                board.PutPiece(t, originT);
            }

            //#En passant
            if(p is Pawn)
            {
                if(origin.Column != destination.Column && catchedPiece == vunerableEnPassant)
                {
                    Piece pawn = board.RemovePiece(destination);
                    Position posP;
                    if(p.Color == Color.White)
                    {
                        posP = new Position(3, destination.Column);
                    }
                    else
                    {
                        posP = new Position(4, destination.Column);
                    }
                    board.PutPiece(pawn, posP);
                }
            }
        }

        //Confirma se o movimento é válido, além de definir se o Rei está em xeque ou se já sofreu xeque-mate.
        //Transforma o peão em rainha caso chegue na ultima casa do adversário.
        //Valida se o peão está vunerável a sofrer um En Passant.
        public void ConfirmMove(Position origin, Position destination)
        {
            Piece catchedPiece = PieceMovement(origin, destination);

            if (Check(currentPlayer))
            {
                ReturnMove(origin, destination, catchedPiece);
                throw new BoardException("You can't put yourself in check!");
            }

            Piece p = board.Piece(destination);

            if (p is Pawn)
            {
                if (p.Color == Color.White && destination.Line ==0 || p.Color == Color.Black && destination.Line == 7)
                {
                    p = board.RemovePiece(destination);
                    pieces.Remove(p);
                    Piece queen = new Queen(board, p.Color);
                    board.PutPiece(queen, destination);
                    pieces.Add(queen);
                }
            }

            if (Check(Enemy(currentPlayer)))
            {
                check = true;
            }
            else
            {
                check = false;
            }

            if (CheckMate(Enemy(currentPlayer)))
            {
                Finished = true;
            }
            else
            {
                turn++;
                ChangePlayer();
            }

            
            //#En passant
            if (p is Pawn && (destination.Line == origin.Line - 2 || destination.Line == origin.Line + 2))
            {
                vunerableEnPassant = p;
            }
            else
            {
                vunerableEnPassant = null;
            }
        }
        
        //Valida e informa ao jogador sobre os erros que podem ocorrer durante a seleção de peças. 
        public void ValidateOriginPosition(Position pos)
        {
            if(board.Piece(pos) == null)
            {
                throw new BoardException("There is no piece in this position!");
            }
            if (currentPlayer != board.Piece(pos).Color)
            {
                throw new BoardException("This is not your piece!");
            }
            if (!board.Piece(pos).HasPossibleMoves())
            {
                throw new BoardException("There is no possible move for this piece!");
            }
            if (pos.Line > board.Lines || pos.Column > board.Columns)
            {
                throw new BoardException("There is no possible move for this piece!");
            }
        }

        //Valida se a posição de destino escolhida está de acordo com as regras de movimentação da peça.
        public void ValidateDestinationPosition(Position origin, Position destination)
        {
            if (!board.Piece(origin).CanMoveFor(destination))
            {
                throw new BoardException("Invalid destination position!");
            }
        }

        //Troca os jogadores e o controle das peças.
        private void ChangePlayer()
        {
            if(currentPlayer == Color.White)
            {
                currentPlayer = Color.Black;
            }
            else
            {
                currentPlayer = Color.White;
            }
        }

        public HashSet<Piece> CatchedPieces(Color color)
        {
            HashSet<Piece> temp = new HashSet<Piece>();
            foreach(Piece x in catched)
            {
                if(x.Color == color)
                {
                    temp.Add(x);
                }
            }
            return temp;
        }

        public HashSet<Piece> PiecesInGame(Color color)
        {
            HashSet<Piece> temp = new HashSet<Piece>();
            foreach (Piece x in pieces)
            {
                if (x.Color == color)
                {
                    temp.Add(x);
                }
            }
            temp.ExceptWith(CatchedPieces(color));
            return temp;
        }

        //Define quais cores são adversárias.
        private Color Enemy(Color color)
        {
            if(color == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        //Realiza a checagem se há um Rei no jogo para cada lado.
        private Piece King(Color color)
        {
            foreach(Piece x in PiecesInGame(color))
            {
                if(x is King)
                {
                    return x;
                }
            }
            return null;
        }

        //Cria a condição de Xeque para o Rei
        public bool Check(Color color)
        {
            Piece king = King(color);

            foreach (Piece x in PiecesInGame(Enemy(color)))
            {
                bool[,] mat = x.PossibleMoves();
                if (mat[king.Position.Line, king.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        //Define as condições para que ocorra o Xeque-mate
        public bool CheckMate(Color color)
        {
            if (!Check(color))
            {
                return false;
            }

            foreach (Piece x in PiecesInGame(color))
            {
                bool[,] mat = x.PossibleMoves();
                for (int i = 0; i < board.Lines; i++)
                {
                    for (int j = 0; j < board.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origin = x.Position;
                            Position destination = new Position(i, j);
                            Piece catchedPiece = PieceMovement(origin, destination);
                            bool check = Check(color);
                            ReturnMove(origin, destination, catchedPiece);
                            if (!check)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        //Adiciona as peças no tabuleiro
        public void PutNewPiece(char column, int line, Piece piece)
        {
            board.PutPiece(piece, new ChessPosition(column, line).toPosition());
            pieces.Add(piece);
        }

        //Lista das peças e em quais posições serão colocadas, além de definir suas cores.
        private void PutPieces()
        {
            PutNewPiece('a', 1, new Tower(board, Color.White));
            PutNewPiece('b', 1, new Knight(board, Color.White));
            PutNewPiece('c', 1, new Bishop(board, Color.White));
            PutNewPiece('d', 1, new Queen(board, Color.White));
            PutNewPiece('e', 1, new King(board, Color.White, this));
            PutNewPiece('f', 1, new Bishop(board, Color.White));
            PutNewPiece('g', 1, new Knight(board, Color.White));
            PutNewPiece('h', 1, new Tower(board, Color.White));
            PutNewPiece('a', 2, new Pawn(board, Color.White, this));
            PutNewPiece('b', 2, new Pawn(board, Color.White, this));
            PutNewPiece('c', 2, new Pawn(board, Color.White, this));
            PutNewPiece('d', 2, new Pawn(board, Color.White, this));
            PutNewPiece('e', 2, new Pawn(board, Color.White, this));
            PutNewPiece('f', 2, new Pawn(board, Color.White, this));
            PutNewPiece('g', 2, new Pawn(board, Color.White, this));
            PutNewPiece('h', 2, new Pawn(board, Color.White, this));

            PutNewPiece('a', 8, new Tower(board, Color.Black));
            PutNewPiece('b', 8, new Knight(board, Color.Black));
            PutNewPiece('c', 8, new Bishop(board, Color.Black));
            PutNewPiece('d', 8, new Queen(board, Color.Black));
            PutNewPiece('e', 8, new King(board, Color.Black, this));
            PutNewPiece('f', 8, new Bishop(board, Color.Black));
            PutNewPiece('g', 8, new Knight(board, Color.Black));
            PutNewPiece('h', 8, new Tower(board, Color.Black));
            PutNewPiece('a', 7, new Pawn(board, Color.Black, this));
            PutNewPiece('b', 7, new Pawn(board, Color.Black, this));
            PutNewPiece('c', 7, new Pawn(board, Color.Black, this));
            PutNewPiece('d', 7, new Pawn(board, Color.Black, this));
            PutNewPiece('e', 7, new Pawn(board, Color.Black, this));
            PutNewPiece('f', 7, new Pawn(board, Color.Black, this));
            PutNewPiece('g', 7, new Pawn(board, Color.Black, this));
            PutNewPiece('h', 7, new Pawn(board, Color.Black, this));

        }
    }
}
