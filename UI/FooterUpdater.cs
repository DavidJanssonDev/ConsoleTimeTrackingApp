using Terminal.Gui;
using TimeTracker.Domain.Entities;
using TimeTracker.Store;

namespace TimeTracker.UI;

/// <summary>
/// Updates status label based on current shift state.
/// </summary>
internal static class FooterUpdater
{
    public static void Update(Label statusLabel, IShiftStore store)
    {
        Shift? active = store.GetActiveShift();
        if (active == null)
        {
            statusLabel.Text = "Status: Not clocked in";
            return;
        }

        string project = active.Project.Name;
        string start = active.StartTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        statusLabel.Text = $"Status: Working on {project} (since {start})";
    }
}
