using System;
using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.MenuModel;
using TimeTracker.Plugins;
using TimeTracker.Store;

namespace TimeTracker.Commands.Reports;

/// <summary>
/// Shows shifts for today.
/// </summary>
public sealed class ViewTodayCommand : ICommand
{
    public string DisplayName => "View Today";
    public string Category => "Reports";

    public bool OpensPage => true;

    public CommandResult Execute(ICommandContext context)
    {
        DateTime today = DateTime.Today;

        List<Shift>? shifts = context.ShiftStore.GetShiftsForDate(today);

        MenuNode page = new("Today")
        {
            Footer = "Esc/Backspace to go back"
        };

        if (shifts.Count == 0)
        {
            page.Items.Add(new MenuNode("No shifts today."));
            return new NavigateToResult(page);
        }

        foreach (Shift shfit in shifts)
            page.Items.Add(new MenuNode($"Shift #{shfit.Id}"));

        return new NavigateToResult(page);
    }
}
