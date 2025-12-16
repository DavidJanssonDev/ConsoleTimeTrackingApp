using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.MenuSystem;

namespace TimeTracker.UI.Interfaces;
public interface IMenuAction
{
    /// <summary>
    /// Executes the action using the given menu engine context.
    /// </summary>
    /// <param name="engine">The menu engine or controller that is managing the menu navigation and UI.</param>
    void Execute(MenuEngine engine);
}
