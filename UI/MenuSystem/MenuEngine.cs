using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Terminal.Gui;
using TimeTracker.UI.Actions.Internal;
using TimeTracker.UI.Actions.Public;
using static Terminal.Gui.View;

namespace TimeTracker.UI.MenuSystem;

/// <summary>
/// The engine that manages menu navigation and UI rendering using Terminal.Gui.
/// It handles displaying menus, capturing user input, and executing selected actions.
/// </summary>
public class MenuEngine
{
    private readonly Stack<Menu> _menuStack = new();
    private readonly Window _mainWindow;
    private readonly ListView _listView;
    private List<MenuOption> _currentOptions = [];

    /// <summary>
    /// Initializes the menu engine with a root menu and prepares the UI.
    /// This constructor sets up the Terminal.Gui application and displays the initial menu.
    /// </summary>
    /// <param name="rootMenu">The root (main) menu to display first.</param>
    public MenuEngine(Menu rootMenu)
    {
        // Initialize Terminal.Gui application
        Application.Init();
        
        // Create the top-level window with the title of the root menu
        _mainWindow = new Window(rootMenu.Title)
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        
        // Add the window to the application
        Application.Top.Add(_mainWindow);
        
        // Create a ListView to display menu options
        _listView = new(new List<MenuOption>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        
        // Attach event handler for when an item is activated (e.g., user presses Enter or double-clicks)
        _listView.OpenSelectedItem += OnItemActivated;

        _listView.KeyPress += OnKeyPressed;

        // Add the ListView to the window
        _mainWindow.Add(_listView);
        
        // Push the root menu onto the stack and show it
        _menuStack.Push(rootMenu);
        ShowCurrentMenu();
    }

   

    /// <summary>
    /// Runs the menu application, entering the Terminal.Gui main loop.
    /// This method will block until the application is quit.
    /// </summary>
    public void Run()
    {
        Application.Run();
        Application.Shutdown();
    }

    /// <summary>
    /// Pushes a submenu onto the navigation stack and displays it.
    /// </summary>
    /// <param name="menu">The submenu to navigate into.</param>
    public void PushMenu(Menu menu)
    {
        _menuStack.Push(menu);
        ShowCurrentMenu();
    }

    /// <summary>
    /// Navigates back to the previous menu (pops the current menu off the stack).
    /// If already at the root menu, this method does nothing.
    /// </summary>
    public void GoBack()
    {
        if (_menuStack.Count > 1)
        {
            _menuStack.Pop();
            ShowCurrentMenu();
        }
    }
    /// <summary>
    /// Stops the menu application, causing the Terminal.Gui main loop to exit.
    /// </summary>
    public void Quit()
    {
        Application.RequestStop();
    }

    /// <summary>
    /// Handles selection of a menu option by executing its associated action.
    /// This method is triggered when the user activates an item in the ListView.
    /// </summary>
    /// <param name="index">The index of the selected option in the current menu.</param>
    private void ExecuteOption(int index)
    {
        if (_currentOptions == null || index < 0 || index >= _currentOptions.Count)
            return;

        MenuOption option = _currentOptions[index];
        
        if (!option.IsEnabled)
            return;

        option.Action?.Execute(this);
    }

    /// <summary>
    /// Displays the menu at the top of the stack, updating the UI to show its title and options.
    /// </summary>
    private void ShowCurrentMenu()
    {
        // Get the current menu (top of the stack)
        Menu currentMenu = _menuStack.Peek();

        // Update window title to current menu's title
        
        _mainWindow.Title = currentMenu.Title;
        // Build the list of options to display (including a Back option if needed)

        _currentOptions =
        [
            // Append all user-defined options for the menu
            .. currentMenu.Options,
        ];

        if (currentMenu.HasBackButton && _menuStack.Count > 1)
        {
            // Insert a "Back" option at the top if allowed and not at root
            _currentOptions.Add(new MenuOption("Back", new BackAction()));
        }

        if (currentMenu.HasBackButton && _menuStack.Count < 1)
        {
            _currentOptions.Add(new MenuOption("Quit", new QuitAction()));
        }
       
        
        // Show the options in the ListView
        _listView.SetSource(_currentOptions);
        
        // Ensure the first item is selected by default
        _listView.SelectedItem = 0;
    }

    /// <summary>
    /// Event handler for ListView item activation (when an option is selected by the user).
    /// This will determine which option was chosen and execute its action.
    /// </summary>
    /// <param name="args">Event args from the ListView (not used directly).</param>
    private void OnItemActivated(ListViewItemEventArgs args)
    {
        // Execute the option corresponding to the currently selected item
        ExecuteOption(_listView.SelectedItem);
    }


    /// <summary>
    /// Handles keyboard input for the menu list view.
    /// Supports arrow key shortcuts:
    /// - Left arrow triggers back if the selected item is the Back option.
    /// - Right arrow triggers navigation if the selected item is a submenu.
    /// </summary>
    /// <param name="args">The key event arguments containing key info.</param>
    private void OnKeyPressed(KeyEventEventArgs args)
    {
        Key key = args.KeyEvent.Key;
        int index = _listView.SelectedItem;

        if (index < 0 || index >= _currentOptions.Count)
            return;

        MenuOption selectedOption = _currentOptions[index];

        if (key == Key.CursorLeft && selectedOption.Action is BackAction && selectedOption.IsEnabled)
        {
            args.Handled = true;
            selectedOption.Action.Execute(this);
        }
        else if (key == Key.CursorRight && selectedOption.Action is Menu && selectedOption.IsEnabled)
        {
            args.Handled = true;
            selectedOption.Action.Execute(this);
        }
    }
}
