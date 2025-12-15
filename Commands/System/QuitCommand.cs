using Terminal.Gui;
using TimeTracker.Plugins;

namespace TimeTracker.Commands.System;

/// <summary>
/// Exits the app.
/// </summary>
public sealed class QuitCommand : ICommand
{
    public string DisplayName => "Quit";
    public string Category => "System";
    public bool OpensPage => false;

    public CommandResult Execute(ICommandContext context)
    {
        Application.RequestStop();
        return new StayResult();
    }
}