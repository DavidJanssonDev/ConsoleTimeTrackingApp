using TimeTracker.Plugins;

namespace TimeTracker.MenuModel;

/// <summary>
/// A selectable item in a menu.
/// 
/// A MenuItem can do ONE of two things:
/// 1) Execute a command (Command != null)
/// 2) Open a submenu (Submenu != null)
/// 
/// Public because MenuNode.Items exposes MenuItem to plugins.
/// </summary>
public sealed class MenuItem
{
    /// <summary>
    /// Text shown in the ListView.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Command executed when this item is selected.
    /// Null if this item is only for navigation to a submenu.
    /// </summary>
    public ICommand? Command { get; }

    /// <summary>
    /// Submenu to open when selected.
    /// Null if this item is a command leaf.
    /// </summary>
    public MenuNode? Submenu { get; set; }

    /// <summary>
    /// If true, UI will show a ▶ indicator.
    /// 
    /// True when:
    /// - Submenu already exists
    /// - OR command can dynamically create one.
    /// </summary>
    public bool HasSubmenuHint { get; }

    /// <summary>
    /// Creates a command item (leaf).
    /// </summary>
    public MenuItem(string text, ICommand command)
    {
        Text = text;
        Command = command;
        Submenu = null;

        // If the command *can* build a submenu later,
        // show the arrow hint even before it exists.
        HasSubmenuHint = command.CanHaveSubmenu;
    }

    /// <summary>
    /// Creates a submenu navigation item.
    /// </summary>
    public MenuItem(string text, MenuNode submenu)
    {
        Text = text;
        Submenu = submenu;
        Command = null;

        // A submenu always shows the arrow.
        HasSubmenuHint = true;
    }
}
