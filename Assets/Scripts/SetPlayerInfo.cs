using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerInfo : MonoBehaviour {

    public PawnColor playerColor;
    PlayerData data;
    public int index, defaultIndex;
	public bool isSelected = false;

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
		data.name = this.GetComponent<InputField>().text;

		if (UIScript.isVersusBot) {
			//PlayerSelection.playerName = this.GetComponent<InputField>().text;
			PlayerSelection.playerColor = playerColor;
		} else {
			isSelected = true;
		}

		
	}

    void OnDestroy()
    {
//		if (gameObject.activeSelf && PlayerSelection.playerInfo.Count>0 && index>=0)
//        {
//            PlayerSelection.playerInfo[index] = data;
//        }
		if (isSelected) {
			PlayerSelection.playerInfo.Add (data);
		}
    }
}
