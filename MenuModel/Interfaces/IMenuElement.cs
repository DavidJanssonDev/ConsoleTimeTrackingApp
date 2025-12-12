namespace TimeTracker.MenuModel.Interfaces;

/// <summary>
/// Base type for anything that can appear in a menu tree.
/// Think of this like a DOM node.
/// </summary>
public interface IMenuElement
{
    /// <summary>
    /// Text/title to show in the UI.
    /// </summary>
    string Title { get; }
}