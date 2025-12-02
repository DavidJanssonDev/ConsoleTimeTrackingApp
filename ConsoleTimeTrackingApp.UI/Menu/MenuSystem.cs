namespace TimeTracker.UI.Menu;

/// <summary>
/// Represents the main menu system for the Shift Tracker application,
/// responsible for displaying available actions and executing user selections.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MenuSystem"/> class with the specified menu actions.
/// </remarks>
/// <param name="menuActions">A collection of menu actions to display and execute.</param>
public class MenuSystem(IEnumerable<IMenuAction> menuActions)
{
    private readonly IEnumerable<IMenuAction> _menuActions = menuActions;
    private bool _running = true;

    /// <summary>
    /// Runs the menu system asynchronously, displaying the menu and executing selected actions until the user chooses to quit.
    /// </summary>
    public async Task RunAsync()
    {
        Console.WriteLine("HHHHHHHHHHHHHHHHHHHHHHHHHHHH");
        AnsiConsole.MarkupLine("[blue]Rendering Menu...[/]");
        while (_running)
        {
            RenderHeader();
            // Display menu and get selection
            //var choice = AnsiConsole.Prompt(
            //    new SelectionPrompt<IMenuAction>()
            //        .Title("[cyan]Choose an action[/]")
            //        .UseConverter(action => action.Title)
            //        .AddChoices(_menuActions)
            //);
            // Execute the selected action
            //await choice.ExecuteAsync();
            // If a Quit action is implemented as IMenuAction that sets _running = false, handle that.
            //if (choice is QuitCommand) _running = false;


            foreach (var action in _menuActions)
            {
                Console.WriteLine($"Available: {action.Title}");
            }
            Console.ReadKey(true);

        }
    }

    /// <summary>
    /// Renders the application header in the console.
    /// </summary>
    private static void RenderHeader()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Shift Tracker").Color(Color.Cyan));
        AnsiConsole.WriteLine();
    }
}
