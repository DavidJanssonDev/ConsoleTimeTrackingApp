using System.Collections.Generic;
using TimeTracker.MenuModel.Interfaces;

namespace TimeTracker.MenuModel;

/// <summary>
/// Represents a menu screen that can contain multiple menu elements.
/// Like a menu / ul in HTML.
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
    /// Optional footer text shown in the status/footer area.
    /// If null, the previous footer remains.
    /// </summary>
    public string? Footer { get; set; }

    /// <summary>
    /// Elements displayed inside this menu.
    /// Menus are MenuNode; commands are MenuCommand.
    /// </summary>
    public List<IMenuElement> Items { get; } = new List<IMenuElement>();
}
