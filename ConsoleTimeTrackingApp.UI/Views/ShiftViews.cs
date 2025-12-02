namespace TimeTracker.UI.Views;

/// <summary>
/// Provides methods for rendering shift and totals tables in the console UI.
/// </summary>
public static class ShiftViews
{
    /// <summary>
    /// Renders a table displaying a list of shifts.
    /// </summary>
    /// <param name="shifts">The collection of shifts to display.</param>
    public static void RenderShiftsTable(IEnumerable<Shift> shifts)
    {
        Table table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("ID")
            .AddColumn("Project")
            .AddColumn("Start")
            .AddColumn("End")
            .AddColumn("Duration")
            .AddColumn("Note");

        foreach (var shift in shifts)
        {
            table.AddRow(
                shift.Id.ToString(),
                shift.Project.Name,
                shift.StartTime.ToString("g"),
                shift.EndTime?.ToString("g") ?? "-",
                shift.Duration.ToString("hh:mm"),
                shift.Note ?? "");
        }

        AnsiConsole.Write(table);
    }
}
