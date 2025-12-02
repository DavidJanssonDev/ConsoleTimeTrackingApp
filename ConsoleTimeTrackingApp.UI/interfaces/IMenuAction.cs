namespace TimeTracker.UI.Interfaces;

/// <summary>
/// Represents a menu action with a title and an asynchronous execution method.
/// </summary>
public interface IMenuAction
{
    /// <summary>
    /// Gets the title of the menu action.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Executes the menu action asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync();
}
