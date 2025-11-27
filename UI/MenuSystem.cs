using ConsoleTimeTrackingApp.Data;
using ConsoleTimeTrackingApp.Models;
using Spectre.Console;

namespace ConsoleTimeTrackingApp.UI
{


    internal class MenuSystem
    {
        private readonly ProjectRepository _projects;
        private readonly ShiftRepository _shifts;

        private bool _running = true;

        public MenuSystem(ProjectRepository projects, ShiftRepository shifts)
        {
            _projects = projects;
            _shifts = shifts;
        }

        public async Task RunAsync()
        {
            await RunLoopAsync();
        }

        // 1) Loop is controlled by _running
        private async Task RunLoopAsync()
        {
            while (_running)
            {
                RenderHeader();

                var choice = PromptForChoice();

                var action = GetAction(choice);

                await action();
            }
        }

        // 2) Input selection only
        private static MenuChoice PromptForChoice()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<MenuChoice>()
                    .Title("Choose an action")
                    .PageSize(10)
                    .AddChoices(Enum.GetValues<MenuChoice>()));
        }

        // 3) Choice -> function mapping only (returns the function)
        private Func<Task> GetAction(MenuChoice choice)
        {
            return choice switch
            {
                MenuChoice.StartShift => StartShiftAsync,
                MenuChoice.EndShift => EndShiftAsync,
                MenuChoice.ViewShifts => ViewShiftsAsync,
                MenuChoice.TotalsByProject => TotalsByProjectAsync,
                MenuChoice.DeleteShift => DeleteShiftAsync,
                MenuChoice.Quit => QuitAsync,
                _ => UnknownAsync
            };
        }

        // --------------------------
        // ACTIONS
        // --------------------------

        private async Task StartShiftAsync()
        {
            var allProjects = await _projects.GetAllAsync();

            var choices = allProjects
                .Select(p => new ProjectChoice(p.Id, p.Name))
                .Prepend(new ProjectChoice(null, "+ Create new project"))
                .ToList();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<ProjectChoice>()
                    .Title("Select a project")
                    .PageSize(10)
                    .UseConverter(c => c.IsNew ? $"[green]{c.Label}[/]" : c.Label)
                    .AddChoices(choices)
            );

            Project selectedProject;

            if (choice.IsNew)
            {
                var name = AnsiConsole.Ask<string>("New Project name?");
                selectedProject = await _projects.CreateAsync(name);
            }
            else
            {
                selectedProject = allProjects.First(p => p.Id == choice.ProjectId);
            }

            var note = AnsiConsole.Prompt(
                new TextPrompt<string>("Optional note?")
                    .AllowEmpty()
            );

            var shift = new Shift
            {
                ProjectId = selectedProject.Id,
                StartTime = DateTimeOffset.Now,
                EndTime = null,
                Note = string.IsNullOrWhiteSpace(note) ? null : note
            };

            await _shifts.AddAsync(shift);

            AnsiConsole.MarkupLine(
                $"[green]Started shift[/] on [bold]{selectedProject.Name}[/] at [yellow]{shift.StartTime:HH:mm}[/]."
            );

            Pause();
        }

        private async Task EndShiftAsync()
        {
            var allShifts = await _shifts.GetAllAsync();
            var open = allShifts.Where(s => s.EndTime is null).ToList();

            if (open.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No active shifts.[/]");
                Pause();
                return;
            }

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<Shift>()
                    .Title("Select shift to end")
                    .PageSize(10)
                    .UseConverter(s =>
                        $"{s.Project.Name} | {s.StartTime:yyyy-MM-dd HH:mm} (open)")
                    .AddChoices(open)
            );

            await _shifts.EndAsync(selected.Id, DateTimeOffset.Now);

            AnsiConsole.MarkupLine("[green]Shift ended.[/]");
            Pause();
        }

        private async Task ViewShiftsAsync()
        {
            var all = await _shifts.GetAllAsync();
            ShiftViews.RenderShiftsTable(all);
            Pause();
        }

        private async Task TotalsByProjectAsync()
        {
            var totals = await _shifts.TotalsByProjectAsync();
            ShiftViews.RenderTotalsTable(totals);
            Pause();
        }

        private async Task DeleteShiftAsync()
        {
            var all = await _shifts.GetAllAsync();

            if (all.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No shifts to delete.[/]");
                Pause();
                return;
            }

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<Shift>()
                    .Title("Select shift to delete")
                    .PageSize(10)
                    .UseConverter(s =>
                        $"{s.Project.Name} | {s.StartTime:yyyy-MM-dd HH:mm} → " +
                        $"{(s.EndTime is null ? "open" : s.EndTime.Value.ToString("HH:mm"))}")
                    .AddChoices(all)
            );

            if (AnsiConsole.Confirm("Really delete this shift?"))
            {
                await _shifts.DeleteAsync(selected.Id);
                AnsiConsole.MarkupLine("[green]Deleted.[/]");
            }

            Pause();
        }

        private Task QuitAsync()
        {
            _running = false;
            return Task.CompletedTask;
        }

        private static Task UnknownAsync()
        {
            return Task.CompletedTask;
        }

        // --------------------------
        // UI HELPERS
        // --------------------------

        private static void RenderHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Shift Tracker").Color(Color.Cyan));
            AnsiConsole.WriteLine();
        }

        private static void Pause()
        {
            AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
            Console.ReadKey(true);
        }

        // --------------------------
        // SUPPORT TYPES
        // --------------------------

        private sealed record ProjectChoice(int? ProjectId, string Label)
        {
            public bool IsNew => ProjectId is null;
        }

        private enum MenuChoice
        {
            StartShift,
            EndShift,
            ViewShifts,
            TotalsByProject,
            DeleteShift,
            Quit
        }

    }

}