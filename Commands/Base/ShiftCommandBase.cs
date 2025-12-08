using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using TimeTracker.MenuModel;
using TimeTracker.Plugins;
using TimeTracker.Store;

namespace TimeTracker.Commands.Base;

internal abstract class ShiftCommandBase(IShiftStore store) : ICommand
{
    public abstract string DisplayName { get; }
    public abstract string Category { get; }
    public abstract bool CanHaveSubmenu { get; }

    public IShiftStore ShiftStore { get; set; } = store;

    public void Execute()
    {
        try
        {
            ExecuteCore();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    }

    public virtual MenuNode? BuildSubmenu()
    {
        return null;
    }

    protected abstract void ExecuteCore();
}
