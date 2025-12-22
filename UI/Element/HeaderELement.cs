using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.Interfaces;
using TimeTracker.UI.MenuSystem;

namespace TimeTracker.UI.Element;

/// <summary>
/// A non-selectable menu element that displays a header or title within a menu.
/// </summary>
public class HeaderElement(string text) : IMenuElement
{
    /// <summary>
    /// The text content of the header.
    /// </summary>
    public string Text { get; set; } = text;
    
    /// <inheritdoc/>
    public bool IsEnabled => false;
    
    /// <inheritdoc/>
    public IMenuAction? Action => null;

    /// <inheritdoc/>
    public Style Style { get; set; } = new Style();

    /// <inheritdoc/>
    public override string ToString() => Text;
}
