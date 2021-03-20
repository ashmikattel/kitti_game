using Mirror;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


//custom version of network manager
public class NetworkManagerLobby : NetworkManager
{
    [SerializeField]
    private int minPlayers = 2;
    [Scene]
    [SerializeField]
    private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField]
    private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField]
    private NetworkGamePlayerLobby gamePlayerPrefab = null;
    [SerializeField]
    private GameObject playerSpawnSystem = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    // server is readied
    public static event Action<NetworkConnection> OnServerReadied;

    //list of room players to display over
    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    // load all resources from spawnable prefab folder
    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        // listen to this if client fails to connect
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if(numPlayers >= maxConnections)
        {
            // if many player disconnect them from server
            conn.Disconnect();
            return;
        }
        if (SceneManager.GetActiveScene().name != "GameLobby")
        {
            //if not in Game Lobby scene disconnect (game in progress)
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if(SceneManager.GetActiveScene().name == "GameLobby")
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if(numPlayers< minPlayers) { return false; }

        foreach(var players in RoomPlayers)
        {
            if(!players.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if(SceneManager.GetActiveScene().name == "GameLobby")
        {
            if(!IsReadyToStart()) { return; }
            ServerChangeScene("OnlineGameScene");
        }
    }

    [Server]
    public override void ServerChangeScene(string newSceneName)
    {
        if(SceneManager.GetActiveScene().name == "GameLobby" && newSceneName.StartsWith("OnlineGameScene"))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
            Debug.Log("Going to Start Gmae");
               /* var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                //NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);*/
            }
        }
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("OnlineGameScene"))
        {
            // all client have spawn system owned by the server
            /*GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);*/
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }
}
