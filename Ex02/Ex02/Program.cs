using Logic;


internal class Program
{
    static void Main()
    {
        char[] hhh = { 'b', 'b' };
        Engine eng = new Engine(4, 4, 4, 8,hhh);
        eng.ResetGame(4);
        eng.GetRandomObjectIndexes();
        int[] bro = eng.GetGuessInfoAndUpdate("kucky");
        foreach (int i in bro) { Console.WriteLine(i); }
    }
}

