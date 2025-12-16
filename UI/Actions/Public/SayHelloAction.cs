using Terminal.Gui;
using TimeTracker.UI.Interfaces;
using TimeTracker.UI.MenuSystem;

namespace TimeTracker.UI.Actions.Public;

/// <summary>
/// A sample action that displays a greeting message.
/// This demonstrates how to create a custom menu action.
/// </summary>
public class SayHelloAction : IMenuAction
{
    /// <summary>
    /// Executes the action by displaying a "Hello" message box to the user.
    /// </summary>
    /// <param name="engine">The menu engine managing the application (unused in this action except for context).</param>
    public void Execute(MenuEngine engine)
    {
        // Display a simple message box greeting the user.
        MessageBox.Query("Greetings", "Hello from the menu system!", "OK");
    }
}
