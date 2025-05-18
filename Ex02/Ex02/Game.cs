using Logic;
using UeserInterface;

namespace Game
{
    internal class Game
    {
        public static void run()
        {
            char[] lettersToGuess = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            int arrayLength = lettersToGuess.Length;
            int guessLength = 4;
            int minGuessNumber = 4;
            int maxGuessNumber = 10;
            Engine eng = new Engine(guessLength, maxGuessNumber, minGuessNumber, arrayLength, lettersToGuess);
            Ui ui = new Ui(eng, guessLength, minGuessNumber, maxGuessNumber, lettersToGuess);
            ui.Run();
        }
    }
}

