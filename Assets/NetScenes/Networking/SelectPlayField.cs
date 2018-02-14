using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayField : MonoBehaviour {

    public GameObject[] boardImgOverlays;
    public static int whichBoard;
    public LobbyManager lm;

	// Use this for initialization
	void Start () 
    {
        whichBoard = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void SelectThisBoard(int thisBoard)
    {
        whichBoard = thisBoard;
                   
        //boardImgOverlays[thisBoard].SetActive(true);
        if (thisBoard == 0)
        {
            boardImgOverlays[0].SetActive(true);
            boardImgOverlays[1].SetActive(false);
           
        }
        else if (thisBoard == 1)
        {
            boardImgOverlays[0].SetActive(false);
            boardImgOverlays[1].SetActive(true);
        }
    }
}
