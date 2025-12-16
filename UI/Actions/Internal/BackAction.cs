
using TimeTracker.UI.Interfaces;
using TimeTracker.UI.MenuSystem;

namespace TimeTracker.UI.Actions.Internal;

/// <summary>
/// Internal action used to navigate back to the previous menu.
/// This is added automatically as the "Back" option when a menu has <see cref="Menu.HasBackButton"/> enabled.
/// </summary>
internal class BackAction : IMenuAction
{
    /// <summary>
    /// Executes the back action by instructing the engine to navigate to the previous menu.
    /// </summary>
    /// <param name="engine">The menu engine managing the menu navigation.</param>
    public void Execute(MenuEngine engine)
    {
        engine.GoBack();
    }
}
