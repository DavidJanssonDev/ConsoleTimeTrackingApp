using ConsoleTimeTrackingApp.Models;
using Spectre.Console;

namespace ConsoleTimeTrackingApp.UI;

internal class ShiftViews
{
    public static void RenderShiftsTable(IEnumerable<Shift> shifts)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Shifts[/]")
            .AddColumn("Project")
            .AddColumn("Start")
            .AddColumn("End")
            .AddColumn("Duration")
            .AddColumn("Note");

        foreach (var s in shifts)
        {
            var duration = s.EndTime is null
                ? "[grey]—[/]"
                : s.Duration.ToString(@"hh\:mm"); // uses computed Duration :contentReference[oaicite:12]{index=12}

            var projectName = s.EndTime is null
                ? $"[yellow]{s.Project.Name}[/]"
                : s.Project.Name;

            table.AddRow(
                projectName,
                s.StartTime.ToString("yyyy-MM-dd HH:mm"),
                s.EndTime?.ToString("yyyy-MM-dd HH:mm") ?? "[yellow]Active[/]",
                duration,
                s.Note ?? string.Empty
            );
        }

        AnsiConsole.Write(table);
    }

    public static void RenderTotalsTable(IReadOnlyDictionary<string, TimeSpan> totals)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Totals by Project[/]")
            .AddColumn("Project")
            .AddColumn("Total");

        foreach (var kvp in totals.OrderBy(k => k.Key))
        {
            table.AddRow(kvp.Key, kvp.Value.ToString(@"hh\:mm"));
        }

        AnsiConsole.Write(table);
    }
}
