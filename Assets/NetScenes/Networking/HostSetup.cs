using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostSetup : MonoBehaviour {

	public string selectedCoinsPerRoom;
	public string roomName;
	public string selectedBoard;

	public InputField nameField;

	public Sprite selected, notSelected;
	public Image[] coinSelect;

	void Start()
	{
		SetBoardName ();
	}

	public void SetRoomName()
	{
		roomName = nameField.text;
	}

	public void SetBoardName()
	{
		selectedBoard = SelectPlayField.whichLevel;
	}


	public void SetCoinValue(Text value)
	{
		//string[] temp = value.text.Split ();
		for (int i = 0; i < coinSelect.Length; i++) {
			coinSelect [i].sprite = notSelected;
		}
		print("Coin text value " + value.text);
		value.transform.GetComponentInParent<Image> ().sprite = selected;
		selectedCoinsPerRoom = (value.text); //int.Parse(temp [0]);	
	}

	public void ChangeSprite(Image img)
	{
		img.sprite = selected;
	}

}
