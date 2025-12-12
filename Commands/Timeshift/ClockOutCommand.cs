using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.Plugins;
using TimeTracker.Store;

namespace TimeTracker.Commands.Timeshift;

/// <summary>
/// Ends the currently active shift.
/// </summary>
public sealed class ClockOutCommand : ICommand
{
    public string DisplayName => "Clock Out";
    public string Category => "Time";
    public bool OpensPage => false;

    public CommandResult Execute(ICommandContext context)
    {
        var activeShift = context.ShiftStore.GetActiveShift();

        if (activeShift is null) 
            return new ShowMessageResult("No Active Shift", "There is no active shift to clock out.");
        

        bool confirm = context.Confirm("Clock Out", "Clock out the active shift?");

        if (!confirm)
            return new StayResult();
        
        context.ShiftStore.ClockOut(activeShift.Id);

        return new ShowMessageResult("Clocked Out", "Shift ended.");
    }
}
