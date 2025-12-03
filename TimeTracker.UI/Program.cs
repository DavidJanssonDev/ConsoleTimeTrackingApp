using Terminal.Gui;
using TimeTracker.UI.Menu;
using TimeTracker.UI.Menu.Comand;
using TimeTracker.UI.Menu.Submenu;

namespace TimeTracker.UI;

public static class Program
{
    public static void Main(string[] args)
    {
        Application.Init();

        var menu = new MenuSystem();

        menu.ShowMenu(new List<IMenuAction>
        {
            new StartShiftCommand(),
            new ProjectToolsMenu()
        });

        Application.Run();
    }
}