using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Application.Services;
using TimeTracker.Infrastructre.Persistence;
using TimeTracker.UI.Menu;


namespace TimeTracker.UI;

/// <summary>
/// Entry point for the TimeTracker UI application.
/// Configures services, ensures database creation, and runs the main menu system.
/// </summary>
public class Program
{
    /// <summary>
    /// Main method to start the application.
    /// </summary>
    public static async Task Main()
    {
        ServiceCollection services = new();
        services.AddDbContext<TimeTrackerDbContext>(options =>
            options.UseSqlite("Data Source=timetracking.db"));

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<IShiftService, ShiftService>();

        services.AddTransient<IMenuAction, StartShiftCommand>();
        services.AddTransient<IMenuAction, QuitCommand>();
        services.AddTransient<MenuSystem>();

        var provider = services.BuildServiceProvider();
        var db = provider.GetRequiredService<TimeTrackerDbContext>();
        db.Database.EnsureCreated();

        var menu = provider.GetRequiredService<MenuSystem>();
        await menu.RunAsync();
    }
}