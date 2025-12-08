using TimeTracker.Commands.Base;
using TimeTracker.Store;

namespace TimeTracker.Commands.System;

/// <summary>
/// Exits the app.
/// </summary>
internal sealed class QuitCommand : ShiftCommandBase
{
    public override string DisplayName => "Quit";
    public override string Category => "System";
    public override bool CanHaveSubmenu => false;

    public QuitCommand(IShiftStore store) : base(store) { }

    protected override void ExecuteCore()
    {
        Terminal.Gui.Application.RequestStop();
    }
}
