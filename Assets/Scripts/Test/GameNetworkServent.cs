using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameNetworkServent : NetworkBehaviour 
{
    [SyncVar]
    public PawnColor playercolor;

    public NetworkManagerServer nm_server;
    public GameManager gm;

	void Start () 
    {

    }

    public void SetColor()
    {
        if (hasAuthority)
        {
            if (isServer)
                playercolor = PlayerSelection.playerColor;
            else
                CmdSetColor(PlayerSelection.playerColor);
        }
    }

	void Update () 
    {
	    	
	}

    [Command]
    public void CmdSetColor(PawnColor color)
    {
        playercolor = color;
    }

    [Command]
    public void CmdDiceThrow()
    {
        nm_server.ServerDiceThrow();
    }

    [Command]
    public void CmdSetDiceValue(int val)
    {
        nm_server.UpdateDiceValue(val);
    }
 
    [Command]
    public void CmdSetplayer(string Name)
    {
        nm_server.SetPlayerOverNetwork(Name);
    }

    [Command]
    public void CmdSetNextTurn()
    {
        nm_server.SetNextTurn();
    }

    void OnDestroy()
    {
        if (nm_server != null)
        {
            nm_server.OnDropConnection(playercolor);
        }

        if (playercolor == PlayerSelection.playerColor)
        {
            if (!isServer)
            {
                NetworkManager.singleton.StopClient();
                Debug.Log("OnDestroy Client Stopped");
            }
            else
            {
                DiscoverNetworks.Instance.StopBroadcast();
                NetworkManager.singleton.StopHost();
                Debug.Log("OnDestroy Host Stopped");
            }

            Destroy(nm_server.nm);
            nm_server.GoToMainMenu();
        }
    }
}
