using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.MenuModel;
using TimeTracker.MenuModel.Forms;
using TimeTracker.MenuModel.Interfaces;
using TimeTracker.Plugins;

namespace TimeTracker.UI;

internal static class MenuNavigation
{
    public static Stack<IMenuElement> CreateStack(IMenuElement root)
    {
        var stack = new Stack<IMenuElement>();
        stack.Push(root);
        return stack;
    }


    public static void WireHandlers(UiState ui, Stack<IMenuElement> stack, ICommandContext context)
    {
        ui.MenuListView.OpenSelectedItem += _ =>
        {
            // Enter / default activation: open pages OR execute commands
            ActivateSelection(ui, stack, context, allowExecuteCommands: true);
        };


        ui.OnFormSubmitted = (form, values) =>
        {
            CommandResult result = form.OnSubmit(values);
            ApplyResult(result, ui, stack);
        };

        // Disable keys except Up/Down/Enter + Back keys
        ui.MainWindow.KeyPress += args =>
        {
            Key key = args.KeyEvent.Key;

            switch (key)
            {
                case Key.Esc:
                    if (stack.Count <= 0) return;
                    if (stack.Peek() is not MenuForm) return;

                    stack.Pop();
                    Rebuild(ui, stack);
                    args.Handled = true;
                    return;

                case Key.CursorLeft:
                    IReadOnlyList<IMenuElement>? mapping = ui.CurrentMenuMapping;
                    int selectedIndex = ui.MenuListView.SelectedItem;

                    if (mapping is not null &&
                        selectedIndex >= 0 &&
                        selectedIndex < mapping.Count &&
                        mapping[selectedIndex] is SystemMenuElement sys &&
                        sys.IsQuit == false)
                    {
                        if (stack.Count > 1)
                        {
                            stack.Pop();
                            Rebuild(ui, stack);
                        }
                    }

                    args.Handled = true;
                    return;

                case Key.CursorRight:
                    ActivateSelection(ui, stack, context, allowExecuteCommands: false);
                    args.Handled = true;
                    return;

                case Key.Enter:
                    ActivateSelection(ui, stack, context, allowExecuteCommands: true);
                    args.Handled = true;
                    return;

                default:
                    return;
            } 
        };
    }




    private static void ActivateSelection(UiState ui, Stack<IMenuElement> stack, ICommandContext context, bool allowExecuteCommands)
    {
        if (stack.Count == 0 || stack.Peek() is not MenuNode) return;

        IReadOnlyList<IMenuElement>? mapping = ui.CurrentMenuMapping;

        if (mapping is null) return;

        int index = ui.MenuListView.SelectedItem;
        if (index < 0 || index >= mapping.Count) return;

        IMenuElement selected = mapping[index];

        switch (selected)
        {
            case SystemMenuElement sys:
                if (sys.IsQuit)
                {
                    context.Quit();
                    return;
                }

                if (stack.Count > 1)
                {
                    stack.Pop();
                    Rebuild(ui, stack);
                }
                return;


            case MenuNode submenu:
                stack.Push(submenu);
                Rebuild(ui, stack);
                return;

            case MenuForm form:
                stack.Push(form);
                Rebuild(ui, stack);
                return;

            case MenuCommand leaf:
                if (!allowExecuteCommands)
                {
                    return; // Right arrow shouldn't execute commands
                }

                CommandResult result = leaf.Command.Execute(context);
                ApplyResult(result, ui, stack);
                return;
        }


    }

    private static void ApplyResult(CommandResult result, UiState ui, Stack<IMenuElement> stack)
    {
        switch (result)
        {
            case StayResult:
                Rebuild(ui, stack);
                break;

            case BackResult:
                if (stack.Count > 1)
                {
                    stack.Pop();
                }
                Rebuild(ui, stack);
                break;

            case NavigateToResult nav:
                stack.Push(nav.Target);
                Rebuild(ui, stack);
                break;

            case ReplaceCurrentResult repl:
                if (stack.Count > 0)
                {
                    stack.Pop();
                }
                stack.Push(repl.Target);
                Rebuild(ui, stack);
                break;

            case ShowMessageResult msg:
                MessageBox.Query(msg.Title, msg.Message, "OK");
                Rebuild(ui, stack);
                break;
        }
    }

    private static void Rebuild(UiState ui, Stack<IMenuElement> stack)
    {
        RebuildToDefaultMenuLayout(ui);

        if (stack.Count == 0)
        {
            return;
        }

        MenuRenderer.Show(stack.Peek(), ui, stack);
    }

    private static void RebuildToDefaultMenuLayout(UiState ui)
    {
        if (ui.MainWindow.Subviews.Contains(ui.MenuListView))
        {
            return;
        }

        ui.MainWindow.RemoveAll();
        ui.MainWindow.Add(ui.MenuListView, ui.StatusLabel);
    }
}
