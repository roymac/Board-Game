using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBack : MonoBehaviour {

    public GameObject[] toBeActivated;
    public GameObject[] toBeDeactivated;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoBackScreen()
    {
        PlayerSelection.isNetworkedGame = false;
        PlayerSelection.playerColor = PawnColor.c_null;
        //PlayerSelection.playerData = null;
        PlayerSelection.playerInfo.Clear();
        PlayerSelection.playerName = "";

        SceneManager.LoadScene("LudoMenu");
    }


}
