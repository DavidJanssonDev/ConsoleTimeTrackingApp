using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using TimeTracker.UI.MenuSystem.Styles;

namespace TimeTracker.UI.MenuSystem;

/// <summary>
/// Defines visual styling options for menu elements (colors, font style, padding, etc.).
/// </summary>
public class Style
{
    /// <summary>
    /// Foreground text color for the element.
    /// </summary>
    public Color? Color {get; set;}

    ///<summary>
    ///Background color behind the element text.
    ///</summary>
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// Display text in bold if supported (true for bold/high-intensity).
    /// </summary>
    public bool Bold { get; set; } = false;

    /// <summary>
    /// Display text in italic if supported (may be ignored if terminal doesn’t support it).
    /// </summary>
    public bool Italic { get; set; } = false;

    /// <summary>
    /// Display text underlined if supported.
    /// </summary>
    public bool Underline { get; set; } = false;

    /// <summary>
    /// Padding space around the element’s content (Top, Bottom, Left, Right).
    /// </summary>
    public Padding Padding { get; set; } = new Padding();

    /// <summary>
    /// For separator elements, the character used to draw the line.
    /// For other elements, this can be ignored.
    /// </summary>
    public char? Character { get; set; }

    /// <summary>Text alignment within the available width (Left, Center, Right).</summary>
    public TextAlignment Alignment { get; set; } = TextAlignment.Left;
}
