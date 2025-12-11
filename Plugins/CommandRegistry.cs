using System.Collections.Generic;
using System.Linq;

namespace TimeTracker.Plugins;

/// <summary>
/// Stores all registered commands and groups them by category.
/// </summary>
internal sealed class CommandRegistry
{
    private readonly List<ICommand> _commands;

    public CommandRegistry()
    {
        _commands = new List<ICommand>();
    }

    public void Register(ICommand command)
    {
        _commands.Add(command);
    }

    public Dictionary<string, List<ICommand>> GetCommandsGroupedByCategory()
    {
        IEnumerable<IGrouping<string, ICommand>> groups = _commands.GroupBy(c => c.Category);
        Dictionary<string, List<ICommand>> dict = new Dictionary<string, List<ICommand>>();

        foreach (IGrouping<string, ICommand> group in groups)
        {
            dict[group.Key] = group.OrderBy(c => c.DisplayName).ToList();
        }

        return dict;
    }

    internal IEnumerable<object> GetCategories()
    {
        throw new NotImplementedException();
    }

    internal IEnumerable<ICommand> GetByCategory(string category)
    {
        throw new NotImplementedException();
    }
}
