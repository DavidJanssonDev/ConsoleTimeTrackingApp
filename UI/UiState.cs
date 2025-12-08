using Terminal.Gui;

namespace TimeTracker.UI;

/// <summary>
/// Holds main UI views.
/// Internal because plugins never need direct UI access.
/// </summary>
internal sealed class UiState
{
    public Toplevel TopLevel { get; }
    public Window MainWindow { get; }
    public ListView MenuListView { get; }
    public Label StatusLabel { get; }

    public UiState(Toplevel topLevel, Window mainWindow, ListView menuListView, Label statusLabel)
    {
        TopLevel = topLevel;
        MainWindow = mainWindow;
        MenuListView = menuListView;
        StatusLabel = statusLabel;
    }
}