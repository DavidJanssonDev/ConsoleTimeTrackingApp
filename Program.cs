using ConsoleTimeTrackingApp.Data;
using ConsoleTimeTrackingApp.Models;
using ConsoleTimeTrackingApp.UI;
using Spectre.Console;
using System;
using System.Globalization;
using System.Threading.Tasks;


namespace Main
{
    using ConsoleTimeTrackingApp.Data;
    using ConsoleTimeTrackingApp.UI;

    internal static class Program
    {
        private static async Task Main()
        {
            using Database db = new();
            db.Database.EnsureCreated();

            ProjectRepository projectRepo = new(db);
            ShiftRepository shiftRepo = new(db);

            MenuSystem menu = new(projectRepo, shiftRepo);
            await menu.RunAsync();
        }
    }

}