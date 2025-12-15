using TimeTracker.MenuModel.Interfaces;
using TimeTracker.Plugins;

namespace TimeTracker.MenuModel;

/// <summary>
/// A leaf node that executes a command when selected.
/// Like a button or link in HTML.
/// </summary>
public sealed class MenuCommand(string title, ICommand command) : IMenuElement
{
    /// <inheritdoc/>
    public string Title { get; } = title;
    public ICommand Command { get; } = command;

}
