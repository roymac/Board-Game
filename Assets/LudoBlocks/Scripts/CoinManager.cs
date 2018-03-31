using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinManager : MonoBehaviour {

	public static Text CoinsText;

	public static int NumberOfCoins;

	//used to hold the amount deducted just in case it has to be given back. like for pooling ,
	//if the player joins, the coins are deducted, but if the match fails to start, then coins are given back.
	public static int justDeductedCoins;

	public AdManager _am;

    private void Start()
    {
        Init();

		//_am = GameObject.Find ("AudioManager").GetComponent<AdManager> ();
    }

	public void GoToAdManager()
	{
		_am.ShowAd ();
	}


    public static void Init()
    {
//		PlayerPrefs.DeleteAll();
		//AwardCoins(50);
        CoinsText = GameObject.Find ("NoOfCoins_txt").GetComponent<Text> ();	

		if (PlayerPrefs.HasKey ("NumberOfCoins") && CoinsText != null)
		{
			CoinsText.text = PlayerPrefs.GetInt ("NumberOfCoins").ToString ();
			NumberOfCoins = PlayerPrefs.GetInt ("NumberOfCoins");
		}
		else if(!PlayerPrefs.HasKey ("NumberOfCoins"))
		{
			AwardCoins (100);
		}
		//PlayerPrefs.DeleteAll();
	}

   	public static void AwardCoins(int coinsAwarded)
	{
		NumberOfCoins += coinsAwarded;
		PlayerPrefs.SetInt ("NumberOfCoins", NumberOfCoins);

		Debug.Log ("Awarded coins : " + coinsAwarded);
		Debug.Log ("Total coins : " + NumberOfCoins);

		UpdateCoinsText (NumberOfCoins.ToString());
	}

	public static int GetCurrentNumberOfCoins()
	{
		if (!PlayerPrefs.HasKey ("NumberOfCoins"))
		{
			return (-1);
		}

		return PlayerPrefs.GetInt ("NumberOfCoins");
	}

	public static void DeductCoins(int coinsDeducted)
	{
		int totalCoins = PlayerPrefs.GetInt ("NumberOfCoins");

		if (totalCoins >= coinsDeducted) {
			totalCoins -= coinsDeducted;
			NumberOfCoins = totalCoins;
			PlayerPrefs.SetInt ("NumberOfCoins", totalCoins);
			UpdateCoinsText (totalCoins.ToString());
		}
		else 
		{
			Debug.Log ("Not enough coins");
		}
	}

	public static void UpdateCoinsText(string coinAmnt)
	{
        if (CoinsText != null)
			CoinsText.text = coinAmnt;
	}

	public void AddCoins(int coins)		//only for test
	{
		AwardCoins (coins);
    }

}
