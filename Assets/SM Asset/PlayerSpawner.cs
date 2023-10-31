using System.Collections.Generic;
using System.IO;
using Fusion;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{

    public GameObject PlayerPrefab;

    private int id = 0;
    private Dictionary<PlayerRef, int> ListOfPlayer;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer) {
            ListOfPlayer.Add(player, id++);
            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        ListOfPlayer.Remove(player);
        player.
    }
}
