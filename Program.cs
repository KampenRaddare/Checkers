/*
Bot one always has red checkers
Bot one always goes first
Bot two always has white checkers
Bot two always goes second

Bots can only be on black tiles

Red always start on top
White always stops on bottom

At the current time double jumping is not aloud
It will be hard to implement and it's not really
fair anyways. I consider it a house rule.

(#color-discrimination #more-racist-than-it-sounds)

The loser bot always has the losing message player before the other bots winning message.

This is how it should work and if there's in inaccuracies at least they'll be consistent so it's still fair
Until I can fully test this it might be hard to find them until that point in time.
In this programs current form it cannot be used, just observed. So please for the love of god don't try.

I MADE IT EVEN MORE C#-EY AND NOW I AM SO DONE WITH CHANGING CASINGS! xD 
Why meeeeeeeeeeeeeeee I mean, uh: whyMeeeeeeeeeeeeeee

On another note I expect that they are some MAJOR logical errors in VerifyMove
It took me a long time to write and lots of crying
I hope it works. Anndddd 300 lines. I'm sure there's ways to increase code reusabilty
at the current moment but I am too exhausted for that.

Also some optimizations with using if elses or switch statements is in order and should be tested
The only thing I need to test now is a window form to visualize plays and a completed bot.
Oh god.
*/
namespace Checkers {
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using System.Collections.Generic;
    public enum Tile {
        White = 0, Black = 1, WhiteChecker = 2, RedChecker = 3, KingedWhiteChecker = 4, KingedRedChecker = 5
    }
    public enum MoveType {
        LeftUp = 0, RightUp = 1, LeftDown = 2, RightDown = 3, LeftUpJump = 4, RightUpJump = 5, LeftDownJump = 6, RightDownJump = 7
    }
    public sealed class Program {
        public static Tile Board(int x,int y) {
            try {
                return _Board[x,y];
            } catch {
                return Tile.White;
            }
        }
        private const int BoardSize = 8; //Rows and columns. Always a square board.
        private const int MaximumSequentialInvalidMoves = 1;
        private const bool EnabledInvalidMovesEnd = true;
        private static List<Tile> BotOneTiles = new List<Tile>() { Tile.RedChecker,Tile.KingedRedChecker };
        private static List<Tile> BotTwoTiles = new List<Tile>() { Tile.WhiteChecker,Tile.KingedWhiteChecker };
        private static Tile[,] _Board = new Tile[BoardSize, BoardSize];
        private static Bot Bot1 = new Bots.Reggie();
        private static Bot Bot2 = new Bots.Dave();
        private static CheckerBoard checkerBoard;
        private static void Main() {
            Console.Title = "Checkers";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(Bot1 != null && Bot2 != null) {
                Setup();
                Console.WriteLine($"Setup complete.{Environment.NewLine}Press any key to start game.");
                StartGame();
            } else {
                Console.WriteLine("One or more bots are nulled.");
            }
            Console.WriteLine("Press any key to exit program.");
            Console.ReadKey(true);
            Application.Exit();
        }
        private static void Setup() { //I think this needs more for loops.
            int tileNumber = 0;
            for(int y = 0;y < BoardSize;y += 1) {
                for(int x = 0;x < BoardSize;x += 1) {
                    if(IsOdd(tileNumber)) {
                        _Board[x,y] = Tile.White;
                    } else {
                        _Board[x,y] = Tile.Black;
                    }
                    tileNumber += 1;
                }
                tileNumber += 1;
            }
            for(int y = 0;y < 2;y += 1) {
                for(int x = 0;x < BoardSize;x += 1) {
                    if(Board(x,y) == Tile.Black) {
                        _Board[x,y] = Tile.RedChecker;
                    }
                }
            }
            for(int y = BoardSize-1;y > BoardSize-3;y -= 1) {
                for(int x = 0;x < BoardSize;x += 1) {
                    if(Board(x,y) == Tile.Black) {
                        _Board[x,y] = Tile.WhiteChecker;
                    }
                }
            }
            Bot1.Setup(1,BoardSize);
            Bot2.Setup(2,BoardSize);
            checkerBoard = new CheckerBoard(BoardSize);
            new Thread(RunCheckerBoard).Start();
            checkerBoard.UpdateBoard(_Board);
        }
        private static void RunCheckerBoard() {
            Application.Run(checkerBoard);
        }
        private static void StartGame() {
            int turn = 1;
            int botOneInvalidMoves = 0;
            int botTwoInvalidMoves = 0;
            bool tooManyInvalidMovesReached = false;
            while(TileExists(BotOneTiles) && TileExists(BotTwoTiles) && !tooManyInvalidMovesReached) {
                Console.ReadKey(true);
                Move botMove = null;
                int whoseTurn = IsOdd(turn) ? 1 : 2;
                bool turnSkipped = false;
                switch(whoseTurn) {
                    case 1:
                        try {
                            botMove = Bot1.Turn();
                        } catch {
                            Console.WriteLine("Exception caught on Bot 1's turn.");
                        }
                        break;
                    case 2:
                        try {
                            botMove = Bot2.Turn();
                        } catch {
                            Console.WriteLine("Exception caught on Bot 2's turn.");
                        }
                        break;
                }
                if(botMove != null) {
                    if(!VerifyMove(botMove,whoseTurn)) {
                        turnSkipped = true;
                        Console.WriteLine($"Illegal move by Bot {whoseTurn}.");
                    } else {
                        Console.WriteLine($"Bot {whoseTurn}: {botMove.Piece.X},{botMove.Piece.Y} @ {botMove.MoveType}");
                        bool isKinged = Board(botMove.Piece.X,botMove.Piece.Y) ==
                            Tile.KingedRedChecker || Board(botMove.Piece.X,botMove.Piece.Y) == Tile.KingedWhiteChecker ?
                            true : false;
                        _Board[botMove.Piece.X,botMove.Piece.Y] = Tile.Black;
                        Tile replaceTile = Tile.White;
                        switch(whoseTurn) {
                            case 1:
                                if(isKinged) {
                                    replaceTile = Tile.KingedRedChecker;
                                } else {
                                    replaceTile = Tile.RedChecker;
                                }
                                break;
                            case 2:
                                if(isKinged) {
                                    replaceTile = Tile.KingedWhiteChecker;
                                } else {
                                    replaceTile = Tile.WhiteChecker;
                                }
                                break;
                        }
                        switch(botMove.MoveType) {
                            case MoveType.LeftDown: // MINUS PLUS
                                _Board[botMove.Piece.X - 1,botMove.Piece.Y + 1] = replaceTile;
                                break;
                            case MoveType.RightDown: // PLUS PLUS
                                _Board[botMove.Piece.X + 1,botMove.Piece.Y + 1] = replaceTile;
                                break;
                            case MoveType.LeftUp: // MINUS MINUS
                                _Board[botMove.Piece.X - 1,botMove.Piece.Y - 1] = replaceTile;
                                break;
                            case MoveType.RightUp: // PLUS MINUS
                                _Board[botMove.Piece.X + 1,botMove.Piece.Y - 1] = replaceTile;
                                break;
                            case MoveType.LeftUpJump: // MINUS MINUS
                                _Board[botMove.Piece.X - 1,botMove.Piece.Y - 1] = Tile.Black;
                                _Board[botMove.Piece.X - 2,botMove.Piece.Y - 2] = replaceTile;
                                break;
                            case MoveType.RightUpJump:  // PLUS MINUS
                                _Board[botMove.Piece.X + 1,botMove.Piece.Y - 1] = Tile.Black;
                                _Board[botMove.Piece.X + 2,botMove.Piece.Y - 2] = replaceTile;
                                break;
                            case MoveType.LeftDownJump: // MINUS PLUS
                                _Board[botMove.Piece.X - 1,botMove.Piece.Y + 1] = Tile.Black;
                                _Board[botMove.Piece.X - 2,botMove.Piece.Y + 2] = replaceTile;
                                break;
                            case MoveType.RightDownJump: // PLUS PLUS
                                _Board[botMove.Piece.X + 1,botMove.Piece.Y + 1] = Tile.Black;
                                _Board[botMove.Piece.X + 2,botMove.Piece.Y + 2] = replaceTile;
                                break;
                        }
                        int y = 0;
                        for(int x = 0;x < BoardSize;x += 1) {
                            if(Board(x,y) == Tile.WhiteChecker) {
                                _Board[x,y] = Tile.KingedWhiteChecker;
                                Console.WriteLine("Bot 2 has a new king!");
                            }
                        }
                        y = BoardSize - 1;
                        for(int x = 0;x < BoardSize;x += 1) {
                            if(Board(x,y) == Tile.RedChecker) {
                                _Board[x,y] = Tile.KingedRedChecker;
                                Console.WriteLine("Bot 1 has a new king!");
                            }
                        }
                    }
                } else {
                    turnSkipped = true;
                    Console.WriteLine($"No move provided by Bot {whoseTurn}.");
                }
                if(turnSkipped) {
                    if(whoseTurn == 1) {
                        botOneInvalidMoves += 1;
                    } else {
                        botTwoInvalidMoves += 1;
                    }
                } else {
                    if(whoseTurn == 1) {
                        botOneInvalidMoves = 0;
                    } else {
                        botTwoInvalidMoves = 0;
                    }
                }
                if(EnabledInvalidMovesEnd) {
                    tooManyInvalidMovesReached = botOneInvalidMoves >= MaximumSequentialInvalidMoves || botTwoInvalidMoves >= MaximumSequentialInvalidMoves;
                }
                turn += 1;
                checkerBoard.UpdateBoard(_Board);
            }
            int winner;
            if(tooManyInvalidMovesReached) {
                if(botOneInvalidMoves == MaximumSequentialInvalidMoves) {
                    winner = 2;
                } else {
                    winner = 1;
                }
            } else {
                if(TileExists(BotOneTiles)) {
                    winner = 1;
                } else {
                    winner = 2;
                }
            }
            EndGame(tooManyInvalidMovesReached,winner);
        }
        private static void EndGame(bool endedByFailure,int winner) {
            switch(winner) {
                case 1:
                    if(endedByFailure) {
                        Console.WriteLine("Bot 2 hit maximum sequential invalid moves.");
                    } else {
                        Console.WriteLine("Bot 2 has no more checkers.");
                    }
                    Console.WriteLine("Bot 1 wins!");
                    Console.WriteLine($"Bot 2 says: {Bot2.LoseMessage()}");
                    Console.WriteLine($"Bot 1 says: {Bot1.WinMessage()}");
                    break;
                case 2:
                    if(endedByFailure) {
                        Console.WriteLine("Bot 1 hit maximum sequential invalid moves.");
                    } else {
                        Console.WriteLine("Bot 1 has no more checkers.");
                    }
                    Console.WriteLine("Bot 2 wins!");
                    Console.WriteLine($"Bot 1 says: {Bot2.LoseMessage()}");
                    Console.WriteLine($"Bot 2 says: {Bot2.WinMessage()}");
                    break;
            }
        }
        public static bool IsOdd(int value) {
            return value % 2 != 0;
        }
        private static bool VerifyUp(Move requestedMove,int botNumber) {
            bool moveValid = true;
            Tile checkOne = Tile.White;
            Tile checkTwo = Tile.White;
            switch(botNumber) {
                case 1:
                    checkOne = Tile.WhiteChecker;
                    checkTwo = Tile.KingedWhiteChecker;
                    break;
                case 2:
                    checkOne = Tile.RedChecker;
                    checkTwo = Tile.KingedRedChecker;
                    break;
            }
            switch(requestedMove.MoveType) {
                case MoveType.LeftUp:
                    if(Board(requestedMove.Piece.X - 1,requestedMove.Piece.Y - 1) != Tile.Black) {
                        moveValid = false;
                    }
                    break;
                case MoveType.RightUp:
                    if(Board(requestedMove.Piece.X + 1,requestedMove.Piece.Y - 1) != Tile.Black) {
                        moveValid = false;
                    }
                    break;
                case MoveType.LeftUpJump:
                    Tile halfWayPiece1 = Board(requestedMove.Piece.X - 1,requestedMove.Piece.Y - 1);
                    if(halfWayPiece1 == checkOne || halfWayPiece1 == checkTwo) {
                        if(Board(requestedMove.Piece.X - 2,requestedMove.Piece.Y - 2) != Tile.Black) {
                            moveValid = false;
                        }
                    } else {
                        moveValid = false;
                    }
                    break;
                case MoveType.RightUpJump:
                    Tile halfWayPiece2 = Board(requestedMove.Piece.X + 1,requestedMove.Piece.Y - 1);
                    if(halfWayPiece2 == checkOne || halfWayPiece2 == checkTwo) {
                        if(Board(requestedMove.Piece.X + 2,requestedMove.Piece.Y - 2) != Tile.Black) {
                            moveValid = false;
                        }
                    } else {
                        moveValid = false;
                    }
                    break;
                default:
                    moveValid = false;
                    break;
            }
            return moveValid;
        }
        private static bool VerifyDown(Move requestedMove,int botNumber) {
            bool moveValid = true;
            Tile checkOne = Tile.White;
            Tile checkTwo = Tile.White;
            switch(botNumber) {
                case 1:
                    checkOne = Tile.WhiteChecker;
                    checkTwo = Tile.KingedWhiteChecker;
                    break;
                case 2:
                    checkOne = Tile.RedChecker;
                    checkTwo = Tile.KingedRedChecker;
                    break;
            }
            switch(requestedMove.MoveType) {
                case MoveType.LeftDown:
                    if(Board(requestedMove.Piece.X - 1,requestedMove.Piece.Y + 1) != Tile.Black) {
                        moveValid = false;
                    }
                    break;
                case MoveType.RightDown:
                    if(Board(requestedMove.Piece.X + 1,requestedMove.Piece.Y + 1) != Tile.Black) {
                        moveValid = false;
                    }
                    break;
                case MoveType.LeftDownJump:
                    Tile halfWayPiece1 = Board(requestedMove.Piece.X - 1,requestedMove.Piece.Y + 1);
                    if(halfWayPiece1 == checkOne || halfWayPiece1 == checkTwo) {
                        if(Board(requestedMove.Piece.X - 2,requestedMove.Piece.Y - 2) != Tile.Black) {
                            moveValid = false;
                        }
                    } else {
                        moveValid = false;
                    }
                    break;
                case MoveType.RightDownJump:
                    Tile halfWayPiece2 = Board(requestedMove.Piece.X + 1,requestedMove.Piece.Y + 1);
                    if(halfWayPiece2 == checkOne || halfWayPiece2 == checkTwo) {
                        if(Board(requestedMove.Piece.X + 2,requestedMove.Piece.Y + 2) != Tile.Black) {
                            moveValid = false;
                        }
                    } else {
                        moveValid = false;
                    }
                    break;
                default:
                    moveValid = false;
                    break;
            }
            return moveValid;
        }
        public static bool VerifyMove(Move requestedMove,int botNumber) {
            bool moveValid = true;
                switch(botNumber) {
                    case 1:
                        switch(Board(requestedMove.Piece.X,requestedMove.Piece.Y)) {
                            case Tile.RedChecker:
                                moveValid = VerifyDown(requestedMove,botNumber);
                                break;
                            case Tile.KingedRedChecker:
                                moveValid = VerifyDown(requestedMove,botNumber) && VerifyUp(requestedMove,botNumber);
                                break;
                            default:
                                moveValid = false;
                                break;
                        }
                        break;
                    case 2:
                        switch(Board(requestedMove.Piece.X,requestedMove.Piece.Y)) {
                            case Tile.WhiteChecker:
                                moveValid = VerifyUp(requestedMove,botNumber);
                                break;
                            case Tile.KingedWhiteChecker:
                                moveValid = VerifyUp(requestedMove,botNumber) && VerifyDown(requestedMove,botNumber);
                                break;
                            default:
                                moveValid = false;
                                break;
                        }
                        break;
                }
            return moveValid;
        }
        private static bool TileExists(List<Tile> tiles) {
            for(int y = 0;y < BoardSize;y += 1) {
                for(int x = 0;x < BoardSize;x += 1) {
                    foreach(Tile tile in tiles) {
                        if(Board(x,y) == tile) {
                            return true;
                            break;
                        }
                    }
                }
            }
            return false;
        }
    }
}