using Terminal.Gui;
using TimeTracker.UI.MenuSystem.Interface;

namespace TimeTracker.UI.MenuSystem.Actions;

public sealed class MessageAction(string title, string message) : IMenuAction
{
    public void Execute(NavigationService nav)
    {
        MessageBox.Query(title, message, "OK");
    }
}
