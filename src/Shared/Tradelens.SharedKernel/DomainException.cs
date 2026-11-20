namespace Tradelens.SharedKernel;

/// <summary>
/// Domain exceptions are thrown for any violations of invariants or defined rules.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}