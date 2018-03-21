using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

	public GameObject[] items;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckIfCanPurchase(GameObject item)
	{
		Debug.Log ("Checking for purchase");

		AudioManager.Instance.UIClick();

		int currCoins = CoinManager.GetCurrentNumberOfCoins ();
		int itemPrice = item.GetComponent<ItemManager> ().GetPrice();

		if (itemPrice != -1 && itemPrice <= currCoins) 
		{
			UnlockItem (item, itemPrice);
		}

		else if (itemPrice == -1) 
		{
			Debug.Log ("Item has already been bought");
		}
		else if(itemPrice > currCoins)
		{
			Debug.Log ("Not enough coins");
		}
	}

	public void UnlockItem(GameObject item, int price)
	{
		//Do Something visual to denote purchase.
		item.GetComponent<ItemManager>().ItemWasBought();
		CoinManager.DeductCoins(price);
	}

	public void CanWeBuy()
	{
		foreach (var item in items) {
			item.GetComponent<ItemManager> ().CheckIfCanBuyItem ();
		}
	}
}
