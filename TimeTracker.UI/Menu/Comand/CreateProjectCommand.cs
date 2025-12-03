using Terminal.Gui;
namespace TimeTracker.UI.Menu.Comand;

public class CreateProjectCommand : IMenuAction
{
    public string Title => "Create Project";

    public async Task ExecuteAsync(MenuSystem menuSystem)
    {
        MessageBox.Query("Create Project", "Project Created!", "OK");
        await Task.Delay(500);
        menuSystem.Back();
    }
}