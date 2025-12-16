using Terminal.Gui;
using TimeTracker.ApplicationCode.Services;
using TimeTracker.UI;
using TimeTracker.UI.Actions.Public;
using TimeTracker.UI.MenuSystem;

namespace TimeTracker;

internal static class Program
{
    static void Main()
    {
        // Define a submenu with a title, and allow a back button for navigation
        var submenu = new Menu("Submenu")
        {
            HasBackButton = true,
            Options =
            {
                // This submenu has its own option demonstrating a command action
                new MenuOption("Submenu Say Hello", new SayHelloAction())
            }
        };

        // Define the root menu with a title and options
        var rootMenu = new Menu("Main Menu")
        {
            Options =
            {
                // Option to navigate into the submenu
                new MenuOption("Go to Submenu", submenu),
                // Option to execute a simple command (say hello)
                new MenuOption("Say Hello", new SayHelloAction()),
                // Option to quit the application
                new MenuOption("Quit", new QuitAction())
            }
        };

        // Create the menu engine with the root menu and run the menu system
        var menuEngine = new MenuEngine(rootMenu);
        menuEngine.Run();
    }

}