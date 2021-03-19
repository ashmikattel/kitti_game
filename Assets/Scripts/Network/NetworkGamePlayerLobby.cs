using TMPro;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

//sits on player that is spawned in
// custom data for player storing name on the network storing on network behaviour
// room player game player to indicate whether they are in lobby or room
public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading....";

    private NetworkManagerLobby room;

    //persist between scenes done once
    private NetworkManagerLobby Room
    {
        get
        {
            if(room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        //to not destroy
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
