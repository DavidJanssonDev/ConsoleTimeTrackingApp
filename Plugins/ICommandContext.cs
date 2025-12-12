using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Store;

namespace TimeTracker.Plugins;

public interface ICommandContext
{
    IShiftStore ShiftStore { get; }


    void Toast(string title, string message);

    bool Confirm(string title, string message);

}
