using Terminal.Gui;
using TimeTracker.MenuModel.Forms;
using TimeTracker.MenuModel.Interfaces;

namespace TimeTracker.UI;

/// <summary>
/// Holds main UI views.
/// Internal because plugins never need direct UI access.
/// </summary>
internal sealed class UiState(Window mainWindow, ListView menuListView, Label statusLabel)
{
    public Window MainWindow { get; } = mainWindow;
    public ListView MenuListView { get; } = menuListView;
    public Label StatusLabel { get; } = statusLabel;
    public Action<MenuForm, IReadOnlyDictionary<string, string>>? OnFormSubmitted { get; set; }
    public IReadOnlyList<IMenuElement>? CurrentMenuMapping { get; set; }
}