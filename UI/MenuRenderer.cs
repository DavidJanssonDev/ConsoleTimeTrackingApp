using System.Collections.Generic;
using TimeTracker.MenuModel;
using TimeTracker.MenuModel.Forms;
using TimeTracker.MenuModel.Interfaces;

namespace TimeTracker.UI;

internal static class MenuRenderer
{
    public static void Show(IMenuElement element, UiState ui, Stack<IMenuElement> stack)
    {
        switch (element)
        {
            case MenuNode menu:
                ShowMenu(menu, ui, stack);
                break;

            case MenuForm form:
                FormRenderer.ShowForm(form, ui);
                break;
        }
    }

    private static void ShowMenu(MenuNode menu, UiState ui, Stack<IMenuElement> stack)
    {
        ui.MainWindow.Title = menu.Title;

        var lines = new List<string>();
        var mapping = new List<IMenuElement>();

        // Real items
        foreach (IMenuElement child in menu.Items)
        {
            bool isPage = child is MenuNode or MenuForm;
            lines.Add(child.Title + (isPage ? " ▶" : ""));
            mapping.Add(child);
        }

        // Virtual Back/Quit LAST row
        bool isRoot = stack.Count <= 1;
        lines.Add(isRoot ? "Quit" : "← Back");
        mapping.Add(new SystemMenuElement(isQuit: isRoot));

        ui.MenuListView.SetSource(lines);

        ui.CurrentMenuMapping = mapping;

        ui.MenuListView.SelectedItem = 0;

        if (!string.IsNullOrWhiteSpace(menu.Footer))
        {
            ui.StatusLabel.Text = menu.Footer;
        }
    }
}
