
using TimeTracker.UI.MenuSystem.Interface;
using TimeTracker.UI.MenuSystem.Styles;

namespace TimeTracker.UI.Element;

/// <summary>
/// A non-selectable menu element that renders a horizontal line (separator) to divide sections.
/// </summary>
public sealed class SeparatorElement : IStylable
{
    /// <summary>
    /// The character used to draw the separator line (repeated across the width).
    /// </summary>
    public char Character { get; set; } = '-';

    /// <summary>
    /// Gets the style for this separator (padding and alignment).
    /// </summary>
    public MenuItemStyle Style { get; } = new MenuItemStyle();
}
