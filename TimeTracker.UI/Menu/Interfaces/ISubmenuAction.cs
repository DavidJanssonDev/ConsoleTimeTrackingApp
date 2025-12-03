namespace TimeTracker.UI.Menu.Interfaces;

public interface ISubmenuAction : IMenuAction
{
    List<IMenuAction> SubActions { get; }
}