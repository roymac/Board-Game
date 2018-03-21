﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateScreens : MonoBehaviour {

    public GameObject[] toBeActivated;
    public GameObject[] toBeDeactivated;

    public LobbyManager lm;
	public PlayerSelection ps;

	// Use this for initialization
	void Start () {
        //StartCoroutine(CheckForLobbyManager());
		ps = this.GetComponent<PlayerSelection>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator CheckForLobbyManager()
    {
        while (lm == null)
        {
            lm = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
            yield return null;
            
        }
    }

    public void GoBackScreen()
    {
		AudioManager.Instance.UIClick();
        PlayerSelection.isNetworkedGame = false;

        PlayerSelection.playerColor = PawnColor.c_null;

        //PlayerSelection.playerData = null;
        if (PlayerSelection.playerInfo != null)
        {
            PlayerSelection.playerInfo.Clear();
        }

        PlayerSelection.playerName = "";

        //ClearSelectStatus();

        SceneManager.LoadScene("LudoMenu");

       // ClearSelectStatus();
		if (ps != null) {
			PlayerSelection.playerSelected = 0;
		}
           
    }

    public void ClearSelectStatus()
    {
        
        if (lm.P1Field != null)
        {
            print("ShouldbeCalled");
            lm.P1Field.GetComponent<GameDataHolder>().isSelected = false;
        }
        if (lm.P2Field != null)
        {
            print("ShouldbeCalled");
            lm.P2Field.GetComponent<GameDataHolder>().isSelected = false;
        }
        if (lm.P3Field != null)
        {
            print("ShouldbeCalled");
            lm.P3Field.GetComponent<GameDataHolder>().isSelected = false;
        }
        if (lm.P4Field != null)
        {
            print("ShouldbeCalled");
            lm.P4Field.GetComponent<GameDataHolder>().isSelected = false;
        }
    }


}