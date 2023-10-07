using Google.Protobuf.WellKnownTypes;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

public class RemoteEvents : Script
{
    [RemoteEvent("CLIENT:SERVER::CLIENT_CREATE_WAYPOINT")]
    public void OnClientCreateWaypoint(Player player, float posX, float posY, float posZ)
    {
        player.Position = new Vector3(posX, posY, posZ);
    }

    [ServerEvent(Event.PlayerConnected)]            // При подключении выдается окно регистрации
    public void OnPlayerConnected(Player player)
    {
        player.TriggerEvent("SERVER:CLIENT::AuthReady");
    }

    [ServerEvent(Event.PlayerSpawn)]            // СПАВН ПОСЛЕ СМЕРТИ
    public void OnPlayerSpawn(Player player)
    {
        player.Position = new Vector3(-209.24597, -788.7034, 30.454039);
        player.Rotation = new Vector3(0, 0, -109.61787);
        player.Dimension = player.Id;
    }

    [RemoteEvent("CLIENT:SERVER::REGISTER_BUTTON_CLICKED")]
    public async void OnCefRegisterButtonClicked(Player player, string username, string password, string email)
    {
        string selectQuery = "SELECT * FROM users WHERE username = @name";
        MySqlCommand selectCommand = new MySqlCommand(selectQuery);
        selectCommand.Parameters.AddWithValue("@name", username);

        DataTable tb = await MySQL.QueryReadAsync(selectCommand);
        if (tb.Rows.Count > 0)
        {
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::REGISTER_USER", true);
            }, 1000);
        }
        else
        {
            string hashedPassword = Crypto.HashPassword(password);

            NAPI.Util.ConsoleOutput("Игрок НЕ существует!");
            string insertQuery = "INSERT INTO users (username, password, email) VALUES (@name, @password, @email)";
            MySqlCommand insertCommand = new MySqlCommand(insertQuery);
            insertCommand.Parameters.AddWithValue("@name", username);
            insertCommand.Parameters.AddWithValue("@password", hashedPassword);
            insertCommand.Parameters.AddWithValue("@email", email);


            MySQL.Query(insertCommand);
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::REGISTER_USER", false);
                player.Position = new Vector3(2239.4763, -1210.3835, 149.53323);
                player.Rotation = new Vector3(0, 0, 127.00000);
                //player.Dimension = player.Id;
                ResetPersonCustomiztion(player); 
                player.SetData("player_username", username);
            }, 1000);
        }
        
    }

   
    [RemoteEvent("CLIENT:SERVER::LOGIN_BUTTON_CLICKED")]
    public async void OnCefLoginButtonClicked(Player player, string username, string password)
    {
        string selectQuery = "SELECT * FROM users WHERE username = @name";
        MySqlCommand selectCommand = new MySqlCommand(selectQuery);
        selectCommand.Parameters.AddWithValue("@name", username);
        DataTable tb = await MySQL.QueryReadAsync(selectCommand);
        if (tb.Rows.Count == 0)
        {
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::LOGIN_USER", true);
            }, 1000);
        }
        else
        {
            string hashedPassword = Crypto.HashPassword(password);
            string outUsername = tb.Rows[0].ItemArray[1].ToString();
            string outHashedPassword = tb.Rows[0].ItemArray[2].ToString();
            string outEmail = tb.Rows[0].ItemArray[3].ToString();
            string outPersonName = tb.Rows[0].ItemArray[4].ToString();
            string outPersonSName = tb.Rows[0].ItemArray[5].ToString();
            string outPersonAge = tb.Rows[0].ItemArray[6].ToString();
            string outPersonGender = tb.Rows[0].ItemArray[7].ToString();

            if (outHashedPassword != hashedPassword) 
            {
                NAPI.Task.Run(() =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::LOGIN_USER", true);     // Если пароли не совпадают ошибка
                }, 1000);
            }
            else
            {
                if (outPersonName == null || outPersonName.Length == 0)
                {
                    NAPI.Task.Run(() =>
                    {
                        NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::REGISTER_USER", false);
                        player.Position = new Vector3(2239.4763, -1210.3835, 149.53323);
                        player.Rotation = new Vector3(0, 0, 127.00000);
                        player.Dimension = player.Id;
                        player.SetData("player_username", username);

                    }, 1000);
                }
               else
                {
                    NAPI.Task.Run(() =>
                    {
                        NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::LOGIN_USER", false);
                        OnCefPersonCreateGenderSwitchButtonClicked(player, outPersonGender);
                        player.SetData("player_username", outUsername);
                        player.Position = new Vector3(-209.24597, -788.7034, 30.454039);
                        player.Rotation = new Vector3(0, 0, -109.61787);
                        //player.Dimension = player.Id;
                    }, 1000);
                }
            }
        }
    }

    [RemoteEvent("CLIENT:SERVER::PERSON_CREATE_BUTTON_CLICKED")]
    public void OnCefPersonCreateButtonClicked(Player player, string name, string secondName, string age, string gender)
    {
        if(player.HasData("player_username"))
        {
            string username = player.GetData<string>("player_username");

            string updateQuery = "UPDATE users SET name = @name, sName = @sName, age = @age, gender = @gender WHERE username = @username";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery);

            updateCommand.Parameters.AddWithValue("@name", name);
            updateCommand.Parameters.AddWithValue("@sname", secondName);
            updateCommand.Parameters.AddWithValue("@age", age);
            updateCommand.Parameters.AddWithValue("@gender", gender);
            updateCommand.Parameters.AddWithValue("@username", username);

            MySQL.Query(updateCommand);
            NAPI.Task.Run(() =>
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "SERVER:CLIENT::CREATE_PERSON", false);
                player.Position = new Vector3(-209.24597, -788.7034, 30.454039);        // СПАВН ПОСЛЕ СОЗДАНИЯ ПЕРСОНАЖА
                player.Rotation = new Vector3(0, 0, -109.61787);
                player.Dimension = player.Id;
            }, 1000);
        }
    }

    [RemoteEvent("CLIENT:SERVER::PERSON_CREATE_GENDER_SWITCH_BUTTON_CLICKED")]

    public void OnCefPersonCreateGenderSwitchButtonClicked(Player player, string gender)
    {
        HeadBlend headBlend = new HeadBlend()
        {
            ShapeFirst = 21,
            ShapeSecond = 0,
            ShapeThird = 0,
            SkinFirst = 21,
            SkinSecond = 0,
            SkinThird = 0,
            ShapeMix = 0.5f,
            SkinMix = 0.5f,
            ThirdMix = 0
        };

        Dictionary<int, HeadOverlay> headOverlays = new Dictionary<int, HeadOverlay>();

        float[] faceFeatures = new float[20]
        {
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0
        };

        bool isMale = gender == "male"; //true - если мужской, false - если женский
        player.SetCustomization(isMale, headBlend, byte.MinValue, byte.MinValue, byte.MinValue, faceFeatures, headOverlays, new Decoration[] { });
    }

    private void ResetPersonCustomiztion(Player player)
    {
        HeadBlend headBlend = new HeadBlend()
        {
            ShapeFirst = 21,
            ShapeSecond = 0,
            ShapeThird = 0,
            SkinFirst = 21,
            SkinSecond = 0,
            SkinThird = 0,
            ShapeMix = 0.5f,
            SkinMix = 0.5f,
            ThirdMix = 0
        };

        Dictionary<int, HeadOverlay> headOverlays = new Dictionary<int, HeadOverlay>();

        float[] faceFeatures = new float[20]
        {
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0
        };

        player.SetCustomization(true, headBlend, byte.MinValue, byte.MinValue, byte.MinValue, faceFeatures, headOverlays, new Decoration[] { });
    }
}