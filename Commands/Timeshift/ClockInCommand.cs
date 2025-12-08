using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.Domain.Common;
using TimeTracker.Store;
using TimeTracker.UI;

namespace TimeTracker.Commands.Timeshift;


/// <summary>
/// Starts a new shift by selecting or creating a project.
/// </summary>
internal sealed class ClockInCommand : ShiftCommandBase
{
    public override string DisplayName => "Clock In";
    public override string Category => "Shift";
    public override bool CanHaveSubmenu => false;

    public ClockInCommand(IShiftStore store) : base(store) { }

    protected override void ExecuteCore()
    {
        Domain.Entities.Shift? active = ShiftStore.GetActiveShift();
        if (active != null)
        {
            throw new InvalidOperationException("Already clocked in.");
        }

        string projectNameOrId = PickProjectOrCreateNew();
        string? note = DialogHelpers.PromptText(Prompts.OptionalNote, allowEmpty: true);

        Domain.Entities.Shift shift = ShiftStore.StartShift(projectNameOrId, note);

        MessageBox.Query(
            "Clock In",
            $"Started {shift.Project.Name} at {shift.StartTime.ToLocalTime():HH:mm}",
            "OK");
    }

    private string PickProjectOrCreateNew()
    {
        List<Project> projects = ShiftStore.GetAllProjects();
        List<string> items = [];

        items.Add(ProjectOptions.CreateNew);

        for (int projectIndex = 0; projectIndex < projects.Count; projectIndex++)
        {
            Project p = projects[projectIndex];
            items.Add($"{p.Id}: {p.Name}");
        }

        items.Add(ProjectOptions.Cancel);

        int selectedIndex = DialogHelpers.ListDialog(items, Prompts.SelectProject);
        string selected = items[selectedIndex];

        if (selected == ProjectOptions.Cancel)
            throw new OperationCanceledException("Clock in cancelled.");
        
        if (selected == ProjectOptions.CreateNew)
            return DialogHelpers.PromptText(Prompts.EnterProjectName, allowEmpty: false);
           
        int colon = selected.IndexOf(':');
        if (colon > 0)
            return selected.Substring(0, colon).Trim();

        return selected;
    }
}
