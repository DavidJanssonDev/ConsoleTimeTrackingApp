using TimeTracker.MenuModel;
using TimeTracker.Plugins;


namespace TimeTracker.Commands.Reports;

/// <summary>
/// Shows shifts for current week.
/// </summary>
public sealed class ViewWeekCommand : ICommand
{
    public string DisplayName => "View Week";
    public string Category => "Reports";
    public bool OpensPage => true;

    public CommandResult Execute(ICommandContext context)
    {
        DateTime today = DateTime.Today;

        // Monday-based week (adjust if you prefer Sunday)
        int delta = ((int)today.DayOfWeek + 6) % 7;
        DateTime start = today.AddDays(-delta);
        DateTime end = start.AddDays(7);

        var shifts = context.ShiftStore.GetShiftsForDateRange(start, end); // :contentReference[oaicite:2]{index=2}

        var page = new MenuNode("This Week")
        {
            Footer = $"{start:yyyy-MM-dd} → {end.AddDays(-1):yyyy-MM-dd}"
        };

        if (shifts.Count == 0)
        {
            page.Items.Add(new MenuNode("No shifts this week."));
            return new NavigateToResult(page);
        }

        foreach (var shift in shifts)
            page.Items.Add(new MenuNode($"Shift #{shift.Id}"));
        

        return new NavigateToResult(page);
    }
}