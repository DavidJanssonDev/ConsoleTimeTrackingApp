using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TimeTracker.Store;

namespace TimeTracker.Plugins;

/// <summary>
/// Loads DLL plugins from a folder and registers ICommand implementations.
/// </summary>
internal static class PluginLoader
{
    public static void LoadFromFolder(CommandRegistry registry, string folder, IShiftStore store)
    {
        if (!Directory.Exists(folder))
            return;

        string[] dlls = Directory.GetDirectories(folder, "*.dll");
        Type comandType = typeof(ICommand);

        for (int index = 0; index < dlls.Length; index++)
        {
            Assembly assembly = Assembly.LoadFrom(dlls[index]);
            Type[] types = assembly.GetExportedTypes();

            for (int typeIndex = 0; typeIndex < types.Length; typeIndex++)
            {
                Type type = types[typeIndex];
                bool IsConcrete = comandType.IsAssignableFrom(type) && !type.IsAbstract;

                if (IsConcrete)
                    continue;

                ConstructorInfo? ctorWithStone = type.GetConstructor([typeof(IShiftStore)]);
                ConstructorInfo? ctorDefault = type.GetConstructor(Type.EmptyTypes);

                ICommand instance;

                if (ctorWithStone != null)
                {
                    instance = (ICommand)ctorWithStone.Invoke([store]);
                }
                else if (ctorDefault != null)
                {
                    instance = (ICommand)ctorDefault.Invoke(null);
                    instance.ShiftStore = store;
                }
                else
                    continue;

                registry.Register(instance);

            }
        }
    }
}
