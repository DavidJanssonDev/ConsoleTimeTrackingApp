using System;
using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.Store;

namespace TimeTracker.Commands.Reports;

/// <summary>
/// Shows shifts for today.
/// </summary>
internal sealed class ViewTodayCommand : ShiftCommandBase
{
    public override string DisplayName => "View Today";
    public override string Category => "Reports";
    public override bool CanHaveSubmenu => false;

    public ViewTodayCommand(IShiftStore store) : base(store) { }

    protected override void ExecuteCore()
    {
        List<Shift> shifts = ShiftStore.GetShiftsForDate(DateTime.Now);
        string text = shifts.Count == 0 ? "No shifts today." : ShiftFormatter.FormatShifts(shifts);
        MessageBox.Query("Today", text, "OK");
    }
}
