using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.MenuModel.Forms;

public sealed class FormField(string key, string label, string defaultValue = "")
{
    public string Key { get; } = key;
    public string Label { get; } = label;
    public string DefaultValue { get; } = defaultValue;
}
