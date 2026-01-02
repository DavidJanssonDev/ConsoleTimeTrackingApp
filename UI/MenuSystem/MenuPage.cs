using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.UI.MenuSystem;

public sealed class MenuPage
{
    public required string Title { get; init; }

    public IReadOnlyList<MenuItem> Items { get; init; } = Array.Empty<MenuItem>();

    public Func<IReadOnlyList<MenuItem>>? LoadItems { get; init; }

    public IReadOnlyList<MenuItem> GetItems()
        => LoadItems?.Invoke() ?? Items;

}
