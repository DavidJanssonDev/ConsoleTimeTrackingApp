using TimeTracker.UI.MenuSystem;
using TimeTracker.UI.MenuSystem.Actions;
using TimeTracker.UI.MenuSystem.Interface;

namespace TimeTracker;

internal static class Program
{
    static void Main()
    {
        MenuPage submenu = new()
        {
            Title = "Submenu",
            Items =
            [
                new MenuItem {Label = "Submenu Say Hello", Action = new MessageAction("Greetings", "Hello from the new menu engine!")}
            ]
        };

        MenuPage root = new()
        {
            Title = "Main Menu",
            Items =
            [
                new MenuItem {Label = "Go to Submenu", Action = new NavigateAction(submenu)},
                new MenuItem {Label = "Say Hello", Action = new MessageAction("Hi", "Hello")}
            ]
        };
        new MenuApp().Run(root);
    }

}

public sealed class MessageAction(string title, string message) : IMenuAction
{
    public void Execute(NavigationService nav)
    {
        Terminal.Gui.MessageBox.Query(title, message, "OK");
    }
}