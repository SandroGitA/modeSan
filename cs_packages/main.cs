using RAGE;
using RAGE.Elements;
using RAGE.Game;
using RAGE.Ui;
public class Main : Events.Script
{

    public static HtmlWindow openedWindow;
    public static Camera currentCamera;
    public Main() 
    {
        Events.OnPlayerReady += OnPlayerReady;
        Events.OnPlayerCreateWaypoint += OnPlayerCreateWaypoint;
        Events.Add("closeBrowser", OnCloseBrowserMessage);

        Events.Add("SERVER:CLIENT::AuthReady", OnPlayerConnected);

    }

    public void OnPlayerReady()
    {
        Chat.Output("Добро пожаловать!");
    }
    
    public void OnCloseBrowserMessage(object[] args)
    {
        openedWindow.Destroy();
        Cursor.ShowCursor(false, false);
        Chat.Activate(true);
    }
    public void OnPlayerConnected(object[] args) // При подключении выдается окно регистрации
    {
        openedWindow = new HtmlWindow("package://cef/auth/index.html");
        openedWindow.Active = true;
        openedWindow.ExecuteJs("document.dispatchEvent(new Event('showLogin'))");
        Cursor.ShowCursor(true, true);
        Ui.DisplayRadar(false);
        Chat.Show(false);
    }
    public void OnPlayerCreateWaypoint(Vector3 position)
    {
        Events.CallRemote("CLIENT:SERVER::CLIENT_CREATE_WAYPOINT", position.X, position.Y, position.Z);
    }



}



//-196,4577, -831,0376, 30,76789
//0, 0, -60,085663