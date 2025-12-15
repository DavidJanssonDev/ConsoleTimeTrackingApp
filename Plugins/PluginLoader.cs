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
    public static void LoadFromFolder(CommandRegistry registry, string folder)
    {
        if (!Directory.Exists(folder))
        {
            return;
        }

        string[] dlls = Directory.GetFiles(folder, "*.dll");
        Type commandType = typeof(ICommand);

        for (int i = 0; i < dlls.Length; i++)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(dlls[i]);
            }
            catch
            {
                continue;
            }

            Type[] types;
            try
            {
                types = assembly.GetExportedTypes();
            }
            catch
            {
                continue;
            }

            for (int typeIndex = 0; typeIndex < types.Length; typeIndex++)
            {
                Type type = types[typeIndex];
                bool isConcreteCommand = commandType.IsAssignableFrom(type) && !type.IsAbstract;

                if (!isConcreteCommand)
                {
                    continue;
                }

                ConstructorInfo? ctorDefault = type.GetConstructor(Type.EmptyTypes);
                if (ctorDefault is null)
                {
                    continue; // keep it simple: plugin commands must have parameterless ctor
                }

                ICommand instance;
                try
                {
                    instance = (ICommand)ctorDefault.Invoke(null);
                }
                catch
                {
                    continue;
                }

                registry.Register(instance);
            }
        }
    }
}