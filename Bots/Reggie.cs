/*Reggie uses no uh, intelligence per say,
he takes an extremely literal approach to checkers
No estimating a best move
Just the first possible move
Because you see
That's how Reggie rolls
Understand?
REGGIE WAS A TEST
DON'T HATE ME PLZ
i need sleep
*/
namespace Checkers.Bots {
    using System.Collections.Generic;
    internal sealed class Reggie:Bot {
        private bool IsBotOne;
        private int TurnNumber = 0;
        public void Setup(int botNumber) {
            switch(botNumber) {
                case 1:
                    IsBotOne = true;
                    break;
                case 2:
                    IsBotOne = false;
                    break;
            }
        }
        public Move Turn() {
            TurnNumber += 1;
            Location index = new Location(0,0);
            Location indexEnd = new Location(Program.BoardSize-1,Program.BoardSize-1);
            List<MoveType> possibleMovesType = new List<MoveType>();
            List<Location> possibleMovesLocation = new List<Location>();
            while(index != indexEnd) {
                Location tempPiece = SelectPiece(index,indexEnd);
                if(tempPiece == null) {
                    return null;
                } else {
                    MoveType? moveType = SelectMove(tempPiece);
                    //index = tempPiece;
                    if(moveType != null) {
                        possibleMovesType.Add((MoveType)moveType);
                        possibleMovesLocation.Add(tempPiece);
                    } else {
                        if(index.X < indexEnd.X) {
                            index.X += 1;
                        } else if(index.Y < indexEnd.Y){
                            index.X = 0;
                            index.Y += 1;
                        }
                    }
                }
            }
            if(possibleMovesType.Count > 0) {
                Move bestMove = null;
                int indexOf = -1;
                if(possibleMovesType.Contains(MoveType.LeftDownJump)) {
                    indexOf = possibleMovesType.IndexOf(MoveType.LeftDownJump);
                } else if(possibleMovesType.Contains(MoveType.RightDownJump)) {
                    indexOf = possibleMovesType.IndexOf(MoveType.RightDownJump);
                } else if(possibleMovesType.Contains(MoveType.LeftUpJump)) {
                    indexOf = possibleMovesType.IndexOf(MoveType.LeftUpJump);
                } else if(possibleMovesType.Contains(MoveType.RightUpJump)) {
                    indexOf = possibleMovesType.IndexOf(MoveType.RightUpJump);
                } else if(possibleMovesType.Contains(MoveType.RightDown)) {
                    indexOf = possibleMovesType.IndexOf(MoveType.RightDown);
                } else if(possibleMovesType.Contains(MoveType.LeftDown)) {
                    indexOf = possibleMovesType.IndexOf(MoveType.LeftDown);
                } else if(possibleMovesType.Contains(MoveType.LeftUp)) {
                    indexOf = possibleMovesType.IndexOf(MoveType.LeftUp);
                } else if(possibleMovesType.Contains(MoveType.RightUp)) {
                    indexOf = possibleMovesType.IndexOf(MoveType.RightUp);
                }
                bestMove = new Move(possibleMovesLocation.ToArray()[indexOf],possibleMovesType.ToArray()[indexOf]);
                return bestMove;
            }
            return null;
        }
        private Location SelectPiece(Location startAt,Location endAt) {
            for(int y = startAt.Y;y < endAt.Y;y += 1) {
                for(int x = startAt.X;x < endAt.X;x += 1) {
                    Tile selectedTile = Program.Board(x,y);
                    if(IsBotOne) {
                        switch(selectedTile) {
                            case Tile.RedChecker:
                            case Tile.KingedRedChecker:
                                return new Location(x,y);
                                break;
                        }
                    } else {
                        switch(selectedTile) {
                            case Tile.WhiteChecker:
                            case Tile.KingedWhiteChecker:
                                return new Location(x,y);
                                break;
                        }
                    }
                }
            }
            return null;
        }
        private MoveType? SelectMove(Location piece) {
            MoveType? selectedMove = null;
            switch(Program.Board(piece.X,piece.Y)) {
                case Tile.WhiteChecker:
                    if((Program.Board(piece.X + 1,piece.Y + 1) == Tile.RedChecker || Program.Board(piece.X + 1,piece.Y + 1) == Tile.KingedRedChecker) && Program.Board(piece.X + 2,piece.Y + 2) == Tile.Black) {
                        selectedMove = MoveType.RightUpJump;
                    } else if((Program.Board(piece.X - 1,piece.Y + 1) == Tile.RedChecker || Program.Board(piece.X - 1,piece.Y + 1) == Tile.KingedRedChecker) && Program.Board(piece.X - 2,piece.Y + 2) == Tile.Black) {
                        selectedMove = MoveType.LeftUpJump;
                    } else if(Program.Board(piece.X - 1,piece.Y + 1) == Tile.Black) {
                        selectedMove = MoveType.LeftUp;
                    } else if(Program.Board(piece.X + 1,piece.Y + 1) == Tile.Black) {
                        selectedMove = MoveType.RightUp;
                    }
                    break;
                case Tile.KingedWhiteChecker:
                    if((Program.Board(piece.X + 1,piece.Y + 1) == Tile.RedChecker || Program.Board(piece.X + 1,piece.Y + 1) == Tile.KingedRedChecker) && Program.Board(piece.X + 2,piece.Y + 2) == Tile.Black) {
                        selectedMove = MoveType.RightUpJump;
                    } else if((Program.Board(piece.X + 1,piece.Y - 1) == Tile.RedChecker || Program.Board(piece.X + 1,piece.Y - 1) == Tile.KingedRedChecker) && Program.Board(piece.X + 2,piece.Y - 2) == Tile.Black) {
                        selectedMove = MoveType.RightDownJump;
                    } else if((Program.Board(piece.X - 1,piece.Y + 1) == Tile.RedChecker || Program.Board(piece.X - 1,piece.Y + 1) == Tile.KingedRedChecker) && Program.Board(piece.X - 2,piece.Y + 2) == Tile.Black) {
                        selectedMove = MoveType.LeftUpJump;
                    } else if((Program.Board(piece.X - 1,piece.Y - 1) == Tile.RedChecker || Program.Board(piece.X - 1,piece.Y - 1) == Tile.KingedRedChecker) && Program.Board(piece.X - 2,piece.Y - 2) == Tile.Black) {
                        selectedMove = MoveType.LeftDownJump;
                    } else if(Program.Board(piece.X - 1,piece.Y + 1) == Tile.Black) {
                        selectedMove = MoveType.LeftUp;
                    } else if(Program.Board(piece.X + 1,piece.Y + 1) == Tile.Black) {
                        selectedMove = MoveType.RightUp;
                    }
                    break;
                case Tile.RedChecker:
                    if((Program.Board(piece.X + 1,piece.Y - 1) == Tile.WhiteChecker || Program.Board(piece.X + 1,piece.Y - 1) == Tile.KingedWhiteChecker) && Program.Board(piece.X + 2,piece.Y - 2) == Tile.Black) {
                        selectedMove = MoveType.RightDownJump;
                    } else if((Program.Board(piece.X - 1,piece.Y - 1) == Tile.WhiteChecker || Program.Board(piece.X - 1,piece.Y + 1) == Tile.KingedWhiteChecker) && Program.Board(piece.X - 2,piece.Y - 2) == Tile.Black) {
                        selectedMove = MoveType.LeftDownJump;
                    } else if(Program.Board(piece.X - 1,piece.Y - 1) == Tile.Black) {
                        selectedMove = MoveType.LeftDown;
                    } else if(Program.Board(piece.X + 1,piece.Y - 1) == Tile.Black) {
                        selectedMove = MoveType.RightDown;
                    }
                    break;
                case Tile.KingedRedChecker:
                    if((Program.Board(piece.X + 1,piece.Y + 1) == Tile.WhiteChecker || Program.Board(piece.X + 1,piece.Y + 1) == Tile.KingedWhiteChecker) && Program.Board(piece.X + 2,piece.Y + 2) == Tile.Black) {
                        selectedMove = MoveType.RightUpJump;
                    } else if((Program.Board(piece.X + 1,piece.Y - 1) == Tile.WhiteChecker || Program.Board(piece.X + 1,piece.Y - 1) == Tile.KingedWhiteChecker) && Program.Board(piece.X + 2,piece.Y - 2) == Tile.Black) {
                        selectedMove = MoveType.RightDownJump;
                    } else if((Program.Board(piece.X - 1,piece.Y + 1) == Tile.WhiteChecker || Program.Board(piece.X - 1,piece.Y + 1) == Tile.KingedWhiteChecker) && Program.Board(piece.X - 2,piece.Y + 2) == Tile.Black) {
                        selectedMove = MoveType.LeftUpJump;
                    } else if((Program.Board(piece.X - 1,piece.Y - 1) == Tile.WhiteChecker || Program.Board(piece.X - 1,piece.Y - 1) == Tile.KingedWhiteChecker) && Program.Board(piece.X - 2,piece.Y - 2) == Tile.Black) {
                        selectedMove = MoveType.LeftDownJump;
                    } else if(Program.Board(piece.X - 1,piece.Y + 1) == Tile.Black) {
                        selectedMove = MoveType.LeftUp;
                    } else if(Program.Board(piece.X + 1,piece.Y + 1) == Tile.Black) {
                        selectedMove = MoveType.RightUp;
                    }
                    break;
            }
            if(selectedMove != null) {
                if(Program.VerifyMove(new Move(piece,(MoveType)selectedMove),(IsBotOne ? 1 : 2))) {
                    return selectedMove;
                } else {
                    return null;
                }
            } else {
                return null;
            }
            return selectedMove;
        }
        public string WinMessage() {
            return "Mmmmmmmmmmmmmmmmmmmmmmm.";
        }
        public string LoseMessage() {
            return $"I'm coming for you bot {(IsBotOne?'1':'2')}..";
        }
    }
}