using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class CustomLobbyTest : NetworkLobbyPlayer {

    public bool exposingAuthority;
	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        Debug.Log("entered Lobby");

    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("started Local Player");
        exposingAuthority = hasAuthority;
        //if(this.hasAuthority)
            //this.SendReadyToBeginMessage();
    }

}
