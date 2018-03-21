using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.Analytics;


public class NetworkTest : MonoBehaviour {

    public static bool isLAN;

	public Text ipAddText;
	public GameObject menuObj, hostList, serverSetup, mpModeObj, themeNShop, enterNametext;

	public List<string> gameName;
	public List<string> gameIP;
	public GameObject scrollparent, gameElement;

	public InputField serverNameRP;
	public string nameData;

    public List<GameObject> gamesList;
    public GameObject NoGameText;
    //method to differentiate between lan and online
 

	void Start ()
    {
		SetMultiplayerMode (isLAN);
        //if(!isLAN)
		//InvokeRepeating ("RefreshGameList", 0, 10f);
    }
	
	void Update ()
    {
		
	}

    public void SetMultiplayerMode(bool isOffline)
    {
        //isLAN = isOffline;
		Debug.Log("IS lan game : " + isOffline);
        mpModeObj.SetActive(false);
        menuObj.SetActive(true);

		if (!isOffline)
        {
            NetworkManager.singleton.StartMatchMaker();
            PlayerSelection.isNetworkedGame = true;
        }
		else if (isOffline)
        {
            Debug.Log("Hooked offline mp event");
            DiscoverNetworks.Instance.onDetectServer += ReceivedBroadcast;
        }
    }

   
	public void TimeToHost()
	{
        //serverSetup.SetActive(true);
        // CoinManager.CoinsText = GameObject.Find("NoOfCoins_txt").GetComponent<Text>();
        NoGameText.SetActive(false);
        themeNShop.SetActive(true);
        menuObj.SetActive(false);
        CoinManager.Init();

        AudioManager.Instance.UIClick();
    }

    public void SetupRoom()
    {
        print(SelectPlayField.whichLevel);

		AnalyticsResult result = Analytics.CustomEvent ("SelectedBoard", new Dictionary<string, object>			//log analytics event for board selection
			{
				{"SelectedLevel", SelectPlayField.whichLevel}	
			});
		print ("Board analytics result : " + result);

        serverSetup.SetActive(true);
        themeNShop.SetActive(false);

		AudioManager.Instance.UIClick();
    }

	public void HostGame()      //Hosting a game
	{
        Debug.Log ("started as host");

		if (serverNameRP.text != "") {

			serverSetup.SetActive (false);
			DiscoverNetworks.Instance.StopBroadcast ();
			NetworkServer.Reset ();
			if (isLAN) {
				Debug.Log ("LAN SHIT : ");
				DiscoverNetworks.Instance.StartBroadCasting ();
				NetworkManager.singleton.StartHost ();
				// LobbyManager.tempName = NetworkManager.singleton.matchName;
			} else {
				Debug.Log ("ONLINE SHIT : " + NetworkManager.singleton.matchName);

				CreateInternetMatch (LobbyManager.tempName);
			}
		} else {
			if (enterNametext != null) {
				enterNametext.SetActive (true);
				Invoke ("DisableText", 5f);
			}
		}

		AudioManager.Instance.UIClick();
	}

	void DisableText()
	{
		enterNametext.SetActive (false);
	}


    public void ReadyToJoin(GameObject ip)
    {
      //  CancelInvoke("RefreshGameList");
        if (isLAN)
        {
            JoinGame(ip);
        }
        else
        {
            //  StartCoroutine(joinMatch(ip.GetComponent<GameDataHolder>().matchDetails));
         //   CancelInvoke("RefreshGameList");
            
            joinMatch((ip.GetComponent<GameDataHolder>().matchDetails));
        }
    }


	//join a game
	public void JoinGame(GameObject ip)
	{
        hostList.SetActive(false);
		print(ip.GetComponent<GameDataHolder>().ip);
        //Network.Disconnect();
        //NetworkServer.Reset();
        NetworkManager.singleton.networkAddress = ip.GetComponent<GameDataHolder>().ip;
        NetworkManager.singleton.StartClient ();

        DiscoverNetworks.Instance.StopBroadcast();
    }

	//Click on join to get list of open games
	public void ReceiveGameBroadCast()
	{
		AudioManager.Instance.UIClick();
        if (isLAN)
        {
            DiscoverNetworks.Instance.ReceiveBroadcast();
        }
        else
        {
            print("Find match");
            //FindInternetMatch("");
          InvokeRepeating("RefreshGameList", 0f, 10f);
        }
	}

	public void ReceivedBroadcast(string fromIP, string data)
	{
        //do something
        print("Display stuff" + data);
       // ipAddText.text = fromIP;
		menuObj.SetActive (false);
		hostList.SetActive (true);

		if (!gameName.Contains (data))
        {
			gameName.Add (data);
            gameIP.Add(fromIP);
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


    //--------------------- Online -------------------------------------------

    public void RefreshGameList()
    {
        foreach (GameObject obj in gamesList)
        {
            Destroy(obj);
        }
        FindInternetMatch("");
    }

    //call this method to request a match to be created on the server
    public void CreateInternetMatch(string matchName)
    {
        NetworkManager.singleton.matchMaker.CreateMatch(matchName, 4, true, "", "", "", 0, 0, OnInternetMatchCreate);
        //PlayerSelection.playerColor = PawnColor.c_Blue;
    }

    //this method is called when your request for creating a match is returned
    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("returned request");
        if (success)
        {
            //Debug.Log("Create match succeeded");

            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);
        }
        else
        {
            Debug.LogError("Create match failed");
        }

    }

    //call this method to find a match through the matchmaker
    public void FindInternetMatch(string matchName)
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 10, matchName, true, 0, 0, OnInternetMatchList);
    }

    //this method is called when a list of matches is returned
    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            if (matches.Count != 0)
            {
                Debug.Log("A list of matches was returned");

                menuObj.SetActive(false);
                hostList.SetActive(true);
                //join the last server (just in case there are two...)
                //NetworkManager.singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
                //showCavas.gameObject.SetActive(true);
                for (int i = 0; i < matches.Count; i++)
                {
                    // GameObject obj = (GameObject)Instantiate(button, scrollTrans);
                    // obj.transform.GetChild(0).GetComponent<Text>().text = matches[0].name;
                    GameObject newGame = Instantiate(gameElement, scrollparent.transform) as GameObject;
                    newGame.GetComponent<Text>().text = matches[i].name;
                    newGame.GetComponent<GameDataHolder>().matchDetails = matches[i];
                    gamesList.Add(newGame);
                   
                }
                NoGameText.SetActive(false);
            }
            else
            {
                NoGameText.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Couldn't connect to match maker");
        }
    }

    //this method is called when your request to join a match is returned
    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Join success");
            float t = 0;
            while (t < 1)
            {
                t += Time.fixedDeltaTime;
            }
            MatchInfo hostInfo = matchInfo;
            Debug.Log(matchInfo);
			NetworkManager.singleton.StopClient ();
            NetworkManager.singleton.StartClient(hostInfo);
            //PlayerSelection.playerColor = PawnColor.c_Green;
        }
        else
        {
            Debug.LogError("Join match failed");
        }
    }

   

    void joinMatch(MatchInfoSnapshot matches)
    {
        Debug.Log(matches.networkId);
        //yield return new WaitForSeconds(2f);
        //NetworkManager.singleton.matchMaker.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
        NetworkManager.singleton.matchMaker.JoinMatch(matches.networkId, "", "", "", 0, 0, OnJoinInternetMatch);
        Debug.Log(matches.name);
        CancelInvoke();
    }
}
