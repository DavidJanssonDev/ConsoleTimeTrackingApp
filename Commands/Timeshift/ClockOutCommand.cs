using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.Store;

namespace TimeTracker.Commands.Timeshift;

/// <summary>
/// Ends the currently active shift.
/// </summary>
internal sealed class ClockOutCommand : ShiftCommandBase
{
    public override string DisplayName => "Clock Out";
    public override string Category => "Shift";
    public override bool CanHaveSubmenu => false;

    public ClockOutCommand(IShiftStore store) : base(store) { }

    protected override void ExecuteCore()
    {
        Domain.Entities.Shift? active = ShiftStore.GetActiveShift();
        if (active == null)
        {
            throw new InvalidOperationException("Not clocked in.");
        }

        ShiftStore.EndShift(active.Id);

        MessageBox.Query("Clock Out", $"Ended {active.Project.Name}.", "OK");
    }
}
