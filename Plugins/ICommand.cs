namespace TimeTracker.Plugins;

public interface ICommand
{
    string DisplayName { get; }
    string Category { get; }

    /// <summary>
    /// Optional: purely metadata to hint that UI may show ▶ next to this item.
    /// It does NOT mean the command builds menus.
    /// </summary>
    bool OpensPage { get; }

    /// <summary>
    /// Execute the command. Return a CommandResult describing what should happen in the UI.
    /// </summary>
    CommandResult Execute(ICommandContext context);
}
