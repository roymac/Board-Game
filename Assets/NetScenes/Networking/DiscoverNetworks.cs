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
        //InitNetworkDiscovery(); //Initialize network discovery
    }

	void OnServerDetected(string add, string data)
	{
		print (data);
		if (onDetectServer != null) {
			onDetectServer.Invoke (add, data);
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
		broadcastData = bdData;
	}

	public void ReceiveBroadcast()
	{
        //StartAsClient();

        this.StopBroadcast();

        if (!running)
        {
            Debug.Log("Start listening");
            if (InitNetworkDiscovery())
            {
                this.StartAsClient();
            }
        }
    }

    public override void OnReceivedBroadcast(string formAdd, string data)
	{
		OnServerDetected(formAdd.Split(':')[3], data);
	}

	void OnApplicationQuit()
	{
		StopBroadcast ();
		if (!isClient) {
			NetworkManager.singleton.StopHost ();
		} else {
			NetworkManager.singleton.StopClient ();
		}

        //NetworkTransport.Shutdown();
        //NetworkTransport.Init();
    }

}
