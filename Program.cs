using TimeTracker.UI.MenuSystem;
using TimeTracker.UI.MenuSystem.Actions;

namespace TimeTracker;

internal static class Program
{
    private static void Main()
    {
        var aboutPage = new MenuPage
        {
            Title = "About",
            Items =
            [
                new MenuItem
                {
                    Label = "Version",
                    Action = new MessageAction("ConsoleTimeTrackingApp", "Version 0.1")
                }
            ]
        };

        var root = new MenuPage
        {
            Title = "Main Menu",
            Items =
            [
                new MenuItem { Label = "About", Action = new NavigateAction(aboutPage) }
            ]
        };

        new MenuApp().Run(root);
    }
}
