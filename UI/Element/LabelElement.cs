using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.Interfaces;

namespace TimeTracker.UI.Element;

public sealed class LabelElement(string title) : IMenuElement
{
    public string Title { get; } = title;
}
