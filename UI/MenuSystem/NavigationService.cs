using Terminal.Gui;

namespace TimeTracker.UI.MenuSystem;

public sealed class NavigationService
{
    private readonly Stack<MenuPage> _stack = new();
    private readonly Action<MenuPage> _render;
    private readonly Action _quit;


    public NavigationService(Action<MenuPage> render, Action quit)
    {
        _render = render;
        _quit = quit;
    }

    public MenuPage Current => _stack.Peek();
    public bool CanGoBack => _stack.Count > 1;

    public void Start(MenuPage root)
    {
        _stack.Clear();
        _stack.Push(root);
        _render(Current);
    }

    public void Push(MenuPage page)
    {
        _stack.Push(page);
        _render(Current);
    }

    public void Pop() 
    {
        if (_stack.Count <= 1)
            return;
        
        _stack.Pop();
        _render(Current);
    }

    public void Quit() => _quit();
}
