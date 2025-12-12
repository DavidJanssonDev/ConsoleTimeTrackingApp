using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.MenuModel.Interfaces;
using TimeTracker.Plugins;

namespace TimeTracker.MenuModel.Forms;

public sealed class MenuForm : IMenuElement
{
    public string Title { get;}
    public string? Footer { get; set; }

    public List<FormField> Fields { get; } = new();

    /// <summary>
    /// Called when the user submits the form.
    /// Return a CommandResult so the form can navigate/refresh/etc.
    /// </summary>
    public Func<IReadOnlyDictionary<string, string>, CommandResult> OnSubmit { get; }

    public MenuForm(
        string title,
        Func<IReadOnlyDictionary<string, string>, CommandResult> onSubmit)
    {
        Title = title;
        OnSubmit = onSubmit;
    }
}
