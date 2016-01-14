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
            while(index != indexEnd) {
                System.Console.ReadKey(true);
                Location tempPiece = SelectPiece(index,indexEnd);
                if(tempPiece == null) {
                    System.Console.WriteLine("tempPieceNulled");
                    return null;
                } else {
                    MoveType? moveType = SelectMove(tempPiece);
                    index = tempPiece;
                    if(moveType != null) {
                        System.Console.WriteLine("Move not null.");
                        return new Move(tempPiece,(MoveType)moveType);
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
            System.Console.WriteLine("RETURNED DEFAULT");
            return null;
        }
        private Location SelectPiece(Location startAt,Location endAt) {
            for(int y = startAt.Y;y < endAt.Y;y += 1) {
                for(int x = startAt.X;x < endAt.X;x += 1) {
                    Tile selectedTile = Program.Board[x,y];
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
            switch(Program.Board[piece.X,piece.Y]) {
                case Tile.WhiteChecker:
                    //setup movement selection
                    break;
                case Tile.KingedWhiteChecker:
                    //setup movement selection
                    break;
                case Tile.RedChecker:
                    //setup movement selection
                    break;
                case Tile.KingedRedChecker:
                    //setup movement selection
                    break;
            } //If I tried to do that right now it would be about as good as throwing a cat at your checkers board
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