using GTANetworkAPI;
using System;

public class Commands : Script
{
    [Command("getpos")]
    public void Cmd_GetPos(Player player)
    {
        Vector3 playerPosition = player.Position;
        Vector3 playerRotation = player.Rotation;
        NAPI.Util.ConsoleOutput($"{playerPosition.X}, {playerPosition.Y}, {playerPosition.Z}");
        NAPI.Util.ConsoleOutput($"{playerRotation.X}, {playerRotation.Y}, {playerRotation.Z}");
    }
    [Command("givew")]   // Команда выдачи оружия

    public void Cmd_giveWeapon(Player player, WeaponHash weaponHash, int ammo)  
    {
        NAPI.Player.GivePlayerWeapon(player, weaponHash, ammo);
        NAPI.Chat.SendChatMessageToPlayer(player, $"Игрок {player.Name} получает {weaponHash} и {ammo} патронов");
    }

    [Command("spawnveh")]   // Команда спавна машин
    public void Cmd_spawnVehicle(Player player, VehicleHash vehicleHash, int color1, int color2, string platenumber)
    {
        Vector3 playerPosition = player.Position;
        Vehicle getVeh = NAPI.Vehicle.CreateVehicle(vehicleHash, new Vector3(playerPosition.X + 1f, playerPosition.Y + 2f, playerPosition.Z + 1f), 10f, color2, color1, platenumber);
        NAPI.Chat.SendChatMessageToPlayer(player, $"Игрок {player.Name} спавнит {vehicleHash}");
    }

    [Command("addnpc")]
    public void addnpc(Player player, PedHash pedHash)
    {
        Vector3 PlayerPos = NAPI.Entity.GetEntityPosition(player);
        Ped John = NAPI.Ped.CreatePed((uint)pedHash, new Vector3(PlayerPos.X + 1f, PlayerPos.Y + 1f, PlayerPos.Z + 1f), 5f, true, false, false, false, 0);
        NAPI.Chat.SendChatMessageToPlayer(player, $"Игрок: {player.Name} | Заспавнил НПС: {pedHash}!");
    }
}
