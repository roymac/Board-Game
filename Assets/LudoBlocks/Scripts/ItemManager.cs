using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {
	
	public bool hasBought;
	public int Price; 

	public GameObject overlay, detailsText, board, selectButton;
	//public string lockedColor, unclockedColor;

	public Text priceText;

	public string PlayerprefKey;

    public int boardIndex;


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
          //  if (overlay != null)
            {
                overlay.SetActive(false);
            }
//			if (detailsText != null) {
//				detailsText.SetActive (false);
//			}
			PlayerPrefs.SetInt (PlayerprefKey, 1);
		}
		hasBought = true;	
	}

	void CheckIfItemWasAlreadyBought()
	{
        Debug.Log("Checking if item was already bought");
        if (PlayerPrefs.GetInt(PlayerprefKey) == 1)
        {
            this.GetComponent<Image>().color = Color.white;
            if (overlay != null)
            {
                overlay.SetActive(false);
            }
//			 
            hasBought = true;
        }

        else if(this.gameObject.name!="Classic")
        {
            hasBought = false;
        }
	}

	public void CheckIfCanBuyItem()
	{
		Debug.Log ("Checking if item can be bought");

		Debug.Log ("No of Coins :" + PlayerPrefs.GetInt ("NumberOfCoins") + "           Price :" + Price);

        if (!hasBought)
        {

            if (priceText != null)
            {
                if (PlayerPrefs.GetInt("NumberOfCoins") >= Price)
                {
                    Debug.Log("buy");
                    priceText.color = Color.white;
                }
                else if (PlayerPrefs.GetInt("NumberOfCoins") < Price)
                {
                    Debug.Log("no buy");
                    priceText.color = Color.red;
                }
            }
        }
	}

    public void CanSelectThisBoard(bool visibility)
    {
        selectButton.SetActive(visibility);
    }

}
