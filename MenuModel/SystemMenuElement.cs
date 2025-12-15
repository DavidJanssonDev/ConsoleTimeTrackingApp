using TimeTracker.MenuModel.Interfaces;

namespace TimeTracker.MenuModel;

/// <summary>
/// A virtual menu row that is not part of the menu model.
/// Used for UI chrome like Back/Quit.
/// </summary>
internal sealed class SystemMenuElement : IMenuElement
{
    public string Title { get; }

    public bool IsQuit { get; }

    public SystemMenuElement(bool isQuit)
    {
        IsQuit = isQuit;
        Title = isQuit ? "Quit" : "← Back";
    }
}
