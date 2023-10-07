using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

public class CefEvents : Events.Script
{
    public CefEvents() 
    {
        Events.Add("CEF:CLIENT::REGISTER_BUTTON_CLICKED", OnCefRegisterButtonClicked);
        Events.Add("CEF:CLIENT::LOGIN_BUTTON_CLICKED", OnCefLoginButtonClicked);
        Events.Add("CEF:CLIENT::PERSON_CREATE_BUTTON_CLICKED", OnCefPersonCreateButtonClicked);
        Events.Add("CEF:CLIENT::PERSON_CREATE_GENDER_SWITCH_BUTTON_CLICKED", OnCefPersonCreateGenderSwitchButtonClicked);

    }

    public void OnCefLoginButtonClicked(object[] args)
    {
        string username = args[0].ToString();
        string password = args[1].ToString();

        Events.CallRemote("CLIENT:SERVER::LOGIN_BUTTON_CLICKED", username, password);
    }

    public void OnCefRegisterButtonClicked(object[] args)
    {
        string username = args[0].ToString();
        string password = args[1].ToString();
        string email = args[2].ToString();

        Events.CallRemote("CLIENT:SERVER::REGISTER_BUTTON_CLICKED", username, password, email);
    }

    public void OnCefPersonCreateButtonClicked(object[] args)
    {
        string name = args[0].ToString();
        string secondName = args[1].ToString();
        string age = args[2].ToString();
        string gender = args[3].ToString();

        Events.CallRemote("CLIENT:SERVER::PERSON_CREATE_BUTTON_CLICKED", name, secondName, age, gender);
    }

    public void OnCefPersonCreateGenderSwitchButtonClicked(object[] args)
    {
        string gender = args[0].ToString();
        Events.CallRemote("CLIENT:SERVER::PERSON_CREATE_GENDER_SWITCH_BUTTON_CLICKED", gender);
    }
}