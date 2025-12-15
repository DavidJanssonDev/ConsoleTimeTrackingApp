using TimeTracker.MenuModel;
using TimeTracker.Plugins;

public sealed class TotalsByProjectCommand : ICommand
{
    public string DisplayName => "Totals By Project";
    public string Category => "View Projects/Shifts";
    public bool OpensPage => true;

    public CommandResult Execute(ICommandContext context)
    {
        Dictionary<string, TimeSpan> totals = context.ShiftStore.TotalsByProject();

        MenuNode page = new("Totals By Project")
        {
            Footer = "Esc/Backspace to go back."
        };

        if (totals.Count == 0)
        {
            page.Items.Add(new MenuNode("No data."));
            return new NavigateToResult(page);
        }

        foreach (KeyValuePair<string, TimeSpan> kv in totals)
        {
            page.Items.Add(new MenuNode($"{kv.Key}: {kv.Value}"));
        }

        return new NavigateToResult(page);
    }
}
