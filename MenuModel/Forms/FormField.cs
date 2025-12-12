using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.MenuModel.Forms;

public sealed class FormField
{
    public string Key { get; }
    public string Label { get; }
    public string DeafultValue { get; }

    public FormField(string key, string label, string deafultValue="")
    {
        Key = key;
        Label = label;
        DeafultValue = deafultValue;
    }
}
