using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	public static string tempName;
	public Netmanager ownManager;
	public int noOfPlayers = 0, isplayerReady = 0;
	public GameObject startGameBtn;


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


	void SetDatap1(string name)
	{
		P1Field.text = name;
		playerStructure1.name = name;
        playerStructure1.color = P1Field.gameObject.GetComponent<GameDataHolder>().color;
        playerStructure1.selected = true;
        P1Field.transform.GetComponentInParent<GameDataHolder>().isSelected = true;
		isplayerReady++;
	}

	void SetDatap2(string name)
	{
		P2Field.text = name;
		playerStructure2.name = name;
        playerStructure2.color = P2Field.gameObject.GetComponent<GameDataHolder>().color;
        playerStructure2.selected = true;
        P2Field.transform.GetComponentInParent<GameDataHolder>().isSelected = true;
		isplayerReady++;
	}

	void SetDatap3(string name)
	{
		P3Field.text = name;
		playerStructure3.name = name;
        playerStructure3.color = P3Field.gameObject.GetComponent<GameDataHolder>().color;
        playerStructure3.selected = true;
        P3Field.transform.GetComponentInParent<GameDataHolder>().isSelected = true;
		isplayerReady++;
	}

	void SetDatap4(string name)
	{
		P4Field.text = name;
		playerStructure4.name = name;
        playerStructure4.color = P4Field.gameObject.GetComponent<GameDataHolder>().color;
        playerStructure4.selected = true;
        P4Field.transform.GetComponentInParent<GameDataHolder>().isSelected = true;
		isplayerReady++;
	}


	void Awake()
	{
		//roomName = DiscoverNetworks.Instance.broadcastData;

	}

	// Use this for initialization
	void Start () 
    {
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isplayerReady >= 2 && isServer && !isReady) {	//Show start game button on the host.
			startGameBtn.SetActive (true);
			isReady = true;
		}

	}




	public void SelectPlayer(GameObject overlay)
	{
        if (overlay.transform.GetComponentInParent<GameDataHolder>().isSelected)
        {
            return;
        }
		if (!isSelected) 
        {
			overlay.SetActive (false);
           	isSelected = true;
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

			} 
			else 
			{
				ownManager.CmdSetData_1 (player.text);
			}
			Debug.Log (player.text);
			break;

		case "P2":

			if (isServer) 
			{
				playerTwoField = player.text;
			} 
			else 
			{
				ownManager.CmdSetData_2 (player.text);
			}
			break;
		
		case "P3":
			if (isServer) 
			{
				playerThreeField = player.text;
			} 
			else 
			{
				ownManager.CmdSetData_3 (player.text);
			}
			break;
		
		case "P4":
			if (isServer) 
			{
				playerFourField = player.text;
			} 
			else 
			{
				ownManager.CmdSetData_4 (player.text);
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
		if (!isClient) {
			NetworkManager.singleton.StopHost ();
		} else {
			NetworkManager.singleton.StopClient ();
		}
	}

    public void GoToScene()
    {
        // SceneManager.LoadScene("LudoLevel");
        if (isServer)
        {
            RpcShiftScenes();
        }
    }

    [ClientRpc]
    public void RpcShiftScenes()
    {
        Debug.Log(PlayerSelection.playerColor);
        SceneManager.LoadScene("LudoLevel");
    }
}

