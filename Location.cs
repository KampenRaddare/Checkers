namespace Checkers {
    public sealed class Location { //totally not a carbon copy of move.cs
        public Location(int x,int y) {
            X = x;
            Y = y;
        }
        internal int X;
        internal int Y;
    }
}