using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.UI.MenuSystem.Styles;

/// <summary>
/// Represents padding around content with four sides (top, bottom, left, right).
/// </summary>
public class Padding
{
    public int Top { get; set; } = 0;
    public int Bottom { get; set; } = 0;
    public int Left { get; set; } = 0;
    public int Right { get; set; } = 0;

    public Padding() { }
    public Padding(int top, int bottom, int left, int right)
    {
        Top = top; Bottom = bottom; Left = left; Right = right;
    }
}
