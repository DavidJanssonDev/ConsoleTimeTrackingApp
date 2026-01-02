using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.MenuSystem.Interface;

namespace TimeTracker.UI.MenuSystem.Actions;

public sealed class BackAction : IMenuAction
{
    public void Execute(NavigationService nav) => nav.Pop();
}
