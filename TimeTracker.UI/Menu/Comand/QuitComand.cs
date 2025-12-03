
using Terminal.Gui;

namespace TimeTracker.UI.Menu.Comand;
public class QuitCommand : IMenuAction
{
    public string Title => "< Quit";

    public Task ExecuteAsync(MenuSystem menuSystem)
    {
        Application.RequestStop();
        return Task.CompletedTask;
    }
}