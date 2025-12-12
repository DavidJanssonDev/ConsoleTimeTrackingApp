using TimeTracker.MenuModel;
using TimeTracker.MenuModel.Forms;
using TimeTracker.Plugins;

public sealed class ManageProjectsCommand : ICommand
{
    public string DisplayName => "Manage Projects";
    public string Category => "Management";
    public bool OpensPage => true;

    public CommandResult Execute(ICommandContext context)
    {
        var page = new MenuNode("Manage Projects")
        {
            Footer = "Select an action."
        };

        page.Items.Add(new MenuCommand("Create Project", new CreateProjectCommand()));

        var projects = context.ShiftStore.GetAllProjects(); // :contentReference[oaicite:5]{index=5}
        foreach (var p in projects)
        {
            // Adjust formatting based on your Project entity fields
            page.Items.Add(new MenuNode($"Project #{p.Id}: {p.Name}"));
        }

        return new NavigateToResult(page);
    }

    private sealed class CreateProjectCommand : ICommand
    {
        public string DisplayName => "Create Project";
        public string Category => "(internal)";
        public bool OpensPage => true;

        public CommandResult Execute(ICommandContext context)
        {
            var form = new MenuForm(
                "Create Project",
                values =>
                {
                    string name = values.TryGetValue("name", out var v) ? v.Trim() : "";
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        return new ShowMessageResult("Missing name", "Please enter a project name.");
                    }

                    context.ShiftStore.EnsureProject(name); // :contentReference[oaicite:6]{index=6}
                    return new BackResult();
                });

            form.Fields.Add(new FormField("name", "Project name"));
            form.Footer = "Enter a name and submit.";

            return new NavigateToResult(form);
        }
    }
}
