using System.CommandLine;
using System.CommandLine.Parsing;

namespace Tradelens.Cli;

class Program
{
    static int Main(string[] args)
    {
        Option<int> num1 = new("--num1")
        {
            Description = "Number to read and display on the console."
        };

        RootCommand rootCommand = new("Tradelens.Cli");
        rootCommand.Options.Add(num1);

        ParseResult parseResult = rootCommand.Parse(args);
        if (parseResult.Errors.Count == 0 && parseResult.GetValue(num1) is int)
        {
            Console.WriteLine(parseResult.GetValue(num1));
            return 0;
        }
        foreach (ParseError parseError in parseResult.Errors)
        {
            Console.Error.WriteLine(parseError.Message);
        }
        return 1;
    }
}