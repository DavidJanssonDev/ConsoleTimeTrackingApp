using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.MenuModel.Interfaces;
using TimeTracker.Plugins;

namespace TimeTracker.MenuModel;

/// <summary>A leaf node that executes a command when selected. Like a <button></button> or <a></a> in HTML. </
public sealed class MenuCommand : IMenuElement
{
    public string Title { get; }
    public ICommand Command { get; }

    public MenuCommand(string title, ICommand command)
    {
        Title = title;
        Command = command;
    }
}
