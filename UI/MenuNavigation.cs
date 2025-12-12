using System.Collections.Generic;
using System.Security.Cryptography;
using Terminal.Gui;
using TimeTracker.MenuModel;
using TimeTracker.MenuModel.Forms;
using TimeTracker.MenuModel.Interfaces;
using TimeTracker.Plugins;

namespace TimeTracker.UI;

/// <summary>
/// Handles Enter to open/execute and Esc to go back.
/// </summary>
internal static class MenuNavigation
{
    public static Stack<MenuNode> CreateStack(MenuNode rootMenu)
    {
        Stack<MenuNode> stack = new();
        stack.Push(rootMenu);
        return stack;
    }
    public static void WireHandlers(UiState ui, Stack<IMenuElement> stack, ICommandContext context)
    {
        // Menu selection handler (works when current is MenuNode)
        ui.MenuListView.OpenSelectedItem += args =>
        {
            if (stack.Peek() is not MenuNode currentMenu)
                return;

            int index = args.Item;

            if (index < 0 || index >= currentMenu.Items.Count)
                return;

            IMenuElement selected = currentMenu.Items[index];

            switch (selected)
            {
                case MenuNode submenu:
                    stack.Push(submenu);
                    MenuRenderer.Show(stack.Peek(), ui);
                    break;

                case MenuForm form:
                    stack.Push(form);
                    MenuRenderer.Show(stack.Peek(), ui);
                    break;

                case MenuCommand leaf:
                    CommandResult result = leaf.Command.Execute(context);
                    ApplyResult(result, ui, stack);
                    break;
            }
        };

        ui.OnFormSubmitted = (form, values) =>
        {
            CommandResult result = form.OnSubmit(values);
            ApplyResult(result, ui, stack);
        };

        ui.MainWindow.KeyPress += args =>
        {
            Key key = args.KeyEvent.Key;
            bool isBack = key == Key.Esc || key == Key.Backspace;

            if (!isBack || stack.Count <= 1)
                return;

            stack.Pop();
            MenuRenderer.Show(stack.Peek(), ui);
            args.Handled = true;
        };
    }

    private static void ApplyResult(CommandResult result, UiState ui, Stack<IMenuElement> stack)
    {
        switch (result)
        {
            case StayResult:
                MenuRenderer.Show(stack.Peek(), ui);
                break;

            case BackResult:
                if (stack.Count > 1)
                    stack.Pop();
                MenuRenderer.Show(stack.Peek(), ui);
                break;

            case NavigateToResult nav:
                stack.Push(nav.Target);
                MenuRenderer.Show(stack.Peek(), ui);
                break;

            case ReplaceCurrentResult repl:
                if (stack.Count > 0) 
                    stack.Pop();

                stack.Push(repl.Target);
                MenuRenderer.Show(stack.Peek(), ui);
                break;

            case ShowMessageResult msg:
                MessageBox.Query(msg.Title, msg.Message, "Ok");
                MenuRenderer.Show(stack.Peek(), ui);
                break;
                   
        }
    }
}
