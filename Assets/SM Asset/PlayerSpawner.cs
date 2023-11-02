using System.Collections.Generic;
using System.IO;
using Fusion;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{

    public GameObject PlayerPrefab;

    // private int id = 0;
    // private Dictionary<int, NetworkObject> ListOfPlayer;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == null) return;
        if (player == Runner.LocalPlayer) {
            NetworkObject playerObject = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
            // ListOfPlayer.Add(player.PlayerId, playerObject);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        // NetworkObject playerObject;
        // ListOfPlayer.TryGetValue(player.PlayerId, out NetworkObject playerObject);
        // if (playerObject == null) return;

        // playerObject.des
        // ListOfPlayer.Remove(player.PlayerId);
        // player.
    }
}
