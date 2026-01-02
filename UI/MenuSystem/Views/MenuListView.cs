using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace TimeTracker.UI.MenuSystem.Views;

/// <summary>
/// A lightweight, custom-drawn vertical menu list.
/// Owns selection, keyboard handling, and rendering.
/// </summary>
public sealed class MenuListView : View
{
    private IReadOnlyList<string> _items = Array.Empty<string>();
    private readonly HashSet<int> _disabledIndexes = new();

    /// <summary>
    /// The currently selected index (0-based).
    /// </summary>
    public int SelectedIndex { get; private set; }

    /// <summary>
    /// Vertical spacing (blank lines) between items.
    /// </summary>
    public int Spacing { get; set; } = 0;

    /// <summary>
    /// Left padding for text inside the view.
    /// </summary>
    public int PaddingLeft { get; set; } = 0;

    /// <summary>
    /// Fired when the user activates the current selection (Enter).
    /// </summary>
    public event Action<int>? ItemActivated;

    /// <summary>
    /// Replace the menu contents
    /// </summary>
    public void SetItems(IReadOnlyList<string> items, IEnumerable<int>? disabledIndexes = null)
    {
        _items = items ?? Array.Empty<string>();
        _disabledIndexes.Clear();

        if (disabledIndexes is not null)
            foreach (int index in disabledIndexes)
                _disabledIndexes.Add(index);

        SelectedIndex = Clamp(SelectedIndex, 0, Math.Max(0, _items.Count - 1));
        SetNeedsDisplay();
    }

    public override bool OnKeyDown(KeyEvent keyEvent)
    {
        if (_items.Count == 0)
            return false;

        switch (keyEvent.Key)
        {
            case Key.CursorUp:
                MoveSelection(-1);
                return true;

            case Key.CursorDown:
                MoveSelection(+1);
                return true;

            case Key.PageUp:
                MoveSelection(-Math.Max(1, Bounds.Height / 2));
                return true;

            case Key.PageDown:
                MoveSelection(+Math.Max(1, Bounds.Height / 2));
                return true;

            case Key.Home:
                SelectedIndex = 0;
                SkipDisabledForward();
                SetNeedsDisplay();
                return true;

            case Key.End:
                SelectedIndex = _items.Count - 1;
                SkipDisabledBackward();
                SetNeedsDisplay();
                return true;

            case Key.Enter:
                if (!_disabledIndexes.Contains(SelectedIndex))
                    ItemActivated?.Invoke(SelectedIndex);
                return true;
        }
        return false;
    }

    public override void OnDrawContent(Rect bounds)
    {
        Driver.SetAttribute(ColorScheme?.Normal ?? Driver.MakeAttribute(Color.White, Color.Black));
        Clear();

        if (_items.Count == 0)
            return;

        int line = 0;

        for (int index = 0; index < _items.Count; index++)
        {
            int y = line;
            if (y >= bounds.Height)
                break;

            bool isSelected = index == SelectedIndex;
            bool isDisabled = _disabledIndexes.Contains(index);

            var attr = GetItemAttribute(isSelected, isDisabled);
            Driver.SetAttribute(attr);

            Move(PaddingLeft, y);

            string text = _items[index] ?? string.Empty;

            // Trim/pad to available width (keep it safe in the terminal)
            int maxWidth = Math.Max(0, bounds.Width - PaddingLeft);
            if (text.Length > maxWidth)
                text = text.Substring(0, maxWidth);

            Driver.AddStr(text); // ✅ FIXED (was Driver.AddStr(Text))
            line += 1 + Spacing;
        }
    }

    private void MoveSelection(int delta)
    {
        SelectedIndex = Clamp(SelectedIndex + delta, 0, _items.Count - 1);

        // If we land on a disabled entry, skip over it in the direction we moved.
        if (_disabledIndexes.Contains(SelectedIndex))
        {
            if (delta >= 0)
                SkipDisabledForward();
            else
                SkipDisabledBackward();
        }

        SetNeedsDisplay();
    }

    private void SkipDisabledForward()
    {
        while (SelectedIndex < _items.Count - 1 && _disabledIndexes.Contains(SelectedIndex))
            SelectedIndex++;
    }

    private void SkipDisabledBackward()
    {
        while (SelectedIndex > 0 && _disabledIndexes.Contains(SelectedIndex))
            SelectedIndex--;
    }

    private Terminal.Gui.Attribute GetItemAttribute(bool isSelected, bool isDisabled)
    {
        var scheme = ColorScheme ?? Colors.Base;

        if (isDisabled)
            return scheme.Disabled;

        return isSelected ? scheme.Focus : scheme.Normal;
    }

    private static int Clamp(int value, int min, int max) 
        => value < min ? min : (value > max ? max : value);
}
