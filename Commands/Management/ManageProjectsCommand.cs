using System.Collections.Generic;
using Terminal.Gui;
using TimeTracker.Commands.Base;
using TimeTracker.Domain.Common;
using TimeTracker.Domain.Entities;
using TimeTracker.MenuModel;
using TimeTracker.Plugins;
using TimeTracker.Store;
using TimeTracker.UI;
using MenuItem = TimeTracker.MenuModel.MenuItem;

namespace TimeTracker.Commands.Management;

/// <summary>
/// Dynamic submenu to create and list projects.
/// </summary>
internal sealed class ManageProjectsCommand : ShiftCommandBase
{
    public override string DisplayName => "Manage Projects";
    public override string Category => "Management";
    public override bool CanHaveSubmenu => true;

    public ManageProjectsCommand(IShiftStore store) : base(store) { }

    protected override void ExecuteCore() { }

    public override MenuNode? BuildSubmenu()
    {
        MenuNode menu = new MenuNode("Projects")
        {
            Footer = "Create new or view list"
        };

        ICommand create = new InlineCommand(
            "Create Project",
            "(internal)",
            ShiftStore,
            () =>
            {
                string name = DialogHelpers.PromptText(Prompts.EnterProjectName, allowEmpty: false);
                Project project = ShiftStore.EnsureProject(name);
                MessageBox.Query("Project", $"Created: {project.Name}", "OK");
            });

        menu.Items.Add(new MenuItem(create.DisplayName, create));

        List<Project> projects = ShiftStore.GetAllProjects();
        for (int i = 0; i < projects.Count; i++)
        {
            Project p = projects[i];
            string text = $"{p.Id}: {p.Name}";
            ICommand noop = new InlineCommand(text, "(internal)", ShiftStore, () => { });
            menu.Items.Add(new MenuItem(text, noop));
        }

        return menu;
    }
}
