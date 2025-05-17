using Logic;


internal class Program
{
    static void Main()
    {
        char[] hhh = { 'A', 'B' , 'C', 'D', 'E', 'F'};
        Engine eng = new Engine(4, 4, 4, 8,hhh);
        eng.ResetGame(4);
        eng.GetRandomObjectIndexes();
        int[] bro = eng.GetGuessMatchingInfo("kucky", "cucky");
        foreach (int i in bro) { Console.WriteLine(i); }
    }
}

