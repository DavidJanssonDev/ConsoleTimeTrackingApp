
using Terminal.Gui;

namespace TimeTracker.UI.MenuSystem.Styles;

/// <summary>
/// Defines style properties (padding and alignment) for menu elements.
/// </summary>
public class MenuItemStyle
{
    /// <summary>
    /// Number of spaces to pad on the left side of the text.
    /// </summary>
    public int PaddingLeft { get; set; } = 0;
    /// <summary>
    /// Number of spaces to pad on the right side of the text.
    /// </summary>
    public int PaddingRight { get; set; } = 0;
    /// <summary>
    /// Horizontal alignment of the text within the padded content area.
    /// </summary>
    public TextAlignment Alignment { get; set; } = TextAlignment.Left;
}
