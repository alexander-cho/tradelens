namespace Tradelens.Core.Models;

public class ExpirationsModel
{
    public required FullExpiryList Expirations { get; set; }
}

public class FullExpiryList
{
    public required List<string> Date { get; set; }
}