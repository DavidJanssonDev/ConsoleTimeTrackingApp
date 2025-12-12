using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.Domain.Common;
using TimeTracker.MenuModel.Forms;
using TimeTracker.Plugins;
using TimeTracker.Store;
using TimeTracker.UI;

namespace TimeTracker.Commands.Timeshift;


/// <summary>
/// Starts a new shift by selecting or creating a project.
/// </summary>
public sealed class ClockInCommand : ICommand
{
    public string DisplayName => "Clock In";
    public string Category => "Shift";
    public bool OpensPage => true;


    public CommandResult Execute(ICommandContext context)
    {

        // If you want to prevent multiple active shifts:
        Shift? active = context.ShiftStore.GetActiveShift();
        if (active is not null)
            return new ShowMessageResult(
                "Active Shift",
                "You already have an active shift. Clock out first."
            );


        CommandResult func(IReadOnlyDictionary<string, string> values)
        {
            string project = values.TryGetValue("project", out var p) ? p.Trim() : "";
            string? note = values.TryGetValue("note", out var n) ? n.Trim() : "";

            if (string.IsNullOrWhiteSpace(project))
                return new ShowMessageResult("Missing project", "Please enter a project name or ID.");

            note = string.IsNullOrWhiteSpace(note) ? null : note;

            context.ShiftStore.ClockIn(project, note);

            return new BackResult();
        }

        MenuForm form = new("Clock In", func);

        form.Fields.Add(new FormField("project", "Project name"));
        form.Fields.Add(new FormField("note", "Note (optional)"));
        form.Footer = "Fill the fields and press Submit.";

        return new NavigateToResult(form);
    }
}
