using Terminal.Gui;
namespace TimeTracker.UI.Menu.Comand;

public class StartShiftCommand : IMenuAction
{
    public string Title => "Start Shift";


    public async Task ExecuteAsync(MenuSystem menuSystem)
    {
        MessageBox.Query("Start Shift", "Shift started!", "OK");
        await Task.Delay(500);
        menuSystem.Back();
    }
}
