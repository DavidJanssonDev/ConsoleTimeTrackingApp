using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using TimeTracker.Plugins;
using TimeTracker.Store;

namespace TimeTracker.UI;

public sealed class CommandContext(IShiftStore shiftStore) : ICommandContext
{
    public IShiftStore ShiftStore { get; } = shiftStore;

    public void Toast(string title, string message) => MessageBox.Query(title, message, "OK");

    public bool Confirm(string title, string message) => MessageBox.Query(title, message, "yes", "No") == 0;
}
