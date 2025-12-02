namespace TimeTracker.UI.Menu;

/// <summary>
/// Represents a menu action that quits the application.
/// </summary>
public class QuitCommand : IMenuAction
{
    /// <summary>
    /// Gets the title of the quit command.
    /// </summary>
    public string Title => "Exit";

    /// <summary>
    /// Executes the quit command asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ExecuteAsync()
    {
        AnsiConsole.MarkupLine("[yellow]Exiting Shift Tracker...[/]");
        return Task.CompletedTask;
    }
}