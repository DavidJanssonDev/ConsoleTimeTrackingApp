using System.Collections.Generic;
using TimeTracker.MenuModel;

namespace TimeTracker.UI;

/// <summary>
/// Renders a MenuNode into the ListView.
/// </summary>
internal static class MenuRenderer
{
    public static void ShowMenu(MenuNode menu, UiState ui)
    {
        ui.MainWindow.Title = menu.Title;

        List<string> items = new List<string>();
        for (int i = 0; i < menu.Items.Count; i++)
        {
            MenuItem item = menu.Items[i];
            string arrow = item.HasSubmenuHint ? " ▶" : string.Empty;
            items.Add(item.Text + arrow);
        }

        ui.MenuListView.SetSource(items);
        ui.MenuListView.SelectedItem = 0;

        if (!string.IsNullOrWhiteSpace(menu.Footer))
        {
            ui.StatusLabel.Text = menu.Footer;
        }
    }
}
