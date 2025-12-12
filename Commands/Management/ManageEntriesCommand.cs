using System;
using TimeTracker.MenuModel;
using TimeTracker.Plugins;

public sealed class ManageEntriesCommand : ICommand
{
    public string DisplayName => "Manage Entries";
    public string Category => "Management";
    public bool OpensPage => true;

    public CommandResult Execute(ICommandContext context)
    {
        DateTime end = DateTime.Today.AddDays(1);
        DateTime start = end.AddDays(-14);

        List<Shift>? shifts = context.ShiftStore.GetShiftsForDateRange(start, end); // :contentReference[oaicite:8]{index=8}

        var page = new MenuNode("Manage Entries")
        {
            Footer = "Pick an entry to manage."
        };

        if (shifts.Count == 0)
        {
            page.Items.Add(new MenuNode("No entries in the last 14 days."));
            return new NavigateToResult(page);
        }

        foreach (Shift shift in shifts)
        {
            string label = $"Shift #{shift.Id}"; // adjust using your Shift fields
            page.Items.Add(new MenuCommand(label, new EntryActionsCommand(shift.Id)));
        }

        return new NavigateToResult(page);
    }

    private sealed class EntryActionsCommand : ICommand
    {
        private readonly long _shiftId;

        public EntryActionsCommand(long shiftId)
        {
            _shiftId = shiftId;
        }

        public string DisplayName => "Entry Actions";
        public string Category => "(internal)";
        public bool OpensPage => true;

        public CommandResult Execute(ICommandContext context)
        {
            var actions = new MenuNode($"Shift #{_shiftId}")
            {
                Footer = "Choose an action."
            };

            actions.Items.Add(new MenuCommand("Delete Entry", new DeleteEntryCommand(_shiftId)));

            return new NavigateToResult(actions);
        }
    }

    private sealed class DeleteEntryCommand : ICommand
    {
        private readonly long _shiftId;

        public DeleteEntryCommand(long shiftId)
        {
            _shiftId = shiftId;
        }

        public string DisplayName => "Delete Entry";
        public string Category => "(internal)";
        public bool OpensPage => false;

        public CommandResult Execute(ICommandContext context)
        {
            bool confirm = context.Confirm("Delete Entry", $"Delete shift #{_shiftId}?");
            if (!confirm)
            {
                return new StayResult();
            }

            context.ShiftStore.DeleteShift(_shiftId); // :contentReference[oaicite:9]{index=9}
            return new ShowMessageResult("Deleted", "Entry deleted.");
        }
    }
}
