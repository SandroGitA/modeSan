using RAGE;
using RAGE.Elements;
using RAGE.Game;
using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Text;

public class ServerEvents : Events.Script
{
    public ServerEvents()
    {
        Events.Add("SERVER:CLIENT::REGISTER_USER", OnServerRegisterUser);
        Events.Add("SERVER:CLIENT::LOGIN_USER", OnServerLoginUser);
        Events.Add("SERVER:CLIENT::CREATE_PERSON", OnServerCreatePerson);
    }

    public void OnServerRegisterUser(object[] args)
    {
        bool isExists = (bool)args[0];
        if (isExists) Main.openedWindow.ExecuteJs("document.dispatchEvent(new Event('registerUserExists'))");
        else
        {
            Main.openedWindow.Destroy();
            Chat.Activate(true);

            Main.currentCamera = new Camera((ushort)Cam.CreateCameraWithParams(Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), 2237.3994f, -1211.000f, 149.56767f, 0f, 0f, -33.666588f, 70.0f, true, 2), 0);
            Cam.PointCamAtCoord(Main.currentCamera.Id, 2239.4763f, -1210.3835f, 149.53323f);
            Cam.SetCamActive(Main.currentCamera.Id, true);
            Cam.RenderScriptCams(true, false, 0, true, false, 0);

            Main.openedWindow = new HtmlWindow("package://cef/person_creater/index.html");
            Main.openedWindow.Active = true;
            Cursor.ShowCursor(true, true);
        }
    }

    public void OnServerLoginUser(object[] args)
    {
        bool notExists = (bool)args[0];
        if (notExists) Main.openedWindow.ExecuteJs("document.dispatchEvent(new Event('loginNotValidData'))");
        else
        {
            Main.openedWindow.Destroy();
            Cursor.ShowCursor(false, false);
            Chat.Show(true);
            Ui.DisplayRadar(true);
        }
    }

    public void OnServerCreatePerson(object[] args)
    {
        if (Main.openedWindow != null) Main.openedWindow.Destroy();
        Cursor.ShowCursor(false, false);
        Ui.DisplayRadar(true);
        Chat.Show(true);
        Cam.RenderScriptCams(false, false, 0, true, false, 0);
        Main.currentCamera = null;
    }
}