using System;
using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.MenuModel.Forms;

namespace TimeTracker.UI;

internal static class FormRenderer
{
    public static void ShowForm(MenuForm form, UiState ui)
    {
        ui.MainWindow.Title = form.Title;

        // Create a simple "form page" layout inside the same window:
        // Clear currnt view content and rebuild
        ui.MainWindow.RemoveAll();

        Label title = new(form.Title)
        {
            X = 1,
            Y = 0
        };

        ui.MainWindow.Add(title);

        Dictionary<string, TextField>? fields = new (StringComparer.Ordinal);

        int y = 2;
        foreach(FormField field in form.Fields)
        {
            Label label = new(field.Label)
            {
                X = 1,
                Y = y,
            };

            TextField input = new(field.DeafultValue)
            {
                X = 1,
                Y = y + 1,
                Width = Dim.Fill(2)
            };

            ui.MainWindow.Add(label, input);
            fields[field.Key] = input;

            y += 3;
        }

        Button submit = new("Submit")
        {
            X = 1,
            Y = y
        };

        ui.MainWindow.Add(submit);

        if (!string.IsNullOrWhiteSpace(form.Footer))
            ui.StatusLabel.Text = form.Footer;

        // Hook submit - actual navigatipn is handeled by MenuNavigation via UiState callback
        submit.Clicked += () =>
        {
            Dictionary<string, string>? values = new(StringComparer.Ordinal);

            foreach ((string key, TextField tf) in fields)
                values[key] = tf.Text?.ToString() ?? string.Empty;

            ui.OnFormSubmitted?.Invoke(form, values);
        };

    }
}
