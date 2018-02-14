using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class NetworkTest : MonoBehaviour {

	public Text ipAddText;
	public GameObject menuObj, hostList, serverSetup;

	public List<string> gameName;
	public List<string> gameIP;
	public GameObject scrollparent, gameElement;

	public InputField serverName;
	public string nameData;

	// Use this for initialization
	void Start () {
		DiscoverNetworks.Instance.onDetectServer += ReceivedBroadcast;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void TimeToHost()
	{
		serverSetup.SetActive (true);
		menuObj.SetActive (false);
	}

	public void HostGame()
	{
        Debug.Log ("started as host");
		serverSetup.SetActive (false);

        DiscoverNetworks.Instance.StartBroadCasting();
        NetworkManager.singleton.StartHost ();

		LobbyManager.tempName = NetworkManager.singleton.matchName;
	}

	//join a game
	public void JoinGame(GameObject ip)
	{
        hostList.SetActive(false);
		print(ip.GetComponent<GameDataHolder>().ip);

        NetworkManager.singleton.networkAddress = ip.GetComponent<GameDataHolder>().ip;
        NetworkManager.singleton.StartClient ();

        DiscoverNetworks.Instance.StopBroadcast();
    }

	//Click on join to get list of open games
	public void ReceiveGameBroadCast()
	{
		DiscoverNetworks.Instance.ReceiveBroadcast ();
	}

	public void ReceivedBroadcast(string fromIP, string data)
	{
		//do something
	//	print("Display stuff" + data);
		ipAddText.text = fromIP;
		menuObj.SetActive (false);
		hostList.SetActive (true);

		if (!gameName.Contains (data)) {
			gameName.Add (data);
			gameIP.Add (fromIP);
			GameObject newGame = Instantiate (gameElement, scrollparent.transform) as GameObject;
			newGame.GetComponent<Text> ().text = data;
			newGame.GetComponent<GameDataHolder> ().name = data;
			newGame.GetComponent<GameDataHolder> ().ip = fromIP;
		}
	}

	void OnDestroy()
	{
		DiscoverNetworks.Instance.onDetectServer -= ReceivedBroadcast;
	}

}
