namespace TradeLensCLI;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello there. Press 't' to start.");
        Account yourAccount = new Account();
        yourAccount.SetAccountType(new string("RothIRA"));
        yourAccount.GetAccountType();
    }
}
