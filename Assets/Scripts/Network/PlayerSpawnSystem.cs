﻿using Mirror;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x=>x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;

    public override void OnStartClient()
    {
        Debug.Log("Start Client");
    }

    [ServerCallback]
    private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if(spawnPoint == null)
        {
            Debug.Log($"Missing spawn point for player {nextIndex}");
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, Quaternion.identity);
        playerInstance.GetComponent<Transform>().SetParent(spawnPoints[nextIndex]);
        NetworkServer.Spawn(playerInstance, conn);
        nextIndex++;
    }
}
