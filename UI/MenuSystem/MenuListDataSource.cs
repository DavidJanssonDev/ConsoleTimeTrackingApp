using System;
using System.Collections;
using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.UI.Element;
using TimeTracker.UI.Interfaces;
using TimeTracker.UI.MenuSystem.Interface;

namespace TimeTracker.UI.MenuSystem;

/// <summary>
/// A data source for rendering a list of IMenuElement items in a ListView.
/// </summary>
public class MenuListDataSource : IListDataSource
{
    private readonly IList<IMenuElement> elements;

    /// <summary>
    /// Initializes a new instance of the MenuListDataSource class.
    /// </summary>
    /// <param name="items">The menu elements to display in the list.</param>
    public MenuListDataSource(IList<IMenuElement> items)
    {
        elements = items;
    }
    
    /// <inheritdoc/>
    public bool IsMarked(int item) => false;

    /// <inheritdoc/>
    public void SetMark(int item, bool value) { }

    /// <inheritdoc/>
    public int Count => elements.Count;

    /// <inheritdoc/>
    public int Length => elements.Count;

    /// <inheritdoc/>
    public IList ToList() => (IList)elements;

    /// <inheritdoc/>
    public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width, int start = 0)
    {
        var element = elements[item];
        string text = element switch
        {
            SeparatorElement sep => new string(sep.Character, width),
            HeaderElement header => header.Text,
            MenuOption option => option.Label,
            _ => element?.ToString() ?? string.Empty
        };

        if (element is IStylable stylable && stylable.Style is { } style)
        {
            text = ApplyPadding(text, style.Padding, width);
        }

        container.Move(col, line);
        driver.AddStr(text.PadRight(width));
    }

    /// <summary>
    /// Applies left and right padding to a string and ensures it fits within a given width.
    /// </summary>
    /// <param name="text">The text to pad.</param>
    /// <param name="padding">The padding to apply.</param>
    /// <param name="width">The maximum width of the output string.</param>
    /// <returns>A padded string of the specified width.</returns>
    private string ApplyPadding(string text, Padding padding, int width)
    {
        var left = new string(' ', padding.Left);
        var right = new string(' ', padding.Right);
        var paddedText = $"{left}{text}{right}";
        return paddedText.Length > width ? paddedText.Substring(0, width) : paddedText.PadRight(width);
    }
}





