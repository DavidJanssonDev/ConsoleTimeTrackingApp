using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using TimeTracker.Plugins;
using TimeTracker.Store;

namespace TimeTracker.UI;

public sealed class CommandContext(IShiftStore shiftStore) : ICommandContext
{
    /// <inheritdoc/>
    public IShiftStore ShiftStore { get; } = shiftStore;

    /// <inheritdoc/>
    public void Toast(string title, string message) => MessageBox.Query(title, message, "OK");

    /// <inheritdoc/>
    public bool Confirm(string title, string message) => MessageBox.Query(title, message, "Yes", "No") == 0;
    

    public void Quit() => Application.RequestStop();
}
