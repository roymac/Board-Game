using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SelectPlayField : MonoBehaviour {

    public GameObject[] boardImgOverlays;
    public static int whichBoard = 0;
    public LobbyManager lm;

    public List<string> levels;
    public static string whichLevel;

	// Use this for initialization
	void Start () 
    {
      //  whichBoard = 0;
        //whichLevel = levels[whichBoard];
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void SelectThisBoard(int thisBoard)
    {
   
        whichBoard = thisBoard;
        whichLevel = levels[whichBoard];

	    print("This board : " + whichLevel);

        //boardImgOverlays[thisBoard].SetActive(true);
        //if (thisBoard == 0)
        //{
        //    boardImgOverlays[0].SetActive(true);
        //    boardImgOverlays[1].SetActive(false);

        //}
        //else if (thisBoard == 1)
        //{
        //    boardImgOverlays[0].SetActive(false);
        //    boardImgOverlays[1].SetActive(true);
        //}
    }
}
