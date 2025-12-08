using TimeTracker.MenuModel;
using TimeTracker.Store;

namespace TimeTracker.Plugins;

/// <summary>
/// Plugin contract. External DLLs implement this.
/// </summary>
public interface ICommand
{
    string DisplayName { get; }
    string Category { get; }
    bool CanHaveSubmenu { get; }

    IShiftStore ShiftStore { get; set; }

    void Execute();
    MenuNode? BuildSubmenu();
}
