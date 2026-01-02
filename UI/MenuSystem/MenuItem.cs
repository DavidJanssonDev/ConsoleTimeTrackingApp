using TimeTracker.UI.MenuSystem.Interface;

namespace TimeTracker.UI.MenuSystem;


/// <summary>
/// Represents a single selectable entry in a <see cref="MenuPage"/>.
///
/// <para>
/// A <see cref="MenuItem"/> is a pure model object:
/// it contains no UI logic, rendering, or layout information.
/// </para>
///
/// <para>
/// The visual representation (colors, disabled styling, layout)
/// and interaction behavior are handled entirely by the menu renderer.
/// </para>
/// </summary>
public sealed class MenuItem
{
    /// <summary>
    /// The text shown to the user for this menu item.
    ///
    /// <para>
    /// This value should be short, descriptive, and unique within
    /// the containing menu page.
    /// </para>
    /// </summary>
    public required string Label { get; init; }

    /// <summary>
    /// Indicates whether this menu item can currently be selected
    /// and executed.
    ///
    /// <para>
    /// Disabled items are expected to be rendered differently
    /// (for example, dimmed or annotated) and must not execute
    /// their associated action.
    /// </para>
    /// </summary>
    public bool Enabled { get; init; } = true;


    /// <summary>
    /// The action executed when this menu item is activated.
    ///
    /// <para>
    /// The action may perform navigation, execute a command,
    /// or trigger UI behavior such as opening a dialog.
    /// </para>
    ///
    /// <para>
    /// If this value is <see langword="null"/>, the item is treated
    /// as non-interactive and should not perform any operation
    /// when selected.
    /// </para>
    /// </summary>
    public IMenuAction? Action { get; init; }
}
