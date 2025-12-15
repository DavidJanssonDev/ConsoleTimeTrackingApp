namespace TimeTracker.Plugins;

/// <summary>
/// Stores all registered commands and groups them by category.
/// </summary>
internal sealed class CommandRegistry
{
    private readonly List<ICommand> _commands = [];


    public void Register(ICommand command)
    {
        _commands.Add(command);
    }

    public IReadOnlyList<ICommand> GetAll()
    {
        return _commands;
    }

    public Dictionary<string, List<ICommand>> GetCommandsGroupedByCategory()
    {
        return _commands
            .GroupBy(category => category.Category)
            .OrderBy(group => group.Key)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(category => category.DisplayName).ToList()
            );
    }
}
