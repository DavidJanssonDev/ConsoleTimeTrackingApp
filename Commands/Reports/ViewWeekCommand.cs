using System;
using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.Store;

namespace TimeTracker.Commands.Reports;

/// <summary>
/// Shows shifts for current week.
/// </summary>
internal sealed class ViewWeekCommand : ShiftCommandBase
{
    public override string DisplayName => "View Week";
    public override string Category => "Reports";
    public override bool CanHaveSubmenu => false;

    public ViewWeekCommand(IShiftStore store) : base(store) { }

    protected override void ExecuteCore()
    {
        DateTime start = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
        DateTime end = start.AddDays(7);

        List<Domain.Entities.Shift> shifts = ShiftStore.GetShiftsForDateRange(start, end);

        string text = shifts.Count == 0 ? "No shifts this week." : ShiftFormatter.FormatShifts(shifts);
        MessageBox.Query("Week", text, "OK");
    }
}
