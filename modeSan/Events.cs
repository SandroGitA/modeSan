using GTANetworkAPI;
using GTANetworkMethods;
using MySql.Data.MySqlClient;
using System.Data;

public class Events : Script
{
    [ServerEvent(Event.ResourceStart)]
    public void OnResourceStart()
    {
        NAPI.Util.ConsoleOutput("Hello user!");
        MySQL.Test();

        string query = "SELECT * FROM users";
        using MySqlCommand command = new MySqlCommand(query);
        DataTable dt = MySQL.QueryRead(command);
        foreach (DataRow dr in dt.Rows)
        {
            var cells = dr.ItemArray;
            foreach (var cell in cells)
            {
                //NAPI.Util.ConsoleOutput(cell.ToString());
            }
        }
    }
}