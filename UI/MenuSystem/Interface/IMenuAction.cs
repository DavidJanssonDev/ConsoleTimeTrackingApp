using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.UI.MenuSystem.Interface;

public interface IMenuAction
{
   void Execute(NavigationService nav);
}

