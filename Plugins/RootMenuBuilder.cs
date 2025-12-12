using System.Collections.Generic;
using TimeTracker.MenuModel;

namespace TimeTracker.Plugins;

/// <summary>
/// Builds a root menu from grouped commands.
/// </summary>
internal static class RootMenuBuilder
{
    public static MenuNode BuildFromCommands(CommandRegistry registry)
    {
        MenuNode root = new MenuNode("Main Menu")
        {
            Footer = "Shift Tracker"
        };

        Dictionary<string, List<ICommand>> grouped = registry.GetCommandsGroupedByCategory();

        foreach (KeyValuePair<string, List<ICommand>> kv in grouped)
        {
            string category = kv.Key;
            List<ICommand> commands = kv.Value;

            MenuNode categoryMenu = new MenuNode(category);

            for (int i = 0; i < commands.Count; i++)
            {
                ICommand cmd = commands[i];

                // Leaf command node:
                categoryMenu.Items.Add(new MenuCommand(cmd.DisplayName, cmd));
            }

            // Category is a submenu node:
            root.Items.Add(categoryMenu);
        }

        return root;
    }
}
