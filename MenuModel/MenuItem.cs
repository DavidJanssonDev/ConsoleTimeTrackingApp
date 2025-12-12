using TimeTracker.MenuModel.Interfaces;
using TimeTracker.Plugins;

namespace TimeTracker.MenuModel;

/// <summary>
/// Temporary adapter for old code/plugins.
/// Prefer MenuCommand and MenuNode directly.
/// </summary>
public sealed class MenuItem : IMenuElement
{
    public string Title { get; }
    public ICommand? Command { get; }
    public MenuNode? Submenu { get; }

    public MenuItem(string title, ICommand command)
    {
        Title = title;
        Command = command;
        Submenu = null;
    }

    public MenuItem(string title, MenuNode submenu)
    {
        Title = title;
        Submenu = submenu;
        Command = null;
    }
}
