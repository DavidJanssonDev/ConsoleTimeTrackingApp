using System.Collections.Generic;
namespace TimeTracker.MenuModel;

/// <summary>
/// Represents a menu screen that can contain multiple menu items.
/// 
/// Public because plugins return MenuNode from ICommand.BuildSubmenu().
/// </summary>
public sealed class MenuNode
{
    /// <summary>
    /// Title shown at the top of the window when this menu is active.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Optional footer text shown in the status/footer area.
    /// If null, the previous footer remains.
    /// </summary>
    public string? Footer { get; set; }

    /// <summary>
    /// Items that will be displayed inside this menu.
    /// 
    /// Public because external plugins may build submenus dynamically.
    /// </summary>
    public List<MenuItem> Items { get; }

    /// <summary>
    /// Creates a new menu with a title and empty items list.
    /// </summary>
    public MenuNode(string title)
    {
        Title = title;
        Items = new List<MenuItem>();
    }
}
