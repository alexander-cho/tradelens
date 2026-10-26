using Tradelens.SharedKernel;

namespace Tradelens.Social.Domain.Posts;

public sealed record Cashtag : ValueObject
{
    public string Value { get; init; }

    private Cashtag(string value)
    {
        Value = value;
    }

    public static Cashtag Create(string value)
    {
        return new Cashtag(value);
    }
}