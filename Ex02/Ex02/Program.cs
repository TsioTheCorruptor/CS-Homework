using Logic;
using UeserInterface;


internal class Program
{
    static void Main()
    {
        char[] lettersToGuess = {'A', 'B', 'C', 'D', 'E', 'F' };
        int arrayLength = lettersToGuess.Length;
        int guessLength = 4;
        int minGuessNumber = 4;
        int maxGuessNumber = 10;
        Engine eng = new Engine(guessLength, maxGuessNumber, minGuessNumber, arrayLength , lettersToGuess);
        Ui ui = new Ui(eng, guessLength, minGuessNumber, maxGuessNumber, lettersToGuess);
        ui.Run();
    }
}

