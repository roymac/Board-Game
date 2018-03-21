using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class GameDataHolder : MonoBehaviour {

    public string name;
	public string ip;
    public MatchInfoSnapshot matchDetails;

	public PawnColor color;
    public bool isSelected;

    public int playerValue;

	void OnDestroy()
	{
		if (SceneManager.GetActiveScene ().name == "LobbyScene") {
			isSelected = false;
			color = PawnColor.c_null;
			//playerValue = -100;
		}
	}
}
