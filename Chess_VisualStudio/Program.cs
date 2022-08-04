using BoardEntities;
using ChessPieces;
using BoardExceptions;

namespace Chess
{
   
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GAME RULES:");
            Console.WriteLine("The only input allowed is the letter of the COLUMN in LOWERCASE LETTER (a to h)," +
                " followed by the LINE NUMBER (1 to 8).");
            Console.WriteLine("The game follows the classic chess rules, " +
                "like movements and turns. The player also controls both sides.");
            Console.WriteLine("In case of error, press any key to continue.");
            Console.WriteLine();
            Console.WriteLine("Press any key to start the game...");
            Console.ReadLine();

            try
            {
                ChessMatch match = new ChessMatch();

                while (!match.Finished)
                {
                    try
                    {
                        Console.Clear();
                        Screen.PrintBoard(match);

                        Console.WriteLine();
                        Console.Write("Origin: ");
                        Position origin = Screen.ReadPosition().toPosition();
                        match.ValidateOriginPosition(origin);

                        bool[,] possibleMoves = match.board.Piece(origin).PossibleMoves();

                        Console.Clear();
                        Screen.CreateBoard(match.board, possibleMoves);


                        Console.WriteLine();
                        Console.Write("Destination: ");
                        Position destination = Screen.ReadPosition().toPosition();
                        match.ValidateDestinationPosition(origin, destination);

                        match.ConfirmMove(origin, destination);
                    }
                    catch (BoardException e)
                    {
                        Console.Write(e.Message);
                        Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.Write("Invalid Position!");
                        Console.ReadLine();
                    }

                }
                Console.Clear();
                Screen.CreateBoard(match.board);
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }
            

            Console.ReadLine();
        }
    }
}