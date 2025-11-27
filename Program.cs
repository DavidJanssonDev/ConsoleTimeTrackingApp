using System;
using System.Globalization;
using System.Threading.Tasks;
using ConsoleTimeTrackingApp.Data;
using ConsoleTimeTrackingApp.Models;

internal static class Program
{
    /// <summary>
    /// Entry point wrapper for async Main in older .NET Framework style projects.
    /// </summary>
    public static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Main application loop.
    /// Initializes DB/repositories and runs menu forever until Quit.
    /// </summary>
    private static async Task MainAsync()
    {
        // DbContext is IDisposable; use "using" to clean it up when exiting.
        using var db = new Database();

        // Creates database + tables if they do not exist.
        await db.Database.EnsureCreatedAsync();

        // Repositories keep EF Core access out of UI.
        var projectRepo = new ProjectRepository(db);
        var shiftRepo = new ShiftRepository(db);

        // Infinite menu loop.
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Shift Tracker ===");
            Console.WriteLine("1) Add shift");
            Console.WriteLine("2) List shifts");
            Console.WriteLine("3) End shift (close)");
            Console.WriteLine("4) Totals by project");
            Console.WriteLine("5) Delete shift");
            Console.WriteLine("0) Quit");
            Console.Write("> ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await AddShift(projectRepo, shiftRepo);
                    break;
                case "2":
                    await ListShifts(shiftRepo);
                    break;
                case "3":
                    await EndShift(shiftRepo);
                    break;
                case "4":
                    await ShowTotals(shiftRepo);
                    break;
                case "5":
                    await DeleteShift(shiftRepo);
                    break;
                case "0":
                    return; // exit app
                default:
                    Console.WriteLine("Unknown option.");
                    Pause();
                    break;
            }
        }
    }

    /// <summary>
    /// Prompts user for data and creates a new shift.
    /// </summary>
    private static async Task AddShift(ProjectRepository projectRepo, ShiftRepository shiftRepo)
    {
        Console.Clear();
        Console.WriteLine("=== Add Shift ===");

        // Ensure the user selects a valid project (or creates one).
        var project = await ChooseOrCreateProject(projectRepo);

        // Read start time with validation loop.
        var start = ReadDateTime("Start time (yyyy-MM-dd HH:mm): ");

        // End time is optional to allow open shifts.
        Console.Write("End time (blank = open shift): ");
        var endRaw = Console.ReadLine();
        DateTimeOffset? end = string.IsNullOrWhiteSpace(endRaw)
            ? null
            : ParseDateTime(endRaw);

        Console.Write("Note (optional): ");
        var note = Console.ReadLine();

        // Create shift entity.
        var shift = new Shift
        {
            ProjectId = project.Id, // FK to project
            StartTime = start,
            EndTime = end,
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim()
        };

        // Insert into DB.
        await shiftRepo.AddAsync(shift);

        Console.WriteLine("Saved!");
        Pause();
    }

    /// <summary>
    /// Lets user pick an existing project or create a new one.
    /// </summary>
    private static async Task<Project> ChooseOrCreateProject(ProjectRepository projectRepo)
    {
        var projects = await projectRepo.GetAllAsync();

        // If there are existing projects, show selection list.
        if (projects.Count > 0)
        {
            Console.WriteLine("Pick a project:");
            for (int i = 0; i < projects.Count; i++)
                Console.WriteLine($"{i + 1}) {projects[i].Name}");

            Console.WriteLine("N) New project");
            Console.Write("> ");

            var pick = Console.ReadLine();

            // User wants new project.
            if (string.Equals(pick, "N", StringComparison.OrdinalIgnoreCase))
                return await CreateNewProject(projectRepo);

            // Validate numeric choice.
            if (int.TryParse(pick, out int index) &&
                index >= 1 && index <= projects.Count)
            {
                return projects[index - 1];
            }

            Console.WriteLine("Invalid choice, creating new project.");
        }

        // If no projects exist OR invalid choice => create new.
        return await CreateNewProject(projectRepo);
    }

    /// <summary>
    /// Prompts user for a new project name and creates it.
    /// </summary>
    private static async Task<Project> CreateNewProject(ProjectRepository projectRepo)
    {
        Console.Write("New project name: ");
        var name = Console.ReadLine() ?? "";
        return await projectRepo.CreateAsync(name);
    }

    /// <summary>
    /// Lists all shifts stored in the database.
    /// </summary>
    private static async Task ListShifts(ShiftRepository shiftRepo)
    {
        Console.Clear();
        Console.WriteLine("=== All Shifts ===");

        List<Shift> shifts = await shiftRepo.GetAllAsync();

        if (shifts.Count == 0)
        {
            Console.WriteLine("No shifts logged yet.");
            Pause();
            return;
        }

        foreach (var s in shifts)
        {
            var endText = s.EndTime?.ToString("g") ?? "OPEN";

            Console.WriteLine(
                $"{s.Id} | {s.Project.Name} | {s.StartTime:g} -> {endText} | {s.Duration}"
            );
        }

        Pause();
    }

    /// <summary>
    /// Ends a shift by Id, setting EndTime to "now".
    /// </summary>
    private static async Task EndShift(ShiftRepository shiftRepo)
    {
        Console.Clear();
        Console.WriteLine("=== End Shift ===");

        Console.Write("Shift Id: ");
        var id = long.Parse(Console.ReadLine() ?? "0");

        await shiftRepo.EndAsync(id, DateTimeOffset.Now);

        Console.WriteLine("Shift closed.");
        Pause();
    }

    /// <summary>
    /// Shows summed duration per project.
    /// </summary>
    private static async Task ShowTotals(ShiftRepository shiftRepo)
    {
        Console.Clear();
        Console.WriteLine("=== Totals By Project ===");

        var totals = await shiftRepo.TotalsByProjectAsync();

        if (totals.Count == 0)
        {
            Console.WriteLine("No closed shifts yet.");
            Pause();
            return;
        }

        foreach (var kv in totals)
            Console.WriteLine($"{kv.Key}: {kv.Value.TotalHours:F2} hours");

        Pause();
    }

    /// <summary>
    /// Deletes a shift by Id.
    /// </summary>
    private static async Task DeleteShift(ShiftRepository shiftRepo)
    {
        Console.Clear();
        Console.WriteLine("=== Delete Shift ===");

        Console.Write("Shift Id: ");
        var id = long.Parse(Console.ReadLine() ?? "0");

        await shiftRepo.DeleteAsync(id);

        Console.WriteLine("Deleted.");
        Pause();
    }

    /// <summary>
    /// Reads a DateTimeOffset using a strict format and keeps asking until valid.
    /// </summary>
    private static DateTimeOffset ReadDateTime(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var raw = Console.ReadLine();

            try
            {
                return ParseDateTime(raw ?? "");
            }
            catch
            {
                Console.WriteLine("Invalid format. Try again.");
            }
        }
    }

    /// <summary>
    /// Parses user input in "yyyy-MM-dd HH:mm" format into DateTimeOffset.
    /// </summary>
    private static DateTimeOffset ParseDateTime(string raw)
    {
        return DateTimeOffset.ParseExact(
            raw.Trim(),
            "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture
        );
    }

    /// <summary>
    /// Simple "press any key to continue" helper.
    /// </summary>
    private static void Pause()
    {
        Console.WriteLine();
        Console.Write("Press any key...");
        Console.ReadKey();
    }
}
