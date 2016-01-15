//This is Dave... Dave isn't very smart. He doesn't know how to play checkers so when it's his turn he does absolutely nothing.
namespace Checkers.Bots {
    internal sealed class Dave : Bot {
        public void Setup(int botNumber,int boardNumber) {
        }
        public Move Turn() {
            return null;
        }
        public string WinMessage() {
            return "Good game. I didn't even know to play, but I think I'm starting to learn!"; //I hate Dave and I created him
        }
        public string LoseMessage() {
            return "Wow! You're really good at this! Maybe you can teach me?"; //He's a good sport, but I hate him.
        }
    }
}