using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiscoverNetworks : NetworkDiscovery
{

    public static DiscoverNetworks Instance;
    public string bdData;
    public Action<string, string> onDetectServer;
    public NetworkTest networkScript;

    public GameObject gameServerScriptPrefab;
    public GameObject gameservantScriptPrefab;

    void Awake()
    {
        networkScript = GameObject.Find ("Canvas").GetComponent<NetworkTest> ();
        if (Instance == null) {
            Instance = this;
        }

    }

    // Use this for initialization
    void Start ()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            StopBroadcast();
            NetworkManager.singleton.StopMatchMaker();
            if (isServer)
            {
                NetworkManager.singleton.StopHost();

            }
            else
            {
                NetworkManager.singleton.StopClient();

            }
            NetworkServer.Reset();
            Network.Disconnect();
            SceneManager.sceneLoaded -= OnSceneLoaded;

            Invoke("DestroyInsitance", 1f);
        }
    }

    public void DestroyInsitance()
    {
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (NetworkManager.singleton.isNetworkActive)
        {
            StopBroadcast();
            NetworkManager.singleton.StopMatchMaker();
            NetworkServer.Reset();
            if (isServer)
            {
                NetworkManager.singleton.StopHost();
        
            }
            else
            {
                NetworkManager.singleton.StopClient();
        
            }
            Network.Disconnect();
        }
    }

    void OnServerDetected(string add, string data)
    {

        if (onDetectServer != null) {
            onDetectServer.Invoke (add, data);
            print(data);
        }
    }

    public bool InitNetworkDiscovery()
    {
        Debug.Log("ND Initialised");
        return Initialize ();
    }

    public void StartBroadCasting()
    {
        //print("Running as server");
        //StartAsServer();

        this.StopBroadcast();

        if (!running)
        {
            Debug.Log("Start hosting");
            if (InitNetworkDiscovery())
            {
                this.StartAsServer();
            }
        }
    }

    public void SetBDData(InputField name)
    {
        bdData = name.text;
        if (NetworkTest.isLAN)
        {
            broadcastData = bdData;
        }
        else
        {
            if(InternetChecker.internetConnectBool)
                LobbyManager.tempName = bdData;
        }
    }

    public void ReceiveBroadcast()
    {
        //StartAsClient();

        this.StopBroadcast();

        if (!running)
        {

            if (InitNetworkDiscovery())
            {
                this.StartAsClient();
            }
            Debug.Log("Start listening");
        }
    }

    public override void OnReceivedBroadcast(string formAdd, string data)
    {
        OnServerDetected(formAdd.Split(':')[3], data);
    }

    void OnApplicationQuit()
    {
        StopBroadcast ();

        if (isServer)
        {
            NetworkManager.singleton.StopHost ();
            NetworkManager.singleton.StopMatchMaker();
        }
        else
        {
            NetworkManager.singleton.StopClient ();
            NetworkManager.singleton.StopMatchMaker();
        }
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (Network.isServer)
            Debug.Log("Local server connection disconnected");
        else
            if (info == NetworkDisconnection.LostConnection)
            Debug.Log("Lost connection to the server");
        else
            Debug.Log("Successfully diconnected from the server");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("LudoMenu");
    }

}
