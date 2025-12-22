using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.MenuSystem;

namespace TimeTracker.UI.Interfaces;

/// <summary>
/// Common interface for elements that can appear in a menu (options, headers, separators).
/// </summary>
public interface IMenuElement
{
    /// <summary>
    /// Styling information for this element (colors, padding, etc.).
    /// </summary>
    Style Style { get; set; }

}

