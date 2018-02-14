using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct PlayerData
{
    public string name;
    public PawnColor color;
    public bool selected;
}

public class PlayerSelection : MonoBehaviour 
{


    public static PlayerData playerData = new PlayerData();

    //public UIScript ui_script;
    public GameObject[] names;
    public GameObject[] overlays;

    public static List<PlayerData> playerInfo;

    public static PawnColor playerColor = PawnColor.c_null;
    public static string playerName;

    public bool isPlayerSelected;
    public static bool isNetworkedGame = false;

	// Use this for initialization
	void Start () {
        playerInfo = new List<PlayerData>();

        if (!isNetworkedGame)
        {
            SetNumberOfPlayers(UIScript.numberOfPlayers);
        }
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetNumberOfPlayers(int number)
	{
        for (int i = 0; i < number; i++)
        {
            names[i].SetActive(true);
            playerInfo.Add(playerData);
            if (UIScript.isVersusBot)
            {
                overlays[i].SetActive(true);
            }
        }
      

		//print (playerInfo.Count);
	}

    public void SelectHumanPlayer(GameObject overlay)
    {
        if (!isPlayerSelected)
        {
            overlay.SetActive(false);
            isPlayerSelected = true;
            playerColor = overlay.GetComponentInParent<SetPlayerInfo>().playerColor;
        }
    }
}
