using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.MenuSystem.Styles;

namespace TimeTracker.UI.MenuSystem.Interface;

/// <summary>
/// Interface for menu elements that have an associated style.
/// </summary>
internal interface IStylable
{
    /// <summary>
    /// Gets the style settings (padding, alignment, etc.) for this menu element.
    /// </summary>
    MenuItemStyle Style { get; }
}

