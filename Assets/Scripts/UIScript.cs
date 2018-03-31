using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using UnityEngine.Analytics;

public class UIScript : MonoBehaviour
{
    public Animator mainMenuAnim, settingsAnimator, flameAnimator_l, flameAnimator_r;
	public GameObject pauseMenu, mainMenu, gameoverMenu, noofPlayerScreen, playerSelectScene, gameModeScreen, MultiplayerMode, OfflineOnlineMode, themeSelection, creditsScreen, selectPlayerErrorMsg, quitGameBox;
    public Text GameOverText;
    public static int numberOfPlayers = 0;
    public static bool isVersusBot = false;
	public static bool isOnline = false, isSP = false;
	public SocialManager sm;
	public Text debugtext;
	public Sprite muteAudioImg, unmuteAudioImg, lowGraphicImg, highGraphicImg, vibrateOnImg, vibrateOffImg;
	public Button audioBtn, graphicsBtn, vibrateBtn;
	public string subject, body;
    public GameObject loadingScreen;

	public bool showQuit = false;
	public static bool vibrate = true;


	bool muteAudio = false, highGraphic = true; 
	string muteSetting;

	public QualityManager _qm;

	public GameObject internetConn,ConLostScreen;

	public int playercount;

	public GameObject adAvailableBox;

	public ToastMessage _tm;

	void Awake()
	{
		
	}

	// Use this for initialization
    void Start()
    {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		if (SceneManager.GetActiveScene ().name == "Classic")
        {
		    AudioManager.Instance.songNumber = 1;
		}
        else if(SceneManager.GetActiveScene().name == "Ruins")
        {
            AudioManager.Instance.songNumber = 2;
        }

		AudioManager.Instance.PlayBGMusic (AudioManager.Instance.songNumber);

        if(mainMenu != null)
           // mainMenu.SetActive(true);

        if (MultiplayerMode != null)
            MultiplayerMode.SetActive(true);

		GetAudioAndGraphicSettingOnStart ();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown ("Escape")) 
		{
			Debug.Log ("Quit game");
			if (!showQuit) {
				ShowQuitDialog (true);		
				showQuit = true;
			}
			//QuitGame ();
		}
    }

	public void ShowAdAvailability(bool visibility)
	{
		adAvailableBox.SetActive (visibility);
	}

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
    }

    public static void StartGame()
    {
        //SceneManager.LoadScene(SelectPlayField.whichLevel);
        LudoLoader.instance.LevelLoaderCall(SelectPlayField.whichLevel);
    }

    public void StartTheGame()
    {
		if (isVersusBot)
		{
			if (PlayerSelection.playerColor != PawnColor.c_null)
			{
				//SceneManager.LoadScene (SelectPlayField.whichLevel);
                LudoLoader.instance.LevelLoaderCall(SelectPlayField.whichLevel);
			}
			else
			{
				ShowPlayerSelectionError ();
			}
		} 
		else
		{
			//SceneManager.LoadScene (SelectPlayField.whichLevel);
			if (UIScript.numberOfPlayers == PlayerSelection.playerSelected) {
				LudoLoader.instance.LevelLoaderCall (SelectPlayField.whichLevel);
			}
		}
    }

	void ShowPlayerSelectionError()
	{
		selectPlayerErrorMsg.SetActive (true);
		Invoke ("HidePlayerSelectionError", 3f);
	}

	void HidePlayerSelectionError()
	{
		selectPlayerErrorMsg.SetActive (false);
	}

	public void ExitGame()
    {
       // DiscoverNetworks.Instance.StopBroadcast();
       // NetworkManager.singleton.StopClient();
		AudioManager.Instance.songNumber = 0;
		PlayerSelection.playerInfo.Clear();
		PlayerSelection.playerColor = PawnColor.c_null;
		PlayerSelection.playerSelected = 0;
        SceneManager.LoadScene("LudoMenu");
    }
    
    public void ShowThemeSelectionAndShopScreen()
    {
        mainMenu.SetActive(false);
        //playerSelectScene.SetActive(false);
        noofPlayerScreen.SetActive(false);
        themeSelection.SetActive(true);
    }
    
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
    }

	public void ShowQuitDialog(bool visibility)
	{
		quitGameBox.SetActive (visibility);
		if (visibility == false) {
			showQuit = false;
		}
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver(string in_GameOverText)
    {
        //mainMenu.SetActive(false);
        //pauseMenu.SetActive(false);
        gameoverMenu.SetActive(true);
        GameOverText.text = in_GameOverText;
    }

    public void OpenPlayerNumberScreen( bool isSP)
    {
        noofPlayerScreen.SetActive(true);
        mainMenu.SetActive(false);
        gameModeScreen.SetActive(false);
		flameAnimator_l.GetComponent<Animator> ().enabled = false;
		flameAnimator_r.GetComponent<Animator> ().enabled = false;
        isVersusBot = isSP;
        isSP = true;

		AudioManager.Instance.UIClick();
		print ("this is single player : " + isSP);
    }

    public void SelectNumberOfPlayer(int playerNumber)
    {
        numberOfPlayers = playerNumber;
		AudioManager.Instance.UIClick();
    }

    public void OpenPlayerSelectionScreen()
    {
		AudioManager.Instance.UIClick();

		AnalyticsResult result = Analytics.CustomEvent ("SelectedBoard", new Dictionary<string, object>			//log analytics event for board selection
			{
				{"SelectedLevel", SelectPlayField.whichLevel}	
			});
		print ("Board analytics result : " + result);


		print(isVersusBot);
      	playerSelectScene.SetActive(true);
        
        noofPlayerScreen.SetActive(false);
        themeSelection.SetActive(false);
    }

    public void OpenGamemodeScreen()
    {
        mainMenu.SetActive(false);
        gameModeScreen.SetActive(true);
    }

	public void ShowHotspotText(bool isLan)
	{
		if (isLan) {
			internetConn.SetActive (true);
		}
	}

	public void OpenMultiplayerModeScreen(bool isLAN)
    {
		onPressOkay ();
		isSP = false;
		PlayerSelection.isNetworkedGame = true;
		NetworkTest.isLAN = isLAN;

		AudioManager.Instance.UIClick ();
		SceneManager.LoadScene ("Main");

//		if (isLAN) {
//			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
//				isSP = false;
//				PlayerSelection.isNetworkedGame = true;
//				NetworkTest.isLAN = isLAN;
//
//				AudioManager.Instance.UIClick ();
//				SceneManager.LoadScene ("Main");
//			} else {
//				internetConn.SetActive (true);
//			}
//		}
//		else 
//		{
//			isSP = false;
//			PlayerSelection.isNetworkedGame = true;
//			NetworkTest.isLAN = isLAN;
//
//			AudioManager.Instance.UIClick ();
//			SceneManager.LoadScene ("Main");
//		}
        
    }

	public void OnAddPlayerbackend()
	{
		playercount++;
	}

	public void OnRemovePlayerbackend()
	{
		playercount--;
		print ("Playercount" + playercount);

		if(playercount == 0)
		{
			CoinManager.AwardCoins (CoinManager.justDeductedCoins);
			ConLostScreen.SetActive(true);
			Invoke("LoadMainMenu", 1f);
			NetworkManager.singleton.StopMatchMaker();
			NetworkManager.singleton.StopClient();
			NetworkManager.singleton.StopHost();
			DiscoverNetworks.Instance.StopBroadcast();
		}
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void onPressOkay()
	{
		internetConn.SetActive(false);
	}
    
//    public void SetNetworkMode(bool value)
//    {
//        isSP = false;
//        isOnline = value;
//        //MultiplayerMode.SetActive(false);
//        //OfflineOnlineMode.SetActive(true);
//    }

    public void ShowAchievements()
    {
		AudioManager.Instance.UIClick();
        //debugtext.text = SocialManager.IsConnectedtoGoogle.ToString();
      //  sm.ConnectToGoogleService();

        if (Social.localUser.authenticated)
        {
            //print(SocialManager.IsConnectedtoGoogle.ToString());
            Social.ShowAchievementsUI();
        }
        else
        {
            sm.ConnectToGoogleService();
            //print("has Authenticated? : " + SocialManager.IsConnectedtoGoogle.ToString());
            //Not connected UI text
        }
    }

	public void GetAudioAndGraphicSettingOnStart()
	{
		muteAudio = !(PlayerPrefs.GetInt ("MuteAudio") == 1);
		if (audioBtn != null) {
			AudioSetting ();
		}

		highGraphic = !(PlayerPrefs.GetInt ("HighGraphic") == 1);
		if (graphicsBtn != null) {
			ApplyGraphicSetting ();
		}

		vibrate = !(PlayerPrefs.GetInt ("Vibrate") == 1);
		if (vibrateBtn != null) {
			SetVibrateSetting ();
		}
	}


	public void AudioSetting()
	{
		print ("Audio settings" + muteAudio);
		muteAudio = !muteAudio;

		AudioManager.mute = muteAudio;
		AudioManager.Instance.MuteAudioSources (AudioManager.mute);

		if (muteAudio) 
		{
			audioBtn.GetComponent<Image> ().sprite = muteAudioImg;
			_tm.showToastOnUiThread ("Sound Off");
			PlayerPrefs.SetInt ("MuteAudio", 1);
		}
		else if (!muteAudio)
		{
			//AudioManager.Instance.UIClick();
			audioBtn.GetComponent<Image> ().sprite = unmuteAudioImg;
			_tm.showToastOnUiThread ("Sound On");
			PlayerPrefs.SetInt("MuteAudio", 0);
			//AudioManager.Instance.MuteAudioSources (false);
		}

	
	}

	public void SetVibrateSetting()
	{
		vibrate = !vibrate;

		if (vibrate)
		{
			vibrateBtn.GetComponent<Image> ().sprite = vibrateOnImg;
			_tm.showToastOnUiThread ("Vibration On");
			PlayerPrefs.SetInt ("Vibrate", 1);
		}
		else if (!vibrate)
		{
			vibrateBtn.GetComponent<Image> ().sprite = vibrateOffImg;
			_tm.showToastOnUiThread ("Vibration Off");
			PlayerPrefs.SetInt ("Vibrate", 0);
		}
	}


	public void shareText()
	{
		AudioManager.Instance.UIClick();
		//execute the below lines if being run on a Android device
		#if UNITY_ANDROID
		//Reference of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		//Reference of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		//start the activity by sending the intent data
		currentActivity.Call ("startActivity", intentObject);
		#endif

	}

	//public void CheckIfInternet()
	//{
	//	if (noConnectionText != null)
	//	{
	//		noConnectionText.gameObject.SetActive (true);
	//		Invoke ("HideMessage", 3f);
	//	}
	//}

	//void HideMessage()
	//{
	//	noConnectionText.gameObject.SetActive (false);
	//}

	public void ApplyGraphicSetting()
	{
		highGraphic = !highGraphic;

		if (highGraphic) 
		{
			graphicsBtn.GetComponent<Image> ().sprite = highGraphicImg;
			PlayerPrefs.SetInt ("HighGraphic", 1);
			_tm.showToastOnUiThread ("High Graphics");
			_qm.SetHighResolution ();		
		}
		else
		{
			graphicsBtn.GetComponent<Image> ().sprite = lowGraphicImg;
			PlayerPrefs.SetInt ("HighGraphic", 0);
			_tm.showToastOnUiThread ("Low Graphics");
			_qm.setLowResolution ();
		}
		AudioManager.Instance.UIClick();
	}

	public void ShowCredits(bool show)
	{
		creditsScreen.SetActive (show);
		AudioManager.Instance.UIClick();
	}

}
