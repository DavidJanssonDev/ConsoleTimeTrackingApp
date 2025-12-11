

using TimeTracker.MenuModel.Interfaces;

namespace TimeTracker.MenuModel;

internal sealed class MenuGroup(string title) : IMenuElement
{
    public string Title { get; } = title;
    public List<IMenuElement> Children { get; } = [];

}
