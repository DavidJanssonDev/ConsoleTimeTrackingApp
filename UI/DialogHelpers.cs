using System;
using System.Collections.Generic;
using Terminal.Gui;

namespace TimeTracker.UI;

/// <summary>
/// Small dialog helpers for selection and text input.
/// </summary>
internal static class DialogHelpers
{
    /// <summary>
    /// Shows a selectable list dialog with a built-in Cancel/Back option.
    /// 
    /// Returns:
    /// - index >= 0 when a real item is chosen
    /// - -1 if user cancels (Cancel button / Esc / Backspace)
    /// </summary>
    public static int ListDialog(List<string> items, string title)
    {
        // Calculate a reasonable dialog height based on item count.
        int height = Math.Min(20, items.Count + 6);

        Dialog dialog = new Dialog(title, width: 60, height: height);

        ListView listView = new ListView(items)
        {
            X = 1,
            Y = 1,
            Width = Dim.Fill() - 2,
            Height = Dim.Fill() - 3,
            AllowsMarking = false
        };

        // Default selection if user presses Enter.
        int selectedIndex = 0;

        // Track cancel state.
        bool cancelled = false;

        // When user presses Enter on an item, close dialog and keep index.
        listView.OpenSelectedItem += (ListViewItemEventArgs args) =>
        {
            selectedIndex = args.Item;
            Application.RequestStop(dialog);
        };

        // Cancel button = back action.
        Button cancelButton = new Button("Back")
        {
            // Not default; Enter still selects list items.
            IsDefault = false
        };

        cancelButton.Clicked += () =>
        {
            cancelled = true;
            Application.RequestStop(dialog);
        };

        // Add views to dialog.
        dialog.Add(listView);
        dialog.AddButton(cancelButton);

        // Allow Esc / Backspace to cancel.
        dialog.KeyPress += (View.KeyEventEventArgs args) =>
        {
            Key key = args.KeyEvent.Key;
            bool isBackKey = key == Key.Esc || key == Key.Backspace;

            if (!isBackKey)
            {
                return;
            }

            cancelled = true;
            Application.RequestStop(dialog);
            args.Handled = true;
        };

        Application.Run(dialog);

        // If cancelled, return -1.
        if (cancelled)
        {
            return -1;
        }

        return selectedIndex;
    }

    public static string PromptText(string prompt, bool allowEmpty)
    {
        TextField field = new TextField(string.Empty)
        {
            X = 1,
            Y = 1,
            Width = Dim.Fill() - 2
        };

        Dialog dialog = new Dialog(prompt, width: 60, height: 6);
        Button okButton = new Button("OK") { IsDefault = true };
        Button cancelButton = new Button("Cancel");

        string result = string.Empty;
        bool cancelled = false;

        okButton.Clicked += () =>
        {
            string raw = field.Text.ToString() ?? string.Empty;

            if (!allowEmpty && string.IsNullOrWhiteSpace(raw))
            {
                MessageBox.ErrorQuery("Validation", "Value required.", "OK");
                return;
            }

            result = raw.Trim();
            Application.RequestStop(dialog);
        };

        cancelButton.Clicked += () =>
        {
            cancelled = true;
            Application.RequestStop(dialog);
        };

        dialog.Add(field);
        dialog.AddButton(okButton);
        dialog.AddButton(cancelButton);

        Application.Run(dialog);

        if (cancelled)
        {
            throw new OperationCanceledException("Prompt cancelled.");
        }

        return result;
    }
}
