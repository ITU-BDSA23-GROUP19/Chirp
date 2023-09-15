public static class Userinterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (Cheep cheep in cheeps)
        {
            Console.WriteLine(cheep.Display());
        }
    }
}