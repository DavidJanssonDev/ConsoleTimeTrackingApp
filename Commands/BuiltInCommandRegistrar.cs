using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Commands.Management;
using TimeTracker.Commands.Reports;
using TimeTracker.Commands.Timeshift;
using TimeTracker.Commands.System;
using TimeTracker.Plugins;
using TimeTracker.Store;
using TimeTracker.UI;

namespace TimeTracker.Commands;

/// <summary>
/// Registers all built-in commands in one place.
/// </summary>
internal static class BuiltInCommandRegistrar
{
    public static void RegisterAll(CommandRegistry registry, IShiftStore store, UiState ui)
    {
        registry.Register(new ClockInCommand(store));
        registry.Register(new ClockOutCommand(store));

        registry.Register(new ViewTodayCommand(store));
        registry.Register(new ViewWeekCommand(store));
        registry.Register(new TotalsByProjectCommand(store));

        registry.Register(new ManageEntriesCommand(store));
        registry.Register(new ManageProjectsCommand(store));

        registry.Register(new QuitCommand(store));

        FooterUpdater.Update(ui.StatusLabel, store);
    }
}
