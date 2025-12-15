using Terminal.Gui;
using TimeTracker.ApplicationCode.Services;
using TimeTracker.Commands;
using TimeTracker.Infrastructre.Persistence;
using TimeTracker.MenuModel;
using TimeTracker.MenuModel.Interfaces;
using TimeTracker.Plugins;
using TimeTracker.Store;
using TimeTracker.UI;

namespace TimeTracker;

internal static class Program
{
    private static void Main()
    {
        Application.Init();

        // 1) UI
        UiState ui = UiFactory.CreateUi();

        // 2) Store (EF Core SQLite)
        IShiftStore shiftStore = CreateShiftStore();

        // 3) Command context (UI layer for commands)
        CommandContext context = new(shiftStore);

        // 4) Register commands (built-ins + plugins)
        CommandRegistry registry = new();
        BuiltInCommandRegistrar.RegisterAll(registry, shiftStore, ui);
        // PluginLoader.LoadFromFolder(registry, "plugins"); // optional

        // 5) Build root menu + navigation stack
        MenuNode root = RootMenuBuilder.BuildFromCommands(registry);
        Stack<IMenuElement> stack = MenuNavigation.CreateStack(root);

        // 6) Render + wire events
        Toplevel top = new();
        top.Add(ui.MainWindow);

        MenuRenderer.Show(stack.Peek(), ui, stack);
        MenuNavigation.WireHandlers(ui, stack, context);

        Application.Run(top);
        Application.Shutdown();
    }

    private static TimeTrackerDbContext CreateDbContext(string databasePath)
    {
        var options = new DbContextOptionsBuilder()
            .UseSqlite($"Data Source={databasePath}")
            .Options;

        var db = new TimeTrackerDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static IShiftStore CreateShiftStore()
    {
        // Put the sqlite file in the working directory.
        TimeTrackerDbContext db = CreateDbContext("timetracker.db");

        var projectRepo = new EfProjectRepository(db);
        var shiftRepo = new EfShiftRepository(db);
        IShiftService shiftService = new ShiftService(projectRepo, shiftRepo);

        return new EfShiftStore(projectRepo, shiftRepo, shiftService);
    }

}