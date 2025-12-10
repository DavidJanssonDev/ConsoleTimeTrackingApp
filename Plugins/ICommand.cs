using TimeTracker.MenuModel;
using TimeTracker.Store;

namespace TimeTracker.Plugins;

/// <summary>
/// Represents a command that can be displayed in a menu and executed by the user.
/// Implementations may optionally provide a submenu for nested command structures.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Gets the human-readable name of the command as shown to the user.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Gets the category name used to group this command in the UI.
    /// </summary>
    string Category { get; }

    /// <summary>
    /// Gets a value indicating whether this command can provide a submenu.
    /// If <see langword="true"/>, <see cref="BuildSubmenu"/> may return a non-null menu node.
    /// </summary>
    bool CanHaveSubmenu { get; }

    /// <summary>
    /// Gets or sets the shift store providing access to shift-related data
    /// that the command may read or modify during execution.
    /// </summary>
    IShiftStore ShiftStore { get; set; }

    /// <summary>
    /// Executes the command's action.
    /// </summary>
    void Execute();

    /// <summary>
    /// Builds the submenu for this command, if supported.
    /// </summary>
    /// <returns>
    /// A <see cref="MenuNode"/> describing the submenu, or <see langword="null"/>
    /// if no submenu is available.
    /// </returns>
    MenuNode? BuildSubmenu();
}
