using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.MenuModel;
using TimeTracker.Plugins;
using TimeTracker.Store;
namespace TimeTracker.Commands;

internal sealed class InlineCommand(string displayName, string category, IShiftStore store, Action execute) : ICommand
{
    private readonly Action _execute = execute;

    public string DisplayName { get; } = displayName;
    public string Category { get; } = category;
    public bool CanHaveSubmenu => false; 
    public IShiftStore ShiftStore { get; set; } = store;

    public void Execute()
    {
        _execute.Invoke();
    }

    public MenuNode? BuildSubmenu()
    {
        return null; 
    }
}
