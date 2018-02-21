using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	public bool hasBought;
	public int Price; 

	public GameObject overlay, detailsText, board;
	//public string lockedColor, unclockedColor;

	public Text priceText;

	public string PlayerprefKey;



	void Start()
	{
		CheckIfItemWasAlreadyBought ();
	}

	public int GetPrice()
	{
		if (hasBought) 
		{
			return (-1);
		}

		return Price;
	}

	void OnEnable()
	{
		CheckIfCanBuyItem ();
	}

	public void ItemWasBought()
	{
		Debug.Log ("Item was bought");
		this.GetComponent<Image> ().color = Color.white;
		if (overlay != null) 
		{
			overlay.SetActive (false);
//			if (detailsText != null) {
//				detailsText.SetActive (false);
//			}
			PlayerPrefs.SetInt (PlayerprefKey, 1);
		}
		hasBought = true;	
	}

	void CheckIfItemWasAlreadyBought()
	{
		if (PlayerPrefs.GetInt (PlayerprefKey) == 1)
		{
			this.GetComponent<Image> ().color = Color.white;
			overlay.SetActive (false);
//			if (detailsText != null) {
//				detailsText.SetActive (false);
//			}
			hasBought = true;
		}
	}

	void CheckIfCanBuyItem()
	{
		Debug.Log ("Checking if item can be bought");

		Debug.Log (PlayerPrefs.GetInt ("NumberOfCoins") + "            " + Price);


		if (priceText != null) 
		{
			if (PlayerPrefs.GetInt ("NumberOfCoins") > Price)
			{
				Debug.Log ("buy");
				priceText.color = Color.white;
			}
			else if(PlayerPrefs.GetInt ("NumberOfCoins") <= Price)
			{
				Debug.Log ("no buy");
				priceText.color = Color.red;
			}
		}
	}

}
