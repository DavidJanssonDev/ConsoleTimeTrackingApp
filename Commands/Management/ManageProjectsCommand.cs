using TimeTracker.Domain.Entities;
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

        List<Project> projects = context.ShiftStore.GetAllProjects();
        foreach (Project p in projects)
        {
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
            CommandResult func(IReadOnlyDictionary<string, string> values)
            {
                string name = values.TryGetValue("name", out var v) ? v.Trim() : "";
                if (string.IsNullOrWhiteSpace(name))
                {
                    return new ShowMessageResult("Missing name", "Please enter a project name.");
                }

                context.ShiftStore.EnsureProject(name);
                return new BackResult();
            }

            MenuForm form = new("Create Project", func);
            form.Fields.Add(new FormField("name", "Project name"));
            form.Footer = "Enter a name and submit.";

            return new NavigateToResult(form);
        }
    }
}
