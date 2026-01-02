using System;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace TimeTracker.UI.MenuSystem.Layout;

/// <summary>
/// Vertical layout container similar to a simple flex-column.
/// Supports spacing (gap) and one or more "fill" children that take remaining space.
/// </summary>
public sealed class VStack : View
{
    public int Spacing { get; set; } = 0;
    public int Padding { get; set; } = 0;

    /// <summary>
    /// Optional child that will be given the remaining height after fixed children are laid out.
    /// </summary>
    public View? FillChild { get; set; }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();

        int innerX = Padding;
        int innerY = Padding;
        int innerWidth = Math.Max(0, Bounds.Width - (Padding * 2));
        int innerHeight = Math.Max(0, Bounds.Height - (Padding * 2));

        // Calculate fixed height usage (everything except FillChild).
        int fixedUsed = 0;
        int count = Subviews.Count;

        for (int i = 0; i < count; i++)
        {
            var child = Subviews[i];
            if (child == FillChild)
                continue;

            // Default to one line if not specified.
            if (child.Height is null)
                child.Height = 1;

            fixedUsed += ResolveHeight(child);
        }

        int gaps = Math.Max(0, count - 1);
        int spacingUsed = gaps * Spacing;

        int remaining = Math.Max(0, innerHeight - fixedUsed - spacingUsed);

        // Layout pass
        int y = innerY;

        for (int i = 0; i < count; i++)
        {
            var child = Subviews[i];

            child.X = innerX;
            child.Width = innerWidth;
            child.Y = y;

            if (child == FillChild)
            {
                child.Height = remaining;
                y += remaining + Spacing;
                continue;
            }

            int h = ResolveHeight(child);
            child.Height = h;
            y += h + Spacing;
        }
    }

    private static int ResolveHeight(View child)
    {
        // If the frame has a height already, use it.
        if (child.Frame.Height > 0)
            return child.Frame.Height;

        // If height was set to an absolute int (common for labels/buttons),
        // it will usually be available via Frame after layout, but default to 1.
        return 1;
    }
}