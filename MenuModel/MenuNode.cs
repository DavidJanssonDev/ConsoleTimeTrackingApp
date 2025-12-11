using System.Collections.Generic;
using TimeTracker.MenuModel.Interfaces;
namespace TimeTracker.MenuModel;

/// <summary>
/// Represents a menu screen that can contain multiple menu items.
/// 
/// Public because plugins return MenuNode from ICommand.BuildSubmenu().
/// </summary>
public sealed class MenuNode(string title) : IMenuElement
{
    /// <summary>
    /// Title shown at the top of the window when this menu is active.
    /// </summary>
    public string Title { get; } = title;

    /// <summary>
    /// Items that will be displayed inside this menu.
    /// 
    /// Public because external plugins may build submenus dynamically.
    /// </summary>
    public List<IMenuElement> Items { get; } = [];

}
