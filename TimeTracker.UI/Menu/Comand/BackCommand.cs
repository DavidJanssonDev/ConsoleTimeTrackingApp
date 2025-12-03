using TimeTracker.UI.Menu.Interfaces;
namespace TimeTracker.UI.Menu.Comand;

public class BackCommand : IMenuAction
{
    public string Title => "< Back";

    public Task ExecuteAsync(MenuSystem menuSystem)
    {
        menuSystem.Back();
        return Task.CompletedTask;
    }
}