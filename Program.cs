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
    using System.Collections.Generic;
    public enum Tile {
        White = 0, Black = 1, WhiteChecker = 2, RedChecker = 3, KingedWhiteChecker = 4, KingedRedChecker = 5
    }
    public enum MoveType {
        LeftUp = 0, RightUp = 1, LeftDown = 2, RightDown = 3, LeftUpJump = 4, RightUpJump = 5, LeftDownJump = 6, RightDownJump = 7
    }
    public sealed class Program {
        public const int BoardSize = 8; //Rows and columns. Always a square board.
        public const int MaximumSequentialInvalidMoves = 4;
        private const bool PrintBoardEnabled = false;
        private static List<Tile> BotOneTiles = new List<Tile>() { Tile.RedChecker,Tile.KingedRedChecker };
        private static List<Tile> BotTwoTiles = new List<Tile>() { Tile.WhiteChecker,Tile.KingedWhiteChecker };
        public static Tile Board(int x, int y) {
            try {
                return _Board[x,y];
            } catch {
                return Tile.White;
            }
        }
        private static Tile[,] _Board = new Tile[BoardSize, BoardSize];
        private static Bot Bot1 = new Bots.Reggie();
        private static Bot Bot2 = new Bots.Dave();
        private static void Main() {
            Console.Title = "Checkers";
            if(Bot1 != null && Bot2 != null) {
                Setup();
                Console.WriteLine($"Setup complete.{Environment.NewLine}Press any key to start game.");
                Console.ReadKey(true);
                StartGame();
            } else {
                Console.WriteLine("One or more bots are nulled.");
            }
            Console.WriteLine("Press any key to exit program.");
            Console.ReadKey(true);
        }
        private static void Setup() { //I think this needs more for loops.
            int tileNumber = 0;
            for(int row = 0;row < BoardSize;row += 1) { //Setup black and white tiles
                for(int column = 0;column < BoardSize;column += 1) {
                    if(IsOdd(tileNumber)) {
                        _Board[row,column] = Tile.White;
                    } else {
                        _Board[row,column] = Tile.Black;
                    }
                    tileNumber += 1;
                }
                tileNumber += 1;
            }
            for(int row = 0;row < 2;row += 1) { //Setup red checkers (Bot 1)
                for(int column = 0;column < BoardSize;column += 1) {
                    if(_Board[row,column] == Tile.Black) {
                        _Board[row,column] = Tile.RedChecker;
                    }
                }
            }
            for(int row = BoardSize - 1;row > BoardSize - 3;row -= 1) { //Setup white checkers (Bot 2)
                for(int column = 0;column < BoardSize;column += 1) {
                    if(_Board[row,column] == Tile.Black) {
                        _Board[row,column] = Tile.WhiteChecker;
                    }
                }
            }
            PrintBoard();
            Bot1.Setup(1);
            Bot2.Setup(2);
        }
        private static void PrintBoard() {
            if(PrintBoardEnabled) {
                for(int row = 0;row < BoardSize;row += 1) {
                    for(int column = 0;column < BoardSize;column += 1) {
                        Console.Write((byte)_Board[row,column]);
                    }
                    Console.Write(Environment.NewLine);
                }
            }
        }
        private static void StartGame() {
            int turn = 1;
            int botOneInvalidMoves = 0;
            int botTwoInvalidMoves = 0;
            bool tooManyInvalidMovesReached = false;
            while((TileExists(BotOneTiles) && TileExists(BotTwoTiles)) && !tooManyInvalidMovesReached) {
                Move botMove = null;
                int whoseTurn = IsOdd(turn) ? 1 : 2;
                bool turnSkipped = false;
                switch(whoseTurn) {
                    case 1:
                        try {
                            botMove = Bot1.Turn();
                        } catch {
                            Console.WriteLine("Caught an exception on Bot 1's turn.");
                        }
                        break;
                    case 2:
                        try {
                            botMove = Bot2.Turn();
                        } catch {
                            Console.WriteLine("Caught an exception on Bot 2's turn.");
                        }
                        break;
                }
                if(botMove != null) {
                    if(!VerifyMove(botMove,whoseTurn)) {
                        turnSkipped = true;
                        Console.WriteLine($"Illegal move by Bot {whoseTurn}.");
                    } else {
                        bool isKinged = _Board[botMove.Piece.X,botMove.Piece.Y] ==
                            Tile.KingedRedChecker || _Board[botMove.Piece.X,botMove.Piece.Y] == Tile.KingedWhiteChecker ?
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
                        break;
                    }
                } else {
                    turnSkipped = true;
                    Console.WriteLine($"Null move provided by Bot {whoseTurn}.");
                }
                if(turnSkipped) {
                    if(whoseTurn == 1) {
                        botOneInvalidMoves += 1;
                    } else {
                        botTwoInvalidMoves += 1;
                    }
                    Console.WriteLine($"Turn {turn} skipped.");
                } else {
                    if(whoseTurn == 1) {
                        botOneInvalidMoves = 0;
                    } else {
                        botTwoInvalidMoves = 0;
                    }
                }
                tooManyInvalidMovesReached = botOneInvalidMoves >= MaximumSequentialInvalidMoves || botTwoInvalidMoves >= MaximumSequentialInvalidMoves;
                turn += 1;
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
                    Console.WriteLine($"Bot 2: {Bot2.LoseMessage()}");
                    Console.WriteLine($"Bot 1: {Bot1.WinMessage()}");
                    break;
                case 2:
                    if(endedByFailure) {
                        Console.WriteLine("Bot 1 hit maximum sequential invalid moves.");
                    } else {
                        Console.WriteLine("Bot 1 has no more checkers.");
                    }
                    Console.WriteLine("Bot 2 wins!");
                    Console.WriteLine($"Bot 1: {Bot2.LoseMessage()}");
                    Console.WriteLine($"Bot 2: {Bot2.WinMessage()}");
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
                    if(_Board[requestedMove.Piece.X - 1,requestedMove.Piece.Y - 1] != Tile.Black) {
                        moveValid = false;
                    }
                    break;
                case MoveType.RightUp:
                    if(_Board[requestedMove.Piece.X + 1,requestedMove.Piece.Y - 1] != Tile.Black) {
                        moveValid = false;
                    }
                    break;
                case MoveType.LeftUpJump:
                    Tile halfWayPiece1 = _Board[requestedMove.Piece.X - 1,requestedMove.Piece.Y - 1];
                    if(halfWayPiece1 != checkOne && halfWayPiece1 != checkTwo) {
                        moveValid = false;
                    }
                    break;
                case MoveType.RightUpJump:
                    Tile halfWayPiece2 = _Board[requestedMove.Piece.X + 1,requestedMove.Piece.Y - 1];
                    if(halfWayPiece2 != checkOne && halfWayPiece2 != checkTwo) {
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
                    if(_Board[requestedMove.Piece.X - 1,requestedMove.Piece.Y + 1] != Tile.Black) {
                        moveValid = false;
                    }
                    break;
                case MoveType.RightDown:
                    if(_Board[requestedMove.Piece.X + 1,requestedMove.Piece.Y + 1] != Tile.Black) {
                        moveValid = false;
                    }
                    break;
                case MoveType.LeftDownJump:
                    Tile halfWayPiece1 = _Board[requestedMove.Piece.X - 1,requestedMove.Piece.Y + 1];
                    if(halfWayPiece1 != checkOne && halfWayPiece1 != checkTwo) {
                        moveValid = false;
                    }
                    break;
                case MoveType.RightDownJump:
                    Tile halfWayPiece2 = _Board[requestedMove.Piece.X + 1,requestedMove.Piece.Y + 1];
                    if(halfWayPiece2 != checkOne && halfWayPiece2 != checkTwo) {
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
            try {
                switch(botNumber) {
                    case 1:
                        switch(_Board[requestedMove.Piece.X,requestedMove.Piece.Y]) {
                            case Tile.WhiteChecker:
                                moveValid = VerifyDown(requestedMove,botNumber);
                                break;
                            case Tile.KingedWhiteChecker:
                                moveValid = VerifyDown(requestedMove,botNumber) && VerifyUp(requestedMove,botNumber);
                                break;
                            default:
                                moveValid = false;
                                break;
                        }
                        break;
                    case 2:
                        switch(_Board[requestedMove.Piece.X,requestedMove.Piece.Y]) {
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
            } catch {
                moveValid = false;
            }
            return moveValid;
        }
        private static bool TileExists(List<Tile> tiles) {
            bool exists = false;
            for(int row = 0;row < BoardSize;row += 1) {
                for(int column = 0;column < BoardSize;column += 1) {
                    if(tiles.Contains(_Board[row,column])) {
                        exists = true;
                        break;
                    }
                }
            }
            return exists;
        }
    }
}