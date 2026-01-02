using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.UI.MenuSystem.Interface;

namespace TimeTracker.UI.MenuSystem.Actions;

public sealed class NavigateAction(MenuPage target) : IMenuAction
{
    public void Execute(NavigationService nav) => nav.Push(target);
}
