using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.MenuModel;

namespace TimeTracker.Plugins;
/// <summary>
/// Builds a root menu from grouped commands.
/// </summary>
internal static class RootMenuBuilder
{
    public static MenuNode BuildFromCommands(CommandRegistry registry)
    {
        MenuNode root = new("Root");

        foreach (string category in registry.GetCategories())
        {
            MenuNode categoryNode = new(category);

            foreach (ICommand cmd in registry.GetByCategory(category))
            {
                if (cmd.CanHaveSubmenu)
                    categoryNode.Items.Add(new MenuNode(cmd.DisplayName));
                else
                    categoryNode.Items.Add(new MenuCommand(cmd.DisplayName, cmd));
            }

            root.Items.Add(categoryNode);
        }

        return root;
    }
}