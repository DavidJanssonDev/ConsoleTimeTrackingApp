using ConsoleTimeTrackingApp.Data;
using ConsoleTimeTrackingApp.Models;
using Spectre.Console;

namespace ConsoleTimeTrackingApp.UI
{


    internal sealed class MenuSystem
    {
        private readonly ProjectRepository _projects;
        private readonly ShiftRepository _shifts;

        private bool _running = true;
        private readonly List<MenuItem> _items;


        public MenuSystem(ProjectRepository projects, ShiftRepository shifts)
        {
            _projects = projects;
            _shifts = shifts;

            _items =
            [
                new("Start shift", StartShiftAsync),
                new("End shift", EndShiftAsync),
                new("View shifts", ViewShiftsAsync),
                new("Totals by project", TotalsByProjectAsync),
                new("Delete shift", DeleteShiftAsync),
                new("Quit", QuitAsync)
            ];
        }
        #region General Methods
        public async Task RunAsync()
        {
            while (_running)
            {
                RenderHeader();

                MenuItem selected = PromptForItem();
                await selected.Action();

            }
        }

        private MenuItem PromptForItem()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<MenuItem>()
                    .Title("Choose an Action")
                    .PageSize(10)
                    .UseConverter(i => i.Title)
                    .AddChoices(_items)
            );
        }

        #endregion

        #region Actions
        private async Task StartShiftAsync()
        {
            List<Project> allProjects = await _projects.GetAllAsync();

            List<ProjectChoice>? choices = [.. allProjects
                .Select(project => new ProjectChoice(project.Id, project.Name))
                .Prepend(new ProjectChoice(null, "+ Create new project"))];

            ProjectChoice? choice = AnsiConsole.Prompt(
                new SelectionPrompt<ProjectChoice>()
                    .Title("Select a Project")
                    .PageSize(10)
                    .UseConverter(projectChoice => projectChoice.IsNew ? $"[green]{projectChoice.Label}[/]" : projectChoice.Label)
                    .AddChoices(choices)
            );

            Project selectedProject;

            if (choice.IsNew)
            {
                string? name = AnsiConsole.Ask<string>("New Project name?");
                selectedProject = await _projects.CreateAsync(name); // create-or-return-existing :contentReference[oaicite:5]{index=5}
            }
            else
            {
                selectedProject = allProjects.First(project => project.Id == choice.ProjectId);
            }

            string? note = AnsiConsole.Prompt(
                new TextPrompt<string>("Optional note?")
                    .AllowEmpty()
            );

            Shift shift = new()
            {
                ProjectId = selectedProject.Id,
                StartTime = DateTimeOffset.Now,
                EndTime = null,
                Note = string.IsNullOrWhiteSpace(note) ? null : note
            }; // fields match Shift.cs :contentReference[oaicite:6]{index=6}
            
            await _shifts.AddAsync(shift); // :contentReference[oaicite:7]{index=7}
            
            AnsiConsole.MarkupLine($"[green]Started shift[/] on [bold]{selectedProject.Name}[/] at [yellow]{shift.StartTime:HH:mm}[/].");

            Pause();
        }

        private async Task EndShiftAsync()
        {
            List<Shift>? allShifts = await _shifts.GetAllAsync();
            List<Shift>? open = [.. allShifts.Where(shift => shift.EndTime is null)];

            if (open.Count == 0) 
            {
                AnsiConsole.Markup("[yellow]No active shifts.[/]");
                Pause();
                return;
            }

            Shift? selected = AnsiConsole.Prompt(
                new SelectionPrompt<Shift>()
                   .Title("Select shift to end")
                   .PageSize(10)
                   .UseConverter(s =>
                       $"{s.Project.Name} | {s.StartTime:yyyy-MM-dd HH:mm} (open)")
                   .AddChoices(open)
            );

            await _shifts.EndAsync(selected.Id, DateTimeOffset.Now);

            AnsiConsole.Markup("[green]Shift ended[/]");
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
            var totals = await _shifts.TotalsByProjectAsync(); // closed shifts only 
            ShiftViews.RenderTotalsTable(totals);
            Pause();
        }

        private async Task DeleteShiftAsync()
        {
            var all = await _shifts.GetAllAsync();

            if (all.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]NO shifts to delete.[/]");
                Pause();
                return;
            }

            Shift? selected = AnsiConsole.Prompt(
                new SelectionPrompt<Shift>()
                    .Title("Selet shfit to delete")
                    .PageSize(10)
                    .UseConverter(shift =>
                        $"{shift.Project.Name} | {shift.StartTime:yyyy-MM-dd HH:mm} → " +
                        $"{(shift.EndTime is null ? "open" : shift.EndTime.Value.ToString("HH:mm"))}")
                    .AddChoices(all)
            );

            if (AnsiConsole.Confirm("Really delete this shift?"))
            {
                await _shifts.DeleteAsync(selected.Id);
                AnsiConsole.MarkupLine("[green]Deleted[/]");
            }

            Pause();
        }

        private Task QuitAsync()
        {
            _running = false;
            return Task.CompletedTask;
        }

        #endregion


        // --------------------------
        // UI helpers + support types
        // --------------------------
        #region UI Helpers
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
        #endregion

        #region Support types
        private sealed record MenuItem(string Title, Func<Task> Action);

        private sealed record ProjectChoice(int? ProjectId, string Label)
        {
            public bool IsNew => ProjectId is null;
        }
        #endregion
    }

}