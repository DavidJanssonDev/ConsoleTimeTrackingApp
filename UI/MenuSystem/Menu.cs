using TimeTracker.UI.Interfaces;

namespace TimeTracker.UI.MenuSystem;

/// <summary>
/// Defines an interface for actions that can be performed when a menu option is selected.
/// Implementations can include navigating to submenus, executing commands, or quitting the application.
/// </summary>
public class Menu : IMenuAction
{
    /// <summary>
    /// Gets or sets the title of this menu (used as window title when rendered).
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets whether this menu should display a "Back" option to return to the parent menu.
    /// If true, a "Back" option will be automatically shown (except for root menu which has no parent).
    /// </summary>
    public bool HasBackButton { get; set; }

    /// <summary>
    /// Gets the collection of menu options in this menu.
    /// Use object initializer syntax to add <see cref="MenuOption"/> items to this list.
    /// </summary>
    public List<MenuOption> Options { get; } = [];

    /// <summary>
    /// Initializes a new menu with the specified title.
    /// </summary>
    /// <param name="title">The title of the menu.</param>
    public Menu(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Executes this menu as an action by navigating into it using the provided menu engine.
    /// This method is used to treat a submenu as a selectable action (to navigate into that submenu).
    /// </summary>
    /// <param name="engine">The menu engine controlling navigation.</param>
    public void Execute(MenuEngine engine)
    {
        engine.PushMenu(this);
    }
}
