using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.MenuModel;
using TimeTracker.MenuModel.Interfaces;
using MenuItem = TimeTracker.MenuModel.MenuItem;

namespace TimeTracker.UI;

/// <summary>
/// Handles Enter to open/execute and Esc to go back.
/// </summary>
internal static class MenuNavigation
{
    public static void OnEnter(IMenuElement selected, Stack<IMenuElement> stack, UiState ui)
    {
        switch (selected)
        {
            case MenuNode submenu:
                stack.Push(submenu);
                MenuRenderer.ShowMenu(submenu, ui);
                break;

            case MenuCommand leaf:
                // lazy submenu support
                if (leaf.Command.CanHaveSubmenu)
                {
                    MenuNode? built = leaf.Command.BuildSubmenu();
                    if (built is not null)
                    {
                        stack.Push(built);
                        MenuRenderer.ShowMenu(built, ui);
                        return;
                    }
                }

                leaf.Command.Execute();
                break;
        }
    public static Stack<MenuNode> CreateStack(MenuNode rootMenu)
    {
        Stack<MenuNode> stack = new Stack<MenuNode>();
        stack.Push(rootMenu);
        return stack;
    }

    public static void WireHandlers(UiState ui, Stack<MenuNode> stack)
    {
        WireSelectionHandler(ui, stack);
        WireBackHandler(ui, stack);
    }

    private static void WireSelectionHandler(UiState ui, Stack<MenuNode> stack)
    {
        ui.MenuListView.OpenSelectedItem += (ListViewItemEventArgs args) =>
        {
            MenuNode current = stack.Peek();
            int index = args.Item;

            MenuItem selected = current.Items[index];

            if (selected.Submenu != null)
            {
                stack.Push(selected.Submenu);
                MenuRenderer.ShowMenu(stack.Peek(), ui);
                return;
            }

            if (selected.Command != null)
            {
                MenuNode? built = selected.Command.BuildSubmenu();
                if (built != null)
                {
                    selected.Submenu = built;
                    stack.Push(built);
                    MenuRenderer.ShowMenu(stack.Peek(), ui);
                    return;
                }

                selected.Command.Execute();
                FooterUpdater.Update(ui.StatusLabel, selected.Command.ShiftStore);
            }
        };
    }

    private static void WireBackHandler(UiState ui, Stack<MenuNode> stack)
    {
        ui.MainWindow.KeyPress += (View.KeyEventEventArgs args) =>
        {
            Key key = args.KeyEvent.Key;
            bool isBack = key == Key.Esc || key == Key.Backspace;

            if (!isBack || stack.Count <= 1)
            {
                return;
            }

            stack.Pop();
            MenuRenderer.ShowMenu(stack.Peek(), ui);
            args.Handled = true;
        };
    }
}
