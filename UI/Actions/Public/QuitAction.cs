using TimeTracker.UI.Interfaces;
using TimeTracker.UI.MenuSystem;

namespace TimeTracker.UI.Actions.Public;

/// <summary>
/// An action that causes the application to exit when executed.
/// Use this in a <see cref="MenuOption"/> to provide a "Quit" option.
/// </summary>
public class QuitAction : IMenuAction
{
    /// <summary>
    /// Executes the quit action, signaling the menu system to terminate the application.
    /// </summary>
    /// <param name="engine">The menu engine managing the application.</param>
    public void Execute(MenuEngine engine)
    {
        engine.Quit();
    }
}
