using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerInfo : MonoBehaviour {

    public PawnColor playerColor;
    PlayerData data;
    public int index;
    //public Dropdown colorSelect;

	// Use this for initialization
	void Start () 
    {
            data.color = playerColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetPlayerData(int index)
	{
        print(UIScript.isVersusBot);
        if (!UIScript.isVersusBot)
        {
            data.name = this.GetComponent<InputField>().text;
        }
        else
        {
            PlayerSelection.playerName = this.GetComponent<InputField>().text;
            PlayerSelection.playerColor = playerColor;
        }

		
	}

    void OnDestroy()
    {
        if (gameObject.activeSelf && PlayerSelection.playerInfo.Count>0)
        {
            PlayerSelection.playerInfo[index] = data;
        }
    }
}
