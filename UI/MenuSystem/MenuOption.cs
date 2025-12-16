using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.Interfaces;

namespace TimeTracker.UI.MenuSystem;

/// <summary>
/// Represents an option in a menu, consisting of a label and an action to execute when selected.
/// The action can be a command or a submenu (any object implementing <see cref="IMenuAction"/>).
/// </summary>
public class MenuOption
{
    /// <summary>
    /// Gets or sets the display text for this menu option.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets the action to perform when this option is selected.
    /// The action can be a navigation to a submenu or a command (such as quit or custom action).
    /// </summary>
    public IMenuAction Action { get; set; }


    /// <summary>Whether this option is currently enabled and can be executed.</summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Initializes a new menu option with the specified label and action.
    /// </summary>
    /// <param name="label">The text to display for this option.</param>
    /// <param name="action">The action to execute when this option is selected (e.g., a submenu or a command).</param>
    public MenuOption(string label, IMenuAction action)
    {
        Label = label;
        Action = action;
    }

    /// <summary>
    /// Returns the label of this menu option.
    /// </summary>
    public override string ToString()
    {
        return IsEnabled ? Label : $"{Label} (disabled)";
    }
}
