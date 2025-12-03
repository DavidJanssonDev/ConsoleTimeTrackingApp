namespace TimeTracker.UI.Menu.Interfaces;

/// <summary>
/// Represents a menu action with a title and an asynchronous execution method.
/// </summary>
public interface IMenuAction
{
    string Title { get; }
    Task ExecuteAsync(MenuSystem menuSystem);
}
