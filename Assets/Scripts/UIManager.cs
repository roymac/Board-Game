using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image LoadingScreen;
	// Use this for initialization
	void Start () 
    {
        if (PlayerSelection.playerColor == PawnColor.c_null)
        {
            CameraSelection = PassPlayCameraSelection;
        }

        RemoveLoadingScreen();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void ShowCameraSelection()
    {
        if (CameraSelection.activeInHierarchy)
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
        ui_s.GameOver(text);
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
        switch (turn)
        {
            case PawnColor.c_Blue:
                showTurn.text = (name == null || name == "") ? "Blues Turn" : name + " Turn";
                showTurn.color = Color.blue;
                break;
            case PawnColor.c_Red:
                showTurn.text = (name == null || name == "") ? "Reds Turn" : name + " Turn";
                showTurn.color = Color.red;
                break;
            case PawnColor.c_Yellow:
                showTurn.text = (name == null || name == "") ? "Yellows Turn" : name + " Turn";
                showTurn.color = Color.yellow;
                break;
            case PawnColor.c_Green:
                showTurn.text = (name == null || name == "") ? "Greens Turn" : name + " Turn";
                showTurn.color = Color.green;
                break;
        }
    }

    public void ShowDiceValues(PawnColor turn, string value)
    {
        showTurn.text = "Rolled :" + value;
        switch (turn)
        {
            case PawnColor.c_Blue:
                showTurn.color = Color.blue;
                break;
            case PawnColor.c_Red:
                showTurn.color = Color.red;
                break;
            case PawnColor.c_Yellow:
                showTurn.color = Color.yellow;
                break;
            case PawnColor.c_Green:
                showTurn.color = Color.green;
                break;
        }
    }

    public void RemoveLoadingScreen()
    {
        StartCoroutine(FadeAway(1f));
    }

    IEnumerator FadeAway(float FadeTime)
    {
        for (float i = 0; i < FadeTime; i+=Time.deltaTime)
        {
            Color col = LoadingScreen.color;
            Color lerpedColor = Color.Lerp(col, Color.clear, Time.deltaTime);
            LoadingScreen.color = lerpedColor;
            yield return null;
        }

        LoadingScreen.gameObject.SetActive(false);
    }
}
