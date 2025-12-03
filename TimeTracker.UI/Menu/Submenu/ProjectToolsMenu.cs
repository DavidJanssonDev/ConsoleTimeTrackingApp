using TimeTracker.UI.Menu.Comand;
using TimeTracker.UI.Menu.Interfaces;

namespace TimeTracker.UI.Menu.Submenu;


public class ProjectToolsMenu : ISubmenuAction
{
    public string Title => "Project Tools";

    public List<IMenuAction> SubActions => new()
    {
        new CreateProjectCommand()
    };

    public Task ExecuteAsync(MenuSystem menuSystem)
    {
        // Should never be executed directly
        return Task.CompletedTask;
    }
}
