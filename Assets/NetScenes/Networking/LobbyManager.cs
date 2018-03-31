using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;

public struct PlayerStruct
{
	public bool selected ;
	public string name;
	public string color;
}


public class LobbyManager : NetworkBehaviour
{

	[SyncVar]
	public PlayerData playerStructure1 = new PlayerData();
	[SyncVar]
	public PlayerData playerStructure2 = new PlayerData();
	[SyncVar]
	public PlayerData playerStructure3 = new PlayerData();
	[SyncVar]
	public PlayerData playerStructure4 = new PlayerData();
    [SyncVar]
    public int selectedBoard;

    //[SyncVar]
    public List<PlayerStruct> playerDataArray;

    //public PlayerSelection playerOptions;

    public bool isSelected = false;
    [SyncVar]
    public bool p1 = false;
    [SyncVar]
    public bool p2 = false;
    [SyncVar]
    public bool p3 = false;
    [SyncVar]
    public bool p4 = false;


    public static string tempName;
	public Netmanager ownManager;
    public static int noOfPlayers = 0;
    public int  isplayerReady = 0;
	public GameObject startGameBtn;
    public GameObject ConLostScreen;


	public Sprite[] boardImages;
	public string[] boardNames;
	public Image SelectedBoardImage;
	public Text BoardName;

	[SyncVar]
	public string roomName;

	[SyncVar(hook = "SetDatap1")]
	public string playerOneField = "";

	[SyncVar(hook = "SetDatap2")]
	public string playerTwoField = "";

	[SyncVar(hook = "SetDatap3")]
	public string playerThreeField = "";

	[SyncVar(hook = "SetDatap4")]
	public string playerFourField = "";

	public InputField P1Field, P2Field, P3Field, P4Field;
	public Text roomNameHeader, textData;

	public bool isReady = false;
    bool first = true;

    public GameObject loadingScreen;

	public static int JoinAmount;


    void SetDatap1(string name)
	{
		print ("THIS IS OVERRIDING SHIT1");
        if (P1Field.text == "" && name != "")
        {
            print("text" + P1Field.text);
            isplayerReady++;
        }
        else
        {
			if (name == "" && P1Field.text != "")
            {
                isplayerReady--;
            }
        }
       
		P1Field.text = name;
		playerStructure1.name = name;
        playerStructure1.color = P1Field.gameObject.GetComponent<GameDataHolder>().color;
        playerStructure1.selected = true;
        P1Field.transform.GetComponentInParent<GameDataHolder>().isSelected = true;

    }

	void SetDatap2(string name)
	{
		print ("THIS IS OVERRIDING SHIT2");
        if (P2Field.text == "" && name != "")
        {
            isplayerReady++;
        }
        else
        {
			if (name == "" && P2Field.text != "")
            {
                isplayerReady--;
            }
        }
		P2Field.text = name;
		playerStructure2.name = name;
        playerStructure2.color = P2Field.gameObject.GetComponent<GameDataHolder>().color;
        playerStructure2.selected = true;
        P2Field.transform.GetComponentInParent<GameDataHolder>().isSelected = true;
    }

	void SetDatap3(string name)
	{
        if (P3Field.text == "" && name != "")
        {
            isplayerReady++;
        }
        else
        {
			if (name == "" && P3Field.text != "")
            {
                isplayerReady--;
            }
        }
		P3Field.text = name;
		playerStructure3.name = name;
        playerStructure3.color = P3Field.gameObject.GetComponent<GameDataHolder>().color;
        playerStructure3.selected = true;
        P3Field.transform.GetComponentInParent<GameDataHolder>().isSelected = true;
    }

	void SetDatap4(string name)
	{
        if (P4Field.text == "" && name != "")
        {
            isplayerReady++;
        }
        else
        {
			if (name == "" && P4Field.text != "")
            {
                isplayerReady--;
            }
        }
		P4Field.text = name;
		playerStructure4.name = name;
        playerStructure4.color = P4Field.gameObject.GetComponent<GameDataHolder>().color;
        playerStructure4.selected = true;
        P4Field.transform.GetComponentInParent<GameDataHolder>().isSelected = true;
	}


	void Awake()
	{
        //roomName = DiscoverNetworks.Instance.broadcastData;
        noOfPlayers = 0;
        if(!NetworkTest.isLAN)
        InvokeRepeating("CheckForConnections", 0.5f, 1f);
	}

	// Use this for initialization
	void Start () 
    {
		//print("From lobby : " + SelectPlayField.whichBoard);
		playerDataArray = new List<PlayerStruct> ();

		P1Field = GameObject.Find("P1").GetComponent<InputField>();
		P2Field = GameObject.Find("P2").GetComponent<InputField>();
		P3Field = GameObject.Find("P3").GetComponent<InputField>();
		P4Field = GameObject.Find("P4").GetComponent<InputField>();

		//if (!isClient) 
		{
			roomName = tempName;
		}
            

		P1Field.text = playerOneField;
		P2Field.text = playerTwoField;
		P3Field.text = playerThreeField;
		P4Field.text = playerFourField;

        P1Field.GetComponentInParent<GameDataHolder>().isSelected = playerStructure1.selected;
        P2Field.GetComponentInParent<GameDataHolder>().isSelected = playerStructure2.selected;
        P3Field.GetComponentInParent<GameDataHolder>().isSelected = playerStructure3.selected;
        P4Field.GetComponentInParent<GameDataHolder>().isSelected = playerStructure4.selected;
        GameObject.Find("AudioManager").GetComponent<LudoLoader>().LevelLoadScreen = loadingScreen;


		//Display which board was chosen in the lobby scene
		if (!isServer) {
			BoardName.text = SelectPlayField.SelectedBoardName;
			print ("Selected board : " + SelectPlayField.SelectedBoardName.Length+"yyyy"+ BoardName.text+"KKKKK");
			if (SelectPlayField.SelectedBoardName == "Classic") {
				print ("00000");
				SelectedBoardImage.sprite = boardImages [0];
			}
			else if (SelectPlayField.SelectedBoardName == "Ruins") {
				print ("11111");
				SelectedBoardImage.sprite = boardImages [1];
			}
		}
		else
		{
			SelectedBoardImage.sprite = boardImages [SelectPlayField.whichBoard];
			BoardName.text = boardNames[SelectPlayField.whichBoard];
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (isplayerReady >= 2 && isplayerReady == noOfPlayers && isServer && !isReady)
        {   //Show start game button on the host.
            startGameBtn.SetActive(true);
            isReady = true;
        }
        else
        {
			if (isServer && isplayerReady < 2) {
				startGameBtn.SetActive (false);
				isReady = false;
			} 
			else
			{
				if (isServer && isplayerReady != noOfPlayers) {
					startGameBtn.SetActive (false);
					isReady = false;
				}
			}
			//isReady = false;
        }

	}

    void CheckForConnections()
    {
       if(Application.internetReachability == NetworkReachability.NotReachable && !NetworkTest.isLAN)
       {
           CancelInvoke();
           gameObject.SetActive(false);
           if (!NetworkTest.isLAN)
           {
               ConLostScreen.SetActive(true);

				CoinManager.AwardCoins (CoinManager.justDeductedCoins);		//if player dcs from lobby screen
               
				Invoke("LoadMainMenu", 1f);
				
               NetworkManager.singleton.StopMatchMaker();
               NetworkManager.singleton.StopClient();
               NetworkManager.singleton.StopHost();
               DiscoverNetworks.Instance.StopBroadcast();

           }

           Debug.Log("OnDisableCalled on LobbyManager");
        }
    }

	public void SelectPlayer(GameObject overlay)
	{
        if (overlay.transform.GetComponentInParent<GameDataHolder>().isSelected)
        {
            return;
        }
        switch (overlay.transform.GetComponentInParent<GameDataHolder>().playerValue)
        {
            case 1:
                if (!p1 && !isSelected)
                {
                    if (isServer)
                    {
                        p1 = true;
                    }
                    else
                    {
                        ownManager.CmdSetPlayer(overlay.transform.GetComponentInParent<GameDataHolder>().playerValue);
                    }

                    isSelected = true;
                    overlay.SetActive(false);
                }
                break;
            case 2:
                if (!p2 && !isSelected)
                {
                    if (isServer)
                    {
                        p2 = true;
                    }
                    else
                    {
                        ownManager.CmdSetPlayer(overlay.transform.GetComponentInParent<GameDataHolder>().playerValue);
                    }

                    isSelected = true;
                    overlay.SetActive(false);
                } 
                break;
            case 3:
                if (!p3 && !isSelected)
                {
                    if (isServer)
                    {
                        p3 = true;
                    }
                    else
                    {
                        ownManager.CmdSetPlayer(overlay.transform.GetComponentInParent<GameDataHolder>().playerValue);
                    }

                    isSelected = true;
                    overlay.SetActive(false);
                }
                break;
            case 4:
                if (!p4 && !isSelected)
                {
                    if (isServer)
                    {
                        p4 = true;
                    }
                    else
                    {
                        ownManager.CmdSetPlayer(overlay.transform.GetComponentInParent<GameDataHolder>().playerValue);
                    }

                    isSelected = true;
                    overlay.SetActive(false);
                }
                break;

            default:
                break;
        }
    }



    public void SetPlayerName(InputField player)
	{
        PlayerSelection.playerColor = player.gameObject.GetComponent<GameDataHolder>().color;
        switch (player.name)
		{
		case "P1":
                    if (isServer)
                    {
                        playerOneField = player.text;
                        if (first)
                        {
                            isplayerReady++;
                            first = false;
                        }
                        

                     }
                    else
                    {
                        ownManager.CmdSetData_1(player.text);
                    }
                    Debug.Log(player.text);
                
			break;

		case "P2":
                    if (isServer)
                    {
                        playerTwoField = player.text;
                        if (first)
                        {
                            isplayerReady++;
                            first = false;
                        }
                    }
                else
                    {
                        ownManager.CmdSetData_2(player.text);
                    }
			break;
		
		case "P3":
                    if (isServer)
                    {
                        playerThreeField = player.text;
                        if (first)
                        {
                            isplayerReady++;
                            first = false;
                        }
                    }
                    else
                    {
                        ownManager.CmdSetData_3(player.text);
                    }
			break;
		
		case "P4":
                    if (isServer)
                    {
                        playerFourField = player.text;
                        if (first)
                        {
                            isplayerReady++;
                            first = false;
                        }
                    }
                    else
                    {
                        ownManager.CmdSetData_4(player.text);
                    }
			break;			

		default:
			break;
		}
	}

    public void SetPlayers()
    {
		if (isServer)
        {
            int boardKey = SelectPlayField.whichBoard;
            RpcGetPlayerStructure(boardKey);
        }
    }

    [ClientRpc]
    public void RpcGetPlayerStructure(int boardKey)	//Based on the number of player add to array before hand and change using element values.
	{
		if (playerStructure1.selected) {
			//playerDataArray [0] = playerStructure1;
            PlayerSelection.playerInfo.Add(playerStructure1);
		}
		if (playerStructure2.selected) {
			//playerDataArray [1] = playerStructure2;
            PlayerSelection.playerInfo.Add(playerStructure2);
        }
		if (playerStructure3.selected) {
			//playerDataArray [2] = playerStructure3;
            PlayerSelection.playerInfo.Add(playerStructure3);
        }
		if (playerStructure4.selected) {
			//playerDataArray [3] = playerStructure4;
            PlayerSelection.playerInfo.Add(playerStructure4);
        }

        //PlayerSelection.playerInfo = playerDataArray;

        print(PlayerSelection.playerInfo.Count);

        SelectPlayField.whichBoard = boardKey;

        GoToScene();
	}

	void OnApplicationQuit()	//This should happen when the player quits the match as well.
	{
		//DiscoverNetworks.Instance.StopBroadcast ();
		if (!isClient) 
        {
			NetworkManager.singleton.StopHost ();
		} 
        else 
        {
			NetworkManager.singleton.StopClient ();
		}
        if (NetworkTest.isLAN)
        {
            DiscoverNetworks.Instance.StopBroadcast();
        }
        else
        {
            NetworkManager.singleton.StopMatchMaker();
        }
	}

    public void GoToScene()
    {
        // SceneManager.LoadScene("LudoLevel");
        if (isServer)
        {
            if (!NetworkTest.isLAN)
            {
                NetworkManager.singleton.matchMaker.SetMatchAttributes(NetworkManager.singleton.matchInfo.networkId, false, 0, OnHideMatch);
            }
            else
            {
                RpcShiftScenes(SelectPlayField.whichLevel);
            }
        }
    }

    public void OnHideMatch(bool success, string extendedInfo)
    {
        if (success)
        {
            RpcShiftScenes(SelectPlayField.whichLevel);
        }
        else
        {
            Debug.Log("network issues detected");
        }
    }

    [ClientRpc]
    public void RpcShiftScenes(string level)
    {
        Debug.Log(level);
        LudoLoader.instance.LevelLoaderCallNormal(level);
        //SceneManager.LoadScene(level);
    }

	bool serverstarted;

    private void OnDisable()
    {
//		ConLostScreen.SetActive(true);
//		Invoke("LoadMainMenu", 1f);
//		NetworkManager.singleton.StopMatchMaker();
//		NetworkManager.singleton.StopClient();
//		NetworkManager.singleton.StopHost();
//		DiscoverNetworks.Instance.StopBroadcast();
    }

	void OnEnable()
	{
		serverstarted = true;
	}

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void OnDestroy()
    {
        isSelected = false;
    }
    
}

