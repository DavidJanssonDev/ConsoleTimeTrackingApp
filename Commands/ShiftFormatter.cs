using System;
using System.Collections.Generic;
using System.Text;

using TimeTracker.Domain.Entities;
namespace TimeTracker.Commands;


/// <summary>
/// Formats shift lists for dialogs.
/// </summary>
internal static class ShiftFormatter
{
    public static string FormatShifts(List<Shift> shifts)
    {
        List<string> lines = new List<string>();
        for (int i = 0; i < shifts.Count; i++)
        {
            lines.Add(FormatSingleShiftLine(shifts[i]));
        }
        return string.Join("\n", lines);
    }

    public static string FormatSingleShiftLine(Shift shift)
    {
        string project = shift.Project.Name;
        string start = shift.StartTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        string end = shift.EndTime?.ToLocalTime().ToString("HH:mm") ?? "(active)";
        double hours = shift.Duration.TotalHours;

        return $"{project,-20} {start} -> {end} | {hours:F2} h";
    }
}
