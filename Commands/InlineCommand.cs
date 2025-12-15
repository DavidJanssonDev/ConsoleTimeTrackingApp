using TimeTracker.Plugins;

namespace TimeTracker.Commands;

internal sealed class InlineCommand : ICommand
{
    private readonly Func<ICommandContext, CommandResult> _execute;

    public InlineCommand(
        string displayName,
        string category,
        bool opensPage,
        Func<ICommandContext, CommandResult> execute)
    {
        DisplayName = displayName;
        Category = category;
        OpensPage = opensPage;
        _execute = execute;
    }

    public string DisplayName { get; }
    public string Category { get; }
    public bool OpensPage { get; }

    public CommandResult Execute(ICommandContext context)
    {
        return _execute(context);
    }
}
