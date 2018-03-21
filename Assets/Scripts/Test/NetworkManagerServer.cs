﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class NetworkManagerServer : NetworkBehaviour
{
    public GameManager gm;
    //UIManager uiManager;

    public GameObject[] servent;
    public GameNetworkServent ownServent;
    public int readycount;
    public PawnColor col;
    public GameObject dice;
    public GameObject nm;

    public bool isPawnMoving;

    // Use this for initialization
    void Awake()
    {
        col = PlayerSelection.playerColor;

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        nm = GameObject.Find("NetWork Manager");

        gm.nm_server = this;
    }


    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        nm = GameObject.Find("NetWork Manager");

        gm.nm_server = this;
        servent = GameObject.FindGameObjectsWithTag("NetworkManager");

        for (int i = 0; i < servent.Length; i++)
        {
            if (servent[i].GetComponent<GameNetworkServent>().hasAuthority)
            {
                ownServent = servent[i].GetComponent<GameNetworkServent>();
                ownServent.nm_server = this;
                //break;
            }

            //if (isServer)
            {
                servent[i].GetComponent<GameNetworkServent>().nm_server = this;
            }

        }

        if (isServer)
        {
            for (int i = 0; i < servent.Length; i++)
            {
                servent[i].GetComponent<GameNetworkServent>().nm_server = this;
            }
            GameObject pos = GameObject.Find("DicePosition");
            GameObject obj = (GameObject)Instantiate(dice, pos.transform.position, Quaternion.identity);
            NetworkServer.Spawn(obj);
            StartCoroutine(StartGame());
        }

    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        if (servent.Length == PlayerSelection.playerInfo.Count)
        {
            RpcInitGame();
            Debug.Log("Calling RPC");
        }
    }

    [ClientRpc]
    void RpcInitGame()
    {
        gm.InitializeGame();
    }

    [ClientRpc]
    void RpcSetTurn(int num)
    {
        gm.StartNetworkedTurn(num);
    }
    [ClientRpc]
    void RpcSetPlayer(string Name)
    {
        gm.SelectPlayerNetwork(Name);
        Debug.Log("Player is " + Name + " OnServer");
    }

    [ClientRpc]
    void RpcSetVal(int num)
    {
        gm.SetDiceRollValue(num);
        gm.HideDice();
        Debug.Log("Dice value is\t" + num);
    }

    [ClientRpc]
    public void RpcUpdateDiceOnLockedPlayerSelect(int num)
    {
        gm.networkedPlayerDiceRollValues = num;
        Debug.Log("Updated Player Value");
    }

    [ClientRpc]
    void RpcSetNextTurn()
    {
        gm.SetNextTurn();
    }

    [ClientRpc]
    void RpcDiceShowEffect()
    {
        gm.ShowDice();
    }

    [ClientRpc]
    void RpcUIDiceChange(int val)
    {
        gm.showUIDice(val);
    }

    public void UIDice(int val)
    {
        if(isServer)
        {
            RpcUIDiceChange(val);
        }
    }

    public void SetDiceValue(int val)
    {
        if (isServer)
        {
            RpcSetVal(val);
        }
    }

    public void UpdateDiceValue(int val)
    {
        if (isServer)
        {
            RpcUpdateDiceOnLockedPlayerSelect(val);
        }
        else
        {
            ownServent.CmdSetDiceValue(val);
        }
    }

    public void GetStartingTurn(int count)
    {
        if (isServer)
        {
            int randIndex = Random.Range(0, count);
            gm.StartNetworkedTurn(randIndex);
            RpcSetTurn(0);
        }
    }

    public void SetPlayerOverNetwork(string Name)
    {
        if (isServer)
        {
            RpcSetPlayer(Name);
        }
        else
        {
            ownServent.CmdSetplayer(Name);
        }
    }

    public void ThrowDice()
    {
        if (isServer)
        {
            ServerDiceThrow();
        }
        else
        {
            ownServent.CmdDiceThrow();
        }
    }

    public void ServerDiceThrow()
    {
        RpcDiceShowEffect();
        gm.dice.ThrowDice();
    }

    public void SetNextTurn()
    {
        if (isServer)
        {
            RpcSetNextTurn();
        }
        else
        {
            ownServent.CmdSetNextTurn();
        }
    }

    public void OnDropConnection(PawnColor col)
    {
        Debug.Log(Time.time + "Pawn Destroyed");

        if (col == PlayerSelection.playerColor)
        {
            gm.OnLoosingServer();
        }
        else
        {
            gm.CheckPlayersOnDisconnect(col);
        }

        int indexOfColor = gm.totalPlayers.IndexOf(col);
        gm.totalPlayers.Remove(col);
        gm.playerNames.RemoveAt(indexOfColor);
        switch (col)
        {
            case PawnColor.c_Blue:
                gm.BlueGameObject.SetActive(false);
                break;
            case PawnColor.c_Green:
                gm.GreenGameObject.SetActive(false);
                break;
            case PawnColor.c_Red:
                gm.RedGameObject.SetActive(false);
                break;
            case PawnColor.c_Yellow:
                gm.YellowGameObject.SetActive(false);
                break;
        }
        if (gm.currentPlayerTurn == col)
        {
            SetNextTurn();
        }

       //if (gm.totalPlayers.Count < 2)
       //{
       //    SceneManager.LoadScene(0);
       //}


    }

    private void OnDestroy()
    {
        NetworkManager.singleton.StopMatchMaker();
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StopClient();
        NetworkServer.Reset();
        Network.Disconnect();
    }
}
