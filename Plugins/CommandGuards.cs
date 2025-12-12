using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Plugins;

public static class CommandGuards
{
    public static CommandResult RequireNoActiveSift(ICommandContext context, string actionName)
    {
        Shift? active = context.ShiftStore.GetActiveShift();
        
        if (active is null)
            return new StayResult();

        return new ShowMessageResult(
            "Active Shift",
            $"You already have an active shift. Please clokc out before '{actionName}"
        );
    }

    public static (bool ok, CommandResult result) TryGetActiveShiftId(ICommandContext context)
    {
        Shift? active = context.ShiftStore.GetActiveShift();
        
        if (active is null)
            return (false, new ShowMessageResult("No Active Shift", "There is no active shift."));
        

        return (true, new StayResult());
    }

}
