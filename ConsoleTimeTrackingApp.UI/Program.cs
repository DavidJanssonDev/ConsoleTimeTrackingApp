using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Application.Services;
using TimeTracker.Infrastructre.Persistence;
using TimeTracker.UI.Menu;


namespace TimeTracker.UI
{
    internal class Program
    {
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
            // Add additional IMenuAction implementations here
            services.AddTransient<MenuSystem>();


            var provider = services.BuildServiceProvider();
            var db = provider.GetRequiredService<TimeTrackerDbContext>();
            db.Database.EnsureCreated();


            var menu = provider.GetRequiredService<MenuSystem>();
            await menu.RunAsync();
            Console.ReadKey(true);
        }
    }
}