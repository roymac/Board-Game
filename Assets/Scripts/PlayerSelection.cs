using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

	public static int playerSelected;

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
//		if (number > 2)
//		{
//			for (int i = 0; i < 4; i++) {
//				//names [i].SetActive (true);
//				//playerInfo.Add (playerData);
//				if (!UIScript.isVersusBot) {
//					overlays [i].SetActive (false);
//				}
//			}
//		}
//
//
//		for (int i = 0; i < 4; i++) {
//			playerInfo.Add (playerData);
//		}
//			
//      

	}

    public void SelectHumanPlayer(GameObject overlay)
    {
		print (UIScript.isVersusBot);

		if ((!UIScript.isVersusBot && (playerSelected<UIScript.numberOfPlayers)) || (UIScript.isVersusBot && playerSelected<1)) 
		{
			overlay.SetActive (false);
			isPlayerSelected = true;
			playerColor = overlay.GetComponentInParent<SetPlayerInfo> ().playerColor;

			overlay.GetComponentInParent<SetPlayerInfo> ().isSelected = true;
			playerSelected++;

			SetLockedPlayer ();
		}


		SetAIOpponentColor ();
	}


	void SetLockedPlayer()
	{

		if((playerSelected==1 && UIScript.isVersusBot) ||  (playerSelected==UIScript.numberOfPlayers && !UIScript.isVersusBot))
		{
			for (int i = 0; i < overlays.Length; i++)
			{
				if (!overlays[i].GetComponentInParent<SetPlayerInfo> ().isSelected)
				{
					overlays[i].transform.GetChild(0).GetComponent<Text>().text = "Locked";
				}
			}	
		}
	}

	void SetAIOpponentColor()
	{
		if (UIScript.numberOfPlayers < 4)
		{
			print (playerColor);
			if (playerColor == PawnColor.c_Green)
			{
				if (UIScript.isVersusBot)
				{
					if (UIScript.numberOfPlayers == 2) {
						overlays [0].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					//	playerSelected++;
					}
					else if(UIScript.numberOfPlayers == 3)
					{
						overlays [0].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
						overlays [1].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
						//playerSelected++;
					}
				}

				else if(UIScript.numberOfPlayers == 2 && !UIScript.isVersusBot)
				{
					overlays [0].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					playerSelected++;
					overlays [0].SetActive(false);
					SetLockedPlayer ();
				}

			}

			if (playerColor == PawnColor.c_Blue) 
			{
				if (UIScript.isVersusBot) 
				{
					if (UIScript.numberOfPlayers == 2)
					{
						overlays [2].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					}
					else if (UIScript.numberOfPlayers == 3)
					{
						overlays [2].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
						overlays [3].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					}
				}
				else if(UIScript.numberOfPlayers == 2 && !UIScript.isVersusBot)
				{
					overlays [2].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					overlays [2].SetActive(false);
					playerSelected++;
					SetLockedPlayer ();
				}

			}

			if (playerColor == PawnColor.c_Red)
			{
				if (UIScript.isVersusBot)
				{
					if (UIScript.numberOfPlayers == 2)
					{
						overlays [1].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					} 
					else if (UIScript.numberOfPlayers == 3)
					{
						overlays [1].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
						overlays [2].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					}
				}
				else if(UIScript.numberOfPlayers == 2 && !UIScript.isVersusBot)
				{
					overlays [1].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					overlays [1].SetActive(false);
					playerSelected++;
					SetLockedPlayer ();
				}

			}

			if (playerColor == PawnColor.c_Yellow)
			{
				//overlays [3].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
				if (UIScript.isVersusBot)
				{
					if (UIScript.numberOfPlayers == 2) {
						overlays [3].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					}
					else if (UIScript.numberOfPlayers == 3)
					{
						overlays [3].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
						overlays [0].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					}
				}
				else if(UIScript.numberOfPlayers == 2 && !UIScript.isVersusBot)
				{
					overlays [3].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
					overlays [3].SetActive(false);
					playerSelected++;
					SetLockedPlayer ();
				}

			}
		}

		if (UIScript.numberOfPlayers == 4 && UIScript.isVersusBot) {
			for (int i = 0; i < overlays.Length; i++) {
				overlays [i].GetComponentInParent<SetPlayerInfo> ().isSelected = true;
			}
		}


		if (!UIScript.isVersusBot) {
			playerColor = PawnColor.c_null;
		}

	}



}
