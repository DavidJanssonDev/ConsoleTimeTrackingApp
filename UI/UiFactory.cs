using Terminal.Gui;

namespace TimeTracker.UI;

/// <summary>
/// Builds the Terminal.Gui UI controls.
/// </summary>
internal static class UiFactory
{
    public static UiState CreateUi()
    {
        Toplevel topLevel = Application.Top;

        Window mainWindow = new Window("Shift Tracker")
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        Label helpLabel = new Label("↑/↓ move · Enter select · Esc/Backspace back")
        {
            X = Pos.Center(),
            Y = 0
        };

        ListView menuListView = new ListView()
        {
            X = Pos.Center(),
            Y = 2,
            Width = 70,
            Height = Dim.Fill() - 5,
            AllowsMarking = false
        };

        Label statusLabel = new Label(string.Empty)
        {
            X = Pos.Center(),
            Y = Pos.Bottom(menuListView) + 1
        };

        mainWindow.Add(helpLabel, menuListView, statusLabel);

        return new UiState(topLevel, mainWindow, menuListView, statusLabel);
    }
}
