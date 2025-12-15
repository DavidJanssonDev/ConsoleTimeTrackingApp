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

        // Replace window content with a form layout.
        ui.MainWindow.RemoveAll();

        var header = new Label(form.Title)
        {
            X = 1,
            Y = 0
        };
        ui.MainWindow.Add(header);

        var inputs = new Dictionary<string, TextField>(StringComparer.Ordinal);

        int y = 2;
        foreach (FormField field in form.Fields)
        {
            var label = new Label(field.Label)
            {
                X = 1,
                Y = y
            };

            var input = new TextField(field.DefaultValue)
            {
                X = 1,
                Y = y + 1,
                Width = Dim.Fill(2)
            };

            ui.MainWindow.Add(label, input);
            inputs[field.Key] = input;

            y += 3;
        }

        var submit = new Button("Submit")
        {
            X = 1,
            Y = y
        };

        ui.MainWindow.Add(submit);

        if (!string.IsNullOrWhiteSpace(form.Footer))
        {
            ui.StatusLabel.Text = form.Footer;
        }

        submit.Clicked += () =>
        {
            var values = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach ((string key, TextField field) in inputs)
            {
                values[key] = field.Text?.ToString() ?? string.Empty;
            }

            ui.OnFormSubmitted?.Invoke(form, values);
        };
    }
}
