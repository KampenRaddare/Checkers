/*
I messed around with having a field for having the bot name but I figured I can just use the class name using GetType().ToString()
And since interfaces don't have fields, I think you can figure out how winmessage and losemessage work, just look at Bots.Dave.cs
Oh and yeah again with the inconsistent and painful to read camel casing.
*/
namespace Checkers {
    internal interface Bot {
        void Setup(int botNumber); //This is really important if you wanna make your bot functional without trial and error.
        Move Turn();
        string WinMessage();
        string LoseMessage();
    }
}