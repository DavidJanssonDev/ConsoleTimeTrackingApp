using System;
using System.Collections.Generic;
using System.Text;
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
        registry.Register(new ClockInCommand());
        registry.Register(new ClockOutCommand());

        registry.Register(new ViewTodayCommand());
        registry.Register(new ViewWeekCommand());
        registry.Register(new TotalsByProjectCommand());

        registry.Register(new ManageEntriesCommand());
        registry.Register(new ManageProjectsCommand());

        FooterUpdater.Update(ui.StatusLabel, store);
    }
}
