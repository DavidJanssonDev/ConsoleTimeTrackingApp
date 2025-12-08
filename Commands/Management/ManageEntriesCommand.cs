using System;
using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.MenuModel;
using TimeTracker.Plugins;
using TimeTracker.Store;
using MenuItem = TimeTracker.MenuModel.MenuItem;

namespace TimeTracker.Commands.Management;

/// <summary>
/// Dynamic submenu listing last 14 days of shifts.
/// Selecting a shift gives a Delete action.
/// </summary>
internal sealed class ManageEntriesCommand : ShiftCommandBase
{
    public override string DisplayName => "Manage Entries";
    public override string Category => "Management";
    public override bool CanHaveSubmenu => true;

    public ManageEntriesCommand(IShiftStore store) : base(store) { }

    protected override void ExecuteCore() { }

    public override MenuNode? BuildSubmenu()
    {
        DateTime end = DateTime.Now.Date.AddDays(1);
        DateTime start = end.AddDays(-14);

        List<Domain.Entities.Shift> shifts = ShiftStore.GetShiftsForDateRange(start, end);

        MenuNode menu = new MenuNode("Manage Entries")
        {
            Footer = "Pick a shift to delete"
        };

        if (shifts.Count == 0)
        {
            ICommand none = new InlineCommand("No shifts found", "(internal)", ShiftStore, () => { });
            menu.Items.Add(new MenuItem(none.DisplayName, none));
            return menu;
        }

        for (int i = 0; i < shifts.Count; i++)
        {
            Domain.Entities.Shift s = shifts[i];
            string line = ShiftFormatter.FormatSingleShiftLine(s);
            MenuNode actions = BuildActionsForShift(s);
            menu.Items.Add(new MenuItem(line, actions));
        }

        return menu;
    }

    private MenuNode BuildActionsForShift(Domain.Entities.Shift shift)
    {
        MenuNode actions = new MenuNode("Entry Actions")
        {
            Footer = shift.Project.Name
        };

        ICommand delete = new InlineCommand(
            "Delete Entry",
            "(internal)",
            ShiftStore,
            () =>
            {
                ShiftStore.DeleteShift(shift.Id);
                MessageBox.Query("Deleted", "Entry deleted.", "OK");
            });

        actions.Items.Add(new MenuItem(delete.DisplayName, delete));
        return actions;
    }
}
