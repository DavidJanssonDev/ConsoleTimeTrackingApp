using Terminal.Gui;
using TimeTracker.UI.MenuSystem.Actions;

namespace TimeTracker.UI.MenuSystem;

public sealed class MenuApp
{
    private readonly Window _window;
    private readonly Label _header;
    private readonly ListView _list;
    private readonly Label _footer;

    private  IReadOnlyList<MenuItem> _currentItems = Array.Empty<MenuItem>();
    private readonly NavigationService _nav;

    public MenuApp()
    {
        _window = new Window()
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

        _list = new ListView()
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

        _list.OpenSelectedItem += _ => ActivateSeleted();
        _list.KeyPress += OnKeyPress;

        _window.Add(_header, _list, _footer);
        Application.Top.Add(_window);

        _nav = new NavigationService(RenderPage, () => Application.RequestStop());

    }

    public void Run(MenuPage root)
    {
        _nav.Start(root);
        Application.Run();
        Application.Shutdown();
    }

    private void RenderPage(MenuPage page)
    {
        _window.Title = page.Title;
        _header.Text = $" {page.Title}";

        var items = page.GetItems().ToList();

        // Add Back/Quit consistantly (instead of the current Conditional logic)
        if (_nav.CanGoBack)
            items.Add(new MenuItem { Label = "← Back", Action = new BackAction() });
        else 
            items.Add(new MenuItem { Label = "Q Quit", Action = new QuitAction() });
    
        _currentItems = items;

        _list.SetSource(items.Select(item => item.Enabled ? item.Label : $"{item.Label} (disabled)").ToList());
        _list.SelectedItem = 0;
    }

    private void ActivateSeleted()
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
