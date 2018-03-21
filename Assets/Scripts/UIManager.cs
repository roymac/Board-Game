using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class UIManager : MonoBehaviour 
{
    public Button SpinButton;
    public Text showTurn;
    public UIScript ui_s;
    public GameObject CameraSelection;
    public GameObject PassPlayCameraSelection;
    public GameObject CameraSelectionButton;
    public BoxCollider CentralCollider;
    public ParticleSystem diceParticle;
    public GameObject DiceFeedbackAnimationObject;
    public GameObject LoadingScreen;

    public StartPointVisuals Red, Blue, Green, Yellow;
    public Sprite[] diceValues;
    public GameObject diceValImage;
    public List<string> FinishList;

	public Text rolledDiceNumber;

	// Use this for initialization
	void Start () 
    {
        if (PlayerSelection.playerColor == PawnColor.c_null)
        {
            CameraSelection = PassPlayCameraSelection;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void ShowCameraSelection(int key)
    {
        if (key==0)
        {
            CameraSelection.SetActive(false);
            CameraSelectionButton.SetActive(true);
        }
        else
        {
            CameraSelection.SetActive(true);
            CameraSelectionButton.SetActive(false);
        }
    }

    public void OnGameOver(string text)
    {
		AnalyticsResult result = Analytics.CustomEvent ("GameEnd");

		print ("game end analytics result : " + result);
        string Overtext = "Player Positions are:\n";
        if (text.Contains("connection"))
        {
            Overtext = text;
        }
        else
        {
            for (int i = 0; i < FinishList.Count; i++)
            {
                string positiontext = " ";
                switch (i)
                {
                    case 0:
                        positiontext = "First";
                        break; 
                    case 1:
                        positiontext = "Second";
                        break;
                    case 2:
                        positiontext = "Third";
                        break;
                    case 4:
                        positiontext = "Fourth";
                        break;

                }
                Overtext += FinishList[i] + " " + positiontext + "\n";
            }
        }
        ui_s.GameOver(Overtext);

    }

    public void ActivateSpinButton()
    {
        //SpinButton.interactable = true;
        CentralCollider.enabled = true;
        CentralCollider.gameObject.GetComponent<DiceRollCollisionController>().AnimateDiceHolder();
    }

    public void DeactivateSpinButton()
    {
        //SpinButton.interactable = false;
        CentralCollider.enabled = false;
    }

    public void ShowCurrentTurn(PawnColor turn, string name)
    {
		//HideDiceImage ();
		rolledDiceNumber.gameObject.SetActive(false);
        switch (turn)
        {
            case PawnColor.c_Blue:
            showTurn.text = (name == null || name == "") ? "Blues Turn" : name + "'s Turn";
            showTurn.color = Color.blue;
            Blue.ShowSelectableAnimation();
            Red.StopSelectableAnimation();
            Yellow.StopSelectableAnimation();
            Green.StopSelectableAnimation();
			//diceValImage.GetComponent<Image> ().color = Color.blue; 
			rolledDiceNumber.color = Color.blue;
			break;
        	
			case PawnColor.c_Red:
			showTurn.text = (name == null || name == "") ? "Reds Turn" : name + "'s Turn";
            showTurn.color = Color.red;
            Blue.StopSelectableAnimation();
            Red.ShowSelectableAnimation();
            Yellow.StopSelectableAnimation();
            Green.StopSelectableAnimation();
			//diceValImage.GetComponent<Image> ().color = Color.red; 
			rolledDiceNumber.color = Color.red;
            break;
       		
			case PawnColor.c_Yellow:
			showTurn.text = (name == null || name == "") ? "Yellows Turn" : name + "'s Turn";
            showTurn.color = Color.yellow;
            Blue.StopSelectableAnimation();
            Red.StopSelectableAnimation();
            Yellow.ShowSelectableAnimation();
            Green.StopSelectableAnimation();
			//diceValImage.GetComponent<Image> ().color = Color.yellow; 
			rolledDiceNumber.color = Color.yellow;
            break;
      		
			case PawnColor.c_Green:
			showTurn.text = (name == null || name == "") ? "Greens Turn" : name + "'s Turn";
            showTurn.color = Color.green;
            Blue.StopSelectableAnimation();
            Red.StopSelectableAnimation();
            Yellow.StopSelectableAnimation();
            Green.ShowSelectableAnimation();
			//diceValImage.GetComponent<Image> ().color = Color.green;
			rolledDiceNumber.color = Color.green;
            break;
        }
    }

    public void HideDiceImage()
    {
        diceValImage.SetActive(false);
    }

    public void ShowDiceValues(PawnColor turn, string value)
    {
		rolledDiceNumber.gameObject.SetActive (true);
		rolledDiceNumber.text = value;


        //showTurn.text = "Rolled :" + value;
        //diceValImage.SetActive(true);
        //diceValImage.GetComponent<Image>().sprite = diceValues[int.Parse(value)];
//
//
//        switch (turn)
//        {
//		case PawnColor.c_Blue:
//			showTurn.color = Color.blue;
//			diceValImage.GetComponent<Image> ().color = Color.blue; 
//                break;
//            case PawnColor.c_Red:
//                showTurn.color = Color.red;
//                break;
//            case PawnColor.c_Yellow:
//                showTurn.color = Color.yellow;
//                break;
//            case PawnColor.c_Green:
//                showTurn.color = Color.green;
//                break;
//        }
    }

    public void RemoveLoadingScreen()
    {
        LoadingScreen.GetComponent<LoaderScreenScript>().StopAllCoroutines();
        LoadingScreen.SetActive(false);
    }
}
