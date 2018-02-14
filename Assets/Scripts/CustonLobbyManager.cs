using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class CustonLobbyManager : NetworkLobbyManager 
{
    int count;
    public GameObject obj;
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
        

    public override void OnLobbyClientEnter()
    {
        base.OnLobbyClientEnter();
        Debug.Log("Lobby Entered");
        StartCoroutine(addplayer());
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        base.OnLobbyClientConnect(conn);
        Debug.Log("Connected to Lobby");
    }

    IEnumerator addplayer()
    {
        do
        {
            yield return new WaitForSeconds(0.05f);
        } while(!client.connection.isReady);


        if (client.connection.isReady)
        {
            Debug.Log("Adding player");
            TryToAddPlayer();
        }


    }

    public override void OnLobbyServerPlayersReady()
    {
        base.OnLobbyServerPlayersReady();
        Debug.Log("All Players Ready");
    }

    public override void OnLobbyClientAddPlayerFailed()
    {
        base.OnLobbyClientAddPlayerFailed();
        Debug.Log("Failed to add player");
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        count++;
        if (count == 2)
        {
            GameObject obje = (GameObject)Instantiate(obj,Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(obje);
        }
        Debug.Log("OnServer");
        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }
}
