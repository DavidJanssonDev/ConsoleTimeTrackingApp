using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.Interfaces;

namespace TimeTracker.UI.MenuSystem;

/// <summary>
/// Represents an option in a menu, consisting of a label and an action to execute when selected.
/// The action can be a command or a submenu (any object implementing <see cref="IMenuAction"/>).
/// </summary>
public class MenuOption(string label, IMenuAction? action = null) : IMenuElement
{
    /// <summary>
    /// Display text for this menu option.
    /// </summary>
    public string Label { get; set; } = label;

    /// <summary>
    /// The action to perform when this option is selected (can be a submenu or command).
    /// </summary>
    public IMenuAction? Action { get; set; } = action;

    /// <summary>
    /// Whether this option is currently enabled (selectable) or not.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <inheritdoc/>
    public Style Style { get; set; } = new Style();

    /// <summary>
    /// Return label (and mark as disabled if not enabled, for debugging/optional display)
    /// </summary>
    public override string ToString()
    {
        return IsEnabled ? Label : $"{Label} (disabled)";
    }
}
