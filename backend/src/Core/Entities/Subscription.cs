namespace Core.Entities;

public class Subscription
{
    public required SubscriptionTiers SubscriptionTier { get; set; }
}

public enum SubscriptionTiers
{
    Starter,
    Pro,
    Enterprise
}