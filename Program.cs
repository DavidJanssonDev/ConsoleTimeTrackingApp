using TimeTracker.UI.MenuSystem;
using TimeTracker.UI.MenuSystem.Actions;
using TimeTracker.UI.MenuSystem.Interface;

namespace TimeTracker;

using TimeTracker.UI.MenuSystem;

internal static class Program
{
    private static void Main()
    {
        var root = new MenuPage
        {
            Title = "Main Menu",
            Items =
            [
                new MenuItem { Label = "Quit", Action = new QuitAction() }
            ]
        };

        new MenuApp().Run(root);
    }
}
