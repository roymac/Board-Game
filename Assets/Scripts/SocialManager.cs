using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;

public class SocialManager : MonoBehaviour {

    public static bool IsConnectedtoGoogle = false;
    //public static SocialManager Instance = null;
    public Text debugtext;

	public GameObject[] allBoards;
	public GameObject loadSaveScreen;

    private void Awake()
    {
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()
//	  //--------enables saving game progress--------------
	   .EnableSavedGames().Build();
//		
     	PlayGamesPlatform.InitializeInstance(config);

        //------------------ recommended for debugging:-------------------------------

        PlayGamesPlatform.DebugLogEnabled = true;
      
      ////------------------Activate the Google Play Games platform--------------------------
        PlayGamesPlatform.Activate();
      
        ConnectToGoogleService();
		//LoadAllData ();
    }

    public void ConnectToGoogleService()
    {
		if (!IsConnectedtoGoogle) 
		{
			Social.localUser.Authenticate (
				(bool success) => {
					IsConnectedtoGoogle = success;
					//LoadAllData();
			});
			
			
		} 

		if (!PlayerPrefs.HasKey ("JustInstalled")) {
			Invoke ("LoadAllData", 3f);
		}

    }



	public void LoadAllData()
	{
		loadSaveScreen.SetActive(true);
		Invoke ("LoadComplete", 6f);
		for (int i = 0; i < allBoards.Length; i++)
		{
			allBoards [i].GetComponent<GPG_CloudSaveSystem> ().StartLoadingData ();	
		}

	}

	void LoadComplete()
	{
		//this.gameObject.SetActive (false);
		loadSaveScreen.SetActive(false);
	}

}
