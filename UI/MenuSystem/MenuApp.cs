using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using TimeTracker.UI.MenuSystem.Actions;

namespace TimeTracker.UI.MenuSystem;

public sealed class MenuApp
{
    private Window _window = null!;
    private Label _header = null!;
    private ListView _list = null!;
    private Label _footer = null!;


    private IReadOnlyList<MenuItem> _currentItems = Array.Empty<MenuItem>();
    private NavigationService _nav = null!;

    public void Run(MenuPage root)
    {
        Application.Init();

        BuildUi();

        _nav = new NavigationService(RenderPage, () => Application.RequestStop());
        _nav.Start(root);

        Application.Run();
        Application.Shutdown();
    }

    private void BuildUi()
    {
        _window = new Window
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        _header = new Label("")
        {
            X = 1,
            Y = 0,
            Width = Dim.Fill() - 2,
            Height = 1
        };

        _list = new ListView
        {
            X = 1,
            Y = 2,
            Width = Dim.Fill() - 2,
            Height = Dim.Fill() - 4
        };

        _footer = new Label("↑/↓ select  Enter open/exec  Esc back  Q quit")
        {
            X = 1,
            Y = Pos.AnchorEnd(1),
            Width = Dim.Fill() - 2,
            Height = 1
        };

        _list.OpenSelectedItem += _ => ActivateSelected();
        _list.KeyPress += OnKeyPress;

        _window.Add(_header, _list, _footer);
        Application.Top.Add(_window);
    }


    private void RenderPage(MenuPage page)
    {
        _window.Title = page.Title;
        _header.Text = $" {page.Title}";

        var items = page.GetItems().ToList();

        // Add Back/Quit consistantly (instead of the current Conditional logic)
        if (_nav.CanGoBack)
            items.Insert(0, new MenuItem { Label = "← Back", Action = new BackAction() });

        items.Add(new MenuItem { Label = "Quit", Action = new QuitAction()});

        _currentItems = items;

        _list.SetSource(items.Select(item => item.Enabled ? item.Label : $"{item.Label} (disabled)").ToList());
        _list.SelectedItem = 0;
    }

    private void ActivateSelected()
    {
        int idx = _list.SelectedItem;
        if (idx < 0 || idx >= _currentItems.Count)
            return;

        var item = _currentItems[idx];
        if (!item.Enabled)
            return;
        item.Action?.Execute(_nav);
    }

    private void OnKeyPress(View.KeyEventEventArgs args)
    {
        if (args.KeyEvent.Key == Key.Esc)
        {
            args.Handled = true;
            _nav.Pop();
            return;
        }

        if (args.KeyEvent.Key == (Key)'q' || args.KeyEvent.Key == (Key)'Q')
        {
            args.Handled = true;
            _nav.Quit();
        }
    }

    
}
