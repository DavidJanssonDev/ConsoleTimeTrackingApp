using System;
using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.Store;

namespace TimeTracker.Commands.Reports;

/// <summary>
/// Shows total time per project.
/// </summary>
internal sealed class TotalsByProjectCommand : ShiftCommandBase
{
    public override string DisplayName => "Totals by Project";
    public override string Category => "Reports";
    public override bool CanHaveSubmenu => false;

    public TotalsByProjectCommand(IShiftStore store) : base(store) { }

    protected override void ExecuteCore()
    {
        Dictionary<string, TimeSpan> totals = ShiftStore.TotalsByProject();
        if (totals.Count == 0)
        {
            MessageBox.Query("Totals", "No completed shifts yet.", "OK");
            return;
        }

        List<string> lines = new List<string>();
        foreach (KeyValuePair<string, TimeSpan> kv in totals)
        {
            lines.Add($"{kv.Key}: {kv.Value.TotalHours:F2} h");
        }

        MessageBox.Query("Totals", string.Join("\n", lines), "OK");
    }
}
