namespace Checkers {
    public sealed class Move { //totally not a carbon copy of location.cs
        public Move(Location piece, MoveType moveType) {
            Piece = piece;
            MoveType = moveType;
        }
        internal Location Piece;
        internal MoveType MoveType;
    }
}