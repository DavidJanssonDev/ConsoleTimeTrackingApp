using Terminal.Gui;
using TimeTracker.UI.MenuSystem.Actions;
using TimeTracker.UI.MenuSystem.Layout;
using TimeTracker.UI.MenuSystem.Views;

namespace TimeTracker.UI.MenuSystem;

public sealed class MenuApp
{
    private Window _window = null!;
    private Label _header = null!;
    private MenuListView _menu = null!;
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

        _header = new Label(string.Empty)
        {
            Height = 1
        };

        _menu = new MenuListView
        {
            Height = Dim.Fill(),   // ✅ IMPORTANT: let VStack allocate remaining space
            PaddingLeft = 1,
            Spacing = 0,
            CanFocus = true
        };

        _footer = new Label("↑/↓ select   Enter open/exec   Esc back   Q quit")
        {
            Height = 1
        };

        _menu.ItemActivated += ActivateSelected;

        // Global keys (works regardless of focus)
        _window.KeyPress += OnKeyPress;

        var rootLayout = new VStack
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            Padding = 1,
            Spacing = 1,
            FillChild = _menu // ✅ this is the important line
        };

        rootLayout.Add(_header, _menu, _footer);

        _window.Add(rootLayout);
        Application.Top.Add(_window);

        _menu.SetFocus();
    }


    private void RenderPage(MenuPage page)
    {
        _window.Title = page.Title;
        _header.Text = page.Title;

        List<MenuItem>? items = page.GetItems().ToList();

        if (_nav.CanGoBack)
            items.Add(new MenuItem
            {
                Label = "← Back",
                Action = new BackAction()
            });

        else
            items.Add(new MenuItem
            {
                Label = "Quit",
                Action = new QuitAction()
            });

        _currentItems = items;

        var labels = items.Select(i => i.Label).ToList();
        var disabledIndexes = items
            .Select((item, index) => (item, index))
            .Where(x => !x.item.Enabled)
            .Select(x => x.index)
            .ToList();

        _menu.SetItems(labels, disabledIndexes);
    }

    private void ActivateSelected(int index)
    {
        if (index < 0 || index >= _currentItems.Count)
            return;

        var item = _currentItems[index];
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
