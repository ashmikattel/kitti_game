using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class KittiNetworkManager : NetworkManager
{
    public Transform playerOneSpawnPoint;
    public Transform playerTwoSpawnPoint;
    public Transform playerThreeSpawnPoint;
    public Transform playerFourSpawnPoint;

    int playerIndex = 0;
    List<Transform> playerSpawnPoint;

    public override void Awake()
    {
        base.Awake();
        playerSpawnPoint = new List<Transform>() { playerOneSpawnPoint, playerTwoSpawnPoint, playerThreeSpawnPoint, playerFourSpawnPoint };
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (playerIndex < 3)
        {
            GameObject player = Instantiate(playerPrefab, playerSpawnPoint[playerIndex].position, Quaternion.identity, playerSpawnPoint[playerIndex]);
            player.name = "Player " + (playerIndex + 1);
            player.GetComponentInChildren<NameManager>().SetName("Player " + (playerIndex + 1));
            playerIndex++;
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}
