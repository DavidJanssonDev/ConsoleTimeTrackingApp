using System.Collections.Generic;
using TimeTracker.MenuModel;
using TimeTracker.MenuModel.Forms;
using TimeTracker.MenuModel.Interfaces;

namespace TimeTracker.UI;

/// <summary>
/// Renders a MenuNode into the ListView.
/// </summary>
internal static class MenuRenderer
{
    public static void Show(IMenuElement element, UiState ui)
    {
        switch (element)
        {
            case MenuNode menu:
                ShowMenu(menu, ui);
                break;

            case MenuForm form:
                FormRenderer.ShowForm(form, ui);
                break;
        }

    }

    private static void ShowMenu(MenuNode menu, UiState ui)
    {
        ui.MainWindow.Title = menu.Title;

        List<string> lines = [];

        foreach(IMenuElement child in menu.Items)
        {
            bool isSubmenu = child is MenuNode or MenuForm;
            string arrow = isSubmenu ? "▶" : string.Empty;
            lines.Add(child.Title + arrow);
        }

        ui.MenuListView.SetSource(lines);
        ui.MenuListView.SelectedItem = 0;

        if (!string.IsNullOrWhiteSpace(menu.Footer)) 
            ui.StatusLabel.Text = menu.Footer;
    }
}
