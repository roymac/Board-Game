using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

	//Button m_Button;
	public ShopManager _sm;
	public UIScript _ui;

	public string placementId = "rewardedVideo";

	public string gameIDAndroid;
    public string iosGameID;

    public GameObject adOverlay;
    void Start ()
	{
        adOverlay.SetActive(false);

        //Advertisement.Initialize (gameID);
      
        //		m_Button = GetComponent<Button>();
        //		if (m_Button) m_Button.onClick.AddListener(ShowAd);
        //
        //		//---------- ONLY NECESSARY FOR ASSET PACKAGE INTEGRATION: ----------//
        //
       if (Advertisement.isSupported)
       {
           if (Application.platform == RuntimePlatform.Android)
           {
               Advertisement.Initialize(gameIDAndroid, true);
           }
           else if (Application.platform == RuntimePlatform.IPhonePlayer)
           {
               Advertisement.Initialize(iosGameID, true);
           }
       }
        
        		//-------------------------------------------------------------------//
        
    }

	void Update ()
	{
		//if (m_Button) m_Button.interactable = Advertisement.IsReady(placementId);
	}

	public void ShowAd ()
	{
		Debug.Log ("Is ad ready : " + Advertisement.IsReady("rewardedVideo"));

		if (Advertisement.IsReady ())
		{
			adOverlay.SetActive (true);
	        
			ShowOptions options = new ShowOptions ();
			options.resultCallback = HandleShowResult;
	        
			AudioManager.Instance.UIClick ();
	        
			Advertisement.Show (placementId, options);
		} 
		else
		{
			if(_ui!=null)
			{
				_ui.ShowAdAvailability (true);
			}
		}
    }

   void HandleShowResult(ShowResult result)
   {
       if (result == ShowResult.Finished)
       {
           Debug.Log("Video completed - Offer a reward to the player");
           CoinManager.AwardCoins(20);
           _sm.CanWeBuy();
  
       }
       else if (result == ShowResult.Skipped)
       {
           Debug.LogWarning("Video was skipped - Do NOT reward the player");
  
       }
       else if (result == ShowResult.Failed)
       {
           Debug.LogError("Video failed to show");
       }
  
       adOverlay.SetActive(false);
   }
}
