using Terminal.Gui;

namespace TimeTracker.UI;

/// <summary>
/// Builds the Terminal.Gui UI controls.
/// </summary>
internal static class UiFactory
{
    public static UiState CreateUi()
    {

        Window mainWindow = new("Shift Tracker")
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        Label helpLabel = new("↑/↓ move · Enter select · Esc/Backspace back")
        {
            X = Pos.Center(),
            Y = 0
        };

        ListView menuListView = new()
        {
            X = Pos.Center(),
            Y = 2,
            Width = 70,
            Height = Dim.Fill() - 5,
            AllowsMarking = false
        };

        Label statusLabel = new(string.Empty)
        {
            X = Pos.Center(),
            Y = Pos.Bottom(menuListView) + 1
        };

        mainWindow.Add(helpLabel, menuListView, statusLabel);

        return new UiState(mainWindow, menuListView, statusLabel);
    }
}
