using Terminal.Gui;
using TimeTracker.UI.Menu.Comand;

namespace TimeTracker.UI.Menu;

/// <summary>
/// Manages all user interface navigation for the TimeTracker TUI app.
/// Supports stacked navigation between menus and submenus, execution of plugin commands,
/// and rendering of dynamic menu content.
/// </summary>
public class MenuSystem()
{
    /// <summary>
    /// Manages all user interface navigation for the TimeTracker TUI app.
    /// Supports stacked navigation between menus and submenus, execution of plugin commands,
    /// and rendering of dynamic menu content.
    /// </summary>
    private readonly Stack<MenuContext> _navigationStack = new();

    /// <summary>
    /// The currently displayed view (menu window).
    /// </summary>
    private View? _currentView;

    /// <summary>
    /// Renders and navigates into a new menu composed of a list of menu actions.
    /// This also pushes the new menu context to the navigation stack.
    /// </summary>
    /// <param name="actions">The list of menu actions or submenus to display.</param>
    /// <param name="title">Optional title for the menu window.</param>
    public void ShowMenu(List<IMenuAction> actions, string title = "TimeTracker")
    {
        var menuItems = new List<IMenuAction>(actions);

        // Add navigation option
        if (_navigationStack.Count == 0)
            menuItems.Add(new QuitCommand());
        else
            menuItems.Add(new BackCommand());

        var context = new MenuContext(title, menuItems);

        RenderMenu(context, pushToStack: true);
    }

    /// <summary>
    /// Navigates one level back in the menu stack.
    /// If already at the root, this exits the application.
    /// </summary>
    public void Back()
    {
        if (_navigationStack.Count > 0)
        {
            _navigationStack.Pop();
        }

        if (_navigationStack.Count == 0)
        {
            Application.RequestStop();
            return;
        }

        RenderMenu(_navigationStack.Peek(), pushToStack: false);
    }

    /// <summary>
    /// Renders the menu window for the given context.
    /// Optionally pushes the menu onto the stack if not already present.
    /// </summary>
    /// <param name="menu">The menu context to render (title and actions).</param>
    /// <param name="pushToStack">If true, the menu is added to the navigation stack.</param>
    private void RenderMenu(MenuContext menu, bool pushToStack)
    {
        if (pushToStack)
        {
            _navigationStack.Push(menu);
        }

        if (_currentView != null)
        {
            Application.Top.Remove(_currentView);
        }

        var win = new Window(menu.Title)
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ColorScheme = new ColorScheme
            {
                Normal = Application.Driver.MakeAttribute(Color.White, Color.Black),
                Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray)
            }
        };

        var listView = new ListView(menu.Actions.Select(a => a.Title).ToList())
        {
            X = Pos.Center(),
            Y = 2,
            Width = Dim.Percent(60),
            Height = Dim.Fill(2),
            CanFocus = true
        };

        listView.OpenSelectedItem += async args =>
        {
            var action = menu.Actions[args.Item];

            switch (action)
            {
                case BackCommand:
                    Back();
                    return;
                case QuitCommand:
                    Application.RequestStop();
                    return;
                case ISubmenuAction submenu:
                    ShowMenu(submenu.SubActions, action.Title);
                    return;
                default:
                    var currentContext = _navigationStack.Peek();
                    await action.ExecuteAsync(this);
                    RenderMenu(currentContext, pushToStack: false);
                    break;
            }
        };

        win.Add(listView);
        _currentView = win;
        Application.Top.Add(win);
        Application.Refresh();
    }
    /// <summary>
    /// Represents a single menu's state, including its display title and set of actions.
    /// </summary>
    /// <param name="Title">The title displayed at the top of the window.</param>
    /// <param name="Actions">The menu items shown to the user.</param>
    private record MenuContext(string Title, List<IMenuAction> Actions);
}

