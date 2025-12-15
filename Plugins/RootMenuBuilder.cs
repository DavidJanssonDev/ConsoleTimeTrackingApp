using TimeTracker.MenuModel;

namespace TimeTracker.Plugins;

internal static class RootMenuBuilder
{
    public static MenuNode BuildFromCommands(CommandRegistry registry)
    {
        MenuNode root = new("Main Menu");
       

        Dictionary<string, List<ICommand>> grouped = registry.GetCommandsGroupedByCategory();
        
        foreach (var (category, commands) in grouped)
        {
            MenuNode categoryMenu = new(category);

            foreach (ICommand cmd in commands)
            {
                categoryMenu.Items.Add(new MenuCommand(cmd.DisplayName, cmd));
            }

            root.Items.Add(categoryMenu);
        }

        return root;
    }
}
