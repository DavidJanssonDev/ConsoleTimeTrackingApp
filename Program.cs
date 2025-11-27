using ConsoleTimeTrackingApp.Data;
using ConsoleTimeTrackingApp.Models;
using ConsoleTimeTrackingApp.UI;
using Spectre.Console;
using System;
using System.Globalization;
using System.Threading.Tasks;


namespace Main
{
    public static class Program
    {
        public static void Main()
        {
            using Database db = new();
            db.Database.EnsureCreated();

            ProjectRepository projectRepo = new(db);
            ShiftRepository shiftRepo = new(db);

            var menu = new MenuSystem(projectRepo, shiftRepo);
        }
    }
}