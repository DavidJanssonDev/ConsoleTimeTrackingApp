using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace TimeTracker.UI.MenuSystem.Layout;

/// <summary>
/// A simple horizontal stack layout container (like a flex row).
/// Positions children left to right with optional spacing.
/// </summary>
public sealed class HStack : View
{
    /// <summary>
    /// Blank columns between children (similar to CSS gap).
    /// </summary>
    public int Spacing { get; set; } = 0;

    /// <summary>
    /// Padding inside the container (applies on all sides).
    /// </summary>
    public int Padding { get; set; } = 0;

    /// <inheritdoc/>
    public override void LayoutSubviews()
    {
        base.LayoutSubviews();

        int x = Padding;
        int availableWidth = Math.Max(0, Bounds.Width - (Padding * 2));
        int availableHeight = Math.Max(0, Bounds.Height - (Padding * 2));

        foreach (var child in Subviews)
        {
            // If the child has no explicit height, stretch it.
            if (child.Height is null)
                child.Height = availableHeight;

            child.X = x;
            child.Y = Padding;

            // If width is not explicitly set, use the current frame width or default to 1.
            int childWidth = child.Frame.Width > 0 ? child.Frame.Width : 1;

            // Stop if we run out of space.
            if (x - Padding + childWidth > availableWidth)
                break;

            x += childWidth + Spacing;
        }
    }
}
