namespace TimeTracker.UI.Menu;
/// <summary>
/// Represents a menu command to start a new shift for a selected or new project.
/// </summary>
public class StartShiftCommand(IShiftService shiftService, IProjectRepository projectRepo) : IMenuAction
{
    /// <inheritdoc/>
    public string Title => "Start shift";

    /// <inheritdoc/>
    public async Task ExecuteAsync()
    {
        // 1. Get list of projects for user to choose, plus an option to create new
        List<Project>? projects = await projectRepo.GetAllAsync();
        string projectIdentifier = string.Empty;
        GetSelectedProejct(ref projects, ref projectIdentifier);

        // 2. Ask for optional note
        string? note = string.Empty;
        GetNote(ref note);

        // 3. Call the service to start the shift
        Shift? newShift = await StartShift(projectIdentifier, note);
        if (newShift is null)
        {
            // Error already displayed in StartShift, just return
            return;
        }

        // 4. Display confirmation to the user
        DisplayProjectShift( ref newShift);
        Pause();
    }

    private static void DisplayProjectShift(ref Shift shift)
    {
        AnsiConsole.MarkupLine($"[green]Started shift[/] on [bold]{shift.Project.Name}[/] at [yellow]{shift.StartTime:HH:mm}[/].");
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    private static void GetSelectedProejct(ref List<Project>? projects, ref string projectIdentifier)
    {
        projects ??= [];

        List<SelectionItem>? choices = [.. projects.Select(static p => new SelectionItem(p.Id.ToString(), p.Name))];

        SelectionItem selectionItem = new("new", "[Create New Project]");
        choices.Insert(0, selectionItem);

        SelectionPrompt<SelectionItem>? selectionPrompt = new SelectionPrompt<SelectionItem>()
            .Title("Select a project (or create new)")
            .UseConverter(item => item.Label)
            .AddChoices(choices);

        // Prompt user to select project or create new
        SelectionItem? selection = AnsiConsole.Prompt(selectionPrompt);

        if (selection.Value == "new")
        {
            // User chose to create a new project
            string? newName = AnsiConsole.Ask<string>("Enter new project name:");
            projectIdentifier = newName;
        }
        else
        {
            // Existing project ID as string
            projectIdentifier = selection.Value;
        }
    }

    private static void GetNote(ref string note)
    {
        TextPrompt<string>? noteTextPrompt = new TextPrompt<string>("Optional note?").AllowEmpty();
        note = AnsiConsole.Prompt(noteTextPrompt);
    }

    private async Task<Shift?> StartShift(string projectIdentifier, string note)
    {
        try
        {
            return await shiftService.StartShiftAsync(projectIdentifier, note);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            Pause();
            return null;
        }
    }

    // Helper record for selection prompt
    private record SelectionItem(string Value, string Label);
}

