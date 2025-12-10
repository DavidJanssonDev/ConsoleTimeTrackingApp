using Terminal.Gui;
using TimeTracker.ApplicationCode.Services;
using TimeTracker.Commands;
using TimeTracker.Infrastructre.Persistence;
using TimeTracker.MenuModel;
using TimeTracker.Plugins;
using TimeTracker.Store;
using TimeTracker.UI;

namespace TimeTracker;

internal static class Program
{
    private static void Main()
    {
        Application.Init();

        // Create Terminal.Gui UI.
        UiState ui = UiFactory.CreateUi();

        // Build DbContext (Sqlite) and ensure DB exists.
        TimeTrackerDbContext dbContext = CreateDbContext("timetracker.db");

        dbContext.Database.EnsureCreated();

        // Your existing repositories + service.
        IProjectRepository projectRepository = new ProjectRepository(dbContext);
        IShiftRepository shiftRepository = new ShiftRepository(dbContext);
        IShiftService shiftService = new ShiftService(projectRepository, shiftRepository);

        // Adapter store used by commands + plugins.
        IShiftStore shiftStore = new EfShiftStore(projectRepository, shiftRepository, shiftService);

        // Register built-in + plugin commands.
        CommandRegistry registry = new();
        BuiltInCommandRegistrar.RegisterAll(registry, shiftStore, ui);

        PluginLoader.LoadFromFolder(registry, "plugins", shiftStore);

        // Build root menu from categories.
        MenuNode rootMenu = RootMenuBuilder.BuildFromCommands(registry);

        // Navigation stack + handlers.
        Stack<MenuNode> stack = MenuNavigation.CreateStack(rootMenu);
        MenuNavigation.WireHandlers(ui, stack);

        // Show root and run.
        MenuRenderer.ShowMenu(stack.Peek(), ui);

        Application.Run(ui.MainWindow);
        Application.Shutdown();
    }

    /// <summary>
    /// Creates Sqlite-backed DbContext and creates schema if missing.
    /// </summary>
    private static TimeTrackerDbContext CreateDbContext(string databasePath)
    {
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlite($"Data Source={databasePath}");

        TimeTrackerDbContext context = new TimeTrackerDbContext(optionsBuilder.Options);
        context.Database.EnsureCreated();
        return context;
    }
}