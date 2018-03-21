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
    private void Awake()
    {
//		  PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//		   // enables saving game progress.
//		   .EnableSavedGames()
//		   // registers a callback to handle game invitations received while the game is not running.
//		    // .WithInvitationDelegate(< callback method >)
//		   // registers a callback for turn based match notifications received while the
//		   // game is not running.
//		   //  .WithMatchDelegate(< callback method >)
//		   // requests the email address of the player be available.
//		   // Will bring up a prompt for consent.
//		   .RequestEmail()
//		   // requests a server auth code be generated so it can be passed to an
//		   //  associated back end server application and exchanged for an OAuth token.
//		   .RequestServerAuthCode(false)
//		   // requests an ID token be generated.  This OAuth token can be used to
//		   //  identify the player to other services such as Firebase.
//		   .RequestIdToken()
//		   .Build();
//		
//     PlayGamesPlatform.InitializeInstance(config);

        //------------------ recommended for debugging:-------------------------------

        PlayGamesPlatform.DebugLogEnabled = true;
      
      ////------------------Activate the Google Play Games platform--------------------------
        PlayGamesPlatform.Activate();
      
        ConnectToGoogleService();
    }

    public void ConnectToGoogleService()
    {
		if (!IsConnectedtoGoogle) 
		{
			Social.localUser.Authenticate (
				(bool success) => {
					IsConnectedtoGoogle = success;
				});
		} 
    }
}
