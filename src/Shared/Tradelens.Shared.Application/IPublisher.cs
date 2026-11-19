namespace Tradelens.Shared.Application;

/// <summary>
/// Publisher interface responsible for publishing a notification or event for whichever handler(s)
/// may be interested.
/// </summary>
public interface IPublisher
{
    Task Publish();
}