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

    public List<GameObject> positionHolders;
    public List<Image> badges;
    public List<Text> Positiontexts;
    public GameObject EngGamePanel,endgameBG;
    public Button EndGameBackButton;
    public GameObject DisconnectionPanel;
    public GameObject flame_l,flame_r,banner_l,banner_r,flameholder_l,flameholder_r;
    public Image turnColorIndicator;

	public Color red,blue,green,yellow;
	// Use this for initialization
	void Start () 
    {
        turnColorIndicator = showTurn.transform.parent.GetChild(1).transform.GetChild(0).GetComponent<Image>();
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
        EngGamePanel.SetActive(true);
        if (text.Contains("connection"))
        {
            StartCoroutine(EngGameConnectionLostSequence());
        }
        else
        {
            for (int i = 0; i < FinishList.Count; i++)
            {
                positionHolders[i].SetActive(true);
            }
            StartCoroutine(EndGameSequence());
        }
//        ui_s.GameOver(Overtext);

    }

    IEnumerator EngGameConnectionLostSequence()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            Color col = endgameBG.GetComponent<Image>().color;
            col.a = i;
            endgameBG.GetComponent<Image>().color = col;
            yield return null;
        } 

        DisconnectionPanel.SetActive(true);
        EndGameBackButton.interactable = true;
    }

    IEnumerator EndGameSequence()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            Color col = endgameBG.GetComponent<Image>().color;
            col.a = i;
            endgameBG.GetComponent<Image>().color = col;
            flameholder_l.GetComponent<Image>().color = col;
            flameholder_r.GetComponent<Image>().color = col;
            yield return null;
        }
        EngGamePanel.GetComponent<Animator>().enabled = true;
        banner_l.SetActive(true);
        banner_r.SetActive(true);
        flame_l.SetActive(true);
        flame_r.SetActive(true);
        
        for (int i = 0; i < FinishList.Count; i++)
        {
            positionHolders[i].SetActive(true);
            for (float j = 0; j <= 1; j += Time.deltaTime)
            {
                Color col = positionHolders[i].GetComponent<Image>().color;
                col.a = j;
                positionHolders[i].GetComponent<Image>().color = col;
                badges[i].color = col;
                yield return null;
            }
            Positiontexts[i].text = FinishList[i];
            Positiontexts[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        EndGameBackButton.interactable = true;
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
			showTurn.color = blue;
            Blue.ShowSelectableAnimation();
            Red.StopSelectableAnimation();
            Yellow.StopSelectableAnimation();
            Green.StopSelectableAnimation();
			//diceValImage.GetComponent<Image> ().color = Color.blue; 
			rolledDiceNumber.color = Color.blue;
			break;
        	
			case PawnColor.c_Red:
			showTurn.text = (name == null || name == "") ? "Reds Turn" : name + "'s Turn";
            showTurn.color = red;
            Blue.StopSelectableAnimation();
            Red.ShowSelectableAnimation();
            Yellow.StopSelectableAnimation();
            Green.StopSelectableAnimation();
			//diceValImage.GetComponent<Image> ().color = Color.red; 
			rolledDiceNumber.color = Color.red;
            break;
       		
			case PawnColor.c_Yellow:
			showTurn.text = (name == null || name == "") ? "Yellows Turn" : name + "'s Turn";
			showTurn.color = yellow;
            Blue.StopSelectableAnimation();
            Red.StopSelectableAnimation();
            Yellow.ShowSelectableAnimation();
            Green.StopSelectableAnimation();
			//diceValImage.GetComponent<Image> ().color = Color.yellow; 
			rolledDiceNumber.color = Color.yellow;
            break;
      		
			case PawnColor.c_Green:
			showTurn.text = (name == null || name == "") ? "Greens Turn" : name + "'s Turn";
			showTurn.color = green;
            Blue.StopSelectableAnimation();
            Red.StopSelectableAnimation();
            Yellow.StopSelectableAnimation();
            Green.ShowSelectableAnimation();
			//diceValImage.GetComponent<Image> ().color = Color.green;
			rolledDiceNumber.color = Color.green;
            break;
        }

        turnColorIndicator.color = rolledDiceNumber.color;
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
