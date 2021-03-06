using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Netmanager : NetworkBehaviour 
{
    public GameObject serverPrefab;
    public GameObject servantPrefab;
    public string tempName;
    public LobbyManager _lm;
	// Use this for initialization
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
	void Start () 
	{
       _lm = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
		GameObject.Find ("Canvas").GetComponent<UIScript> ().OnAddPlayerbackend ();
        if (isServer)
        {
            LobbyManager.noOfPlayers++;
			Debug.Log (LobbyManager.noOfPlayers);
        }

        DontDestroyOnLoad(this.gameObject);
		
		if (hasAuthority) 
		{
			GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().ownManager = this;
		}	
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > 2)
        {
            if (isServer && hasAuthority)
            {
                GameObject obj = (GameObject)Instantiate(serverPrefab);
                NetworkServer.Spawn(obj);
                //NetworkManager.singleton.matchMaker.SetMatchAttributes;
                //NetworkManager.singleton.matchMaker.SetMatchAttributes(NetworkManager.singleton.matchMaker.)
            }
            GetComponent<GameNetworkServent>().SetColor();
            DiscoverNetworks.Instance.StopBroadcast();
        }

        if (scene.buildIndex == 0)
        {
            if (hasAuthority)
            {
                if (!isServer)
                {
                    CmdCleanup();
                }
                else
                {
                    NetworkServer.Destroy(this.gameObject);
                }
                
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    public void OnBack()
    {
        NetworkManager.singleton.StopMatchMaker();
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
    }

    private void OnApplicationQuit()
    {
        OnBack();
    }

    void OnDestroy()
    {
        Debug.Log(Time.time + "Destroy Called"); 
      //  SceneManager.sceneLoaded -= OnSceneLoaded;
        //LobbyManager _lm = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();

        if (SceneManager.GetActiveScene().name == "LobbyScene")
        {
//			CoinManager.AwardCoins (CoinManager.justDeductedCoins);
			GameObject obj = GameObject.Find ("Canvas");
			if (obj != null)
			{
				obj.GetComponent<UIScript> ().OnRemovePlayerbackend ();
			}
            if(_lm == null)
            {
                Debug.Log("LobbyManager not found");
                return;
            }

            if (_lm.isServer)
            {
                Debug.Log("IsServer");
                LobbyManager.noOfPlayers--;
               // _lm.isplayerReady--;
            
                //return;
                if (_lm.playerOneField == tempName)         //Clear the name from the lobby scene if a player dc
                {
                    Debug.Log("First Field");
                    _lm.playerOneField = "";
                    _lm.p1 = false;
                    _lm.P1Field.GetComponent<GameDataHolder>().isSelected = false;
					_lm.playerStructure1.color = PawnColor.c_null;
					_lm.playerStructure1.selected = false;

                  //  ClearName(_lm.playerOneField, _lm.p1);
                }
                else if (_lm.playerTwoField == tempName)
                {
                    Debug.Log("Second Field");
                    _lm.playerTwoField = "";
                    _lm.p2 = false;
                    _lm.P2Field.GetComponent<GameDataHolder>().isSelected = false;
					_lm.playerStructure2.color = PawnColor.c_null;
					_lm.playerStructure2.selected = false;
                }
                else if (_lm.playerThreeField == tempName)
                {
                    Debug.Log("Third Field");
                    _lm.playerThreeField = "";
                    _lm.p3 = false;
                    _lm.P3Field.GetComponent<GameDataHolder>().isSelected = false;
					_lm.playerStructure3.color = PawnColor.c_null;
					_lm.playerStructure3.selected = false;
                }
                else if (_lm.playerFourField == tempName)
                {
                    Debug.Log("Fourth Field");
                    _lm.playerFourField = "";
                    _lm.p4 = false;
                    _lm.P4Field.GetComponent<GameDataHolder>().isSelected = false;
					_lm.playerStructure4.color = PawnColor.c_null;
					_lm.playerStructure4.selected = false;
                }
            }
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void ClearName(string name, bool isSet)
    {
        name = "";
        isSet = false;
    }

    [Command]
    public void CmdCleanup()
    {
        Debug.Log(Time.time + "Command Called");
        NetworkServer.Destroy(this.gameObject);
    }

	[Command]
	public void CmdSetData_1(string Name)
	{
		GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().playerOneField = Name;
        tempName = Name;
	}
	[Command]
	public void CmdSetData_2(string Name)
	{
		GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().playerTwoField = Name;
        tempName = Name;
    }
	[Command]
	public void CmdSetData_3(string Name)
	{
		GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().playerThreeField = Name;
        tempName = Name;
    }
	[Command]
	public void CmdSetData_4(string Name)
	{
		GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().playerFourField = Name;
        tempName = Name;
    }

    [Command]
    public void CmdSetPlayer(int playerValue)
    {
        LobbyManager lm = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();

        switch (playerValue)
        {
            case 1:
                if (!lm.p1 && !lm.isSelected)
                {
                    lm.p1 = true;
                }
                break;
            case 2:
                if (!lm.p2 && !lm.isSelected)
                {
                    lm.p2 = true;
                }
                break;
            case 3:
                if (!lm.p3 && !lm.isSelected)
                {
                    lm.p3 = true;
                }
                break;
            case 4:
                if (!lm.p4 && !lm.isSelected)
                {
                    lm.p4 = true;
                }
                break;

            default:
                break;
        }
    }
}
