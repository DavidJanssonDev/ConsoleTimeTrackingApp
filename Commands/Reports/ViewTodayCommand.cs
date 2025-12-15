using TimeTracker.MenuModel;
using TimeTracker.Plugins;


namespace TimeTracker.Commands.Reports;

/// <summary>
/// Shows shifts for today.
/// </summary>
public sealed class ViewTodayCommand : ICommand
{
    public string DisplayName => "View Today";
    public string Category => "View Projects/Shifts";

    public bool OpensPage => true;

    public CommandResult Execute(ICommandContext context)
    {
        DateTime today = DateTime.Today;

        List<Shift>? shifts = context.ShiftStore.GetShiftsForDate(today);

        MenuNode page = new("Today")
        {
            Footer = "Esc/Backspace to go back"
        };

        if (shifts.Count == 0)
        {
            page.Items.Add(new MenuNode("No shifts today."));
            return new NavigateToResult(page);
        }

        foreach (Shift shfit in shifts)
            page.Items.Add(new MenuNode($"Shift #{shfit.Id}"));

        return new NavigateToResult(page);
    }
}
