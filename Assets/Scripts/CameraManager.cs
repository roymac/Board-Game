using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{
    public GameObject GreenCamera, YellowCamera, BlueCamera, RedCamera, TopDownCamera,sideCamera1,sideCamera2;
	// Use this for initialization
	void Start () 
    {
        if (PlayerSelection.playerColor != PawnColor.c_null)
        {
            showCamera((int)PlayerSelection.playerColor);
        }
        else
        {
            PassPlayCameras(1);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void PassPlayCameras(int key)
    {
       
        GetComponent<UIManager>().ShowCameraSelection(0);
        switch (key)
        {
            case 0:
                TopDownCamera.SetActive(true);
                sideCamera1.SetActive(false);
                sideCamera2.SetActive(false);
                break;
            case 1:
                TopDownCamera.SetActive(false);
                sideCamera1.SetActive(true);
                sideCamera2.SetActive(false);
                break;
            case 2:
                TopDownCamera.SetActive(false);
                sideCamera1.SetActive(false);
                sideCamera2.SetActive(true);
                break;
        }
    }

    public void showCamera(int in_color)
    {
        PawnColor col = (PawnColor)in_color;
        GetComponent<UIManager>().ShowCameraSelection(0);
        switch (col)
        {
            case PawnColor.c_Blue:
                BlueCamera.SetActive(true);
                YellowCamera.SetActive(false);
                RedCamera.SetActive(false);
                GreenCamera.SetActive(false);
                TopDownCamera.SetActive(false);
                break;
            case PawnColor.c_Green:
                BlueCamera.SetActive(false);
                YellowCamera.SetActive(false);
                RedCamera.SetActive(false);
                GreenCamera.SetActive(true);
                TopDownCamera.SetActive(false);
                break;
            case PawnColor.c_Red:
                BlueCamera.SetActive(false);
                YellowCamera.SetActive(false);
                RedCamera.SetActive(true);
                GreenCamera.SetActive(false);
                TopDownCamera.SetActive(false);
                break;
            case PawnColor.c_Yellow:
                BlueCamera.SetActive(false);
                YellowCamera.SetActive(true);
                RedCamera.SetActive(false);
                GreenCamera.SetActive(false);
                TopDownCamera.SetActive(false);
                break;
            case PawnColor.c_null:
                BlueCamera.SetActive(false);
                YellowCamera.SetActive(false);
                RedCamera.SetActive(false);
                GreenCamera.SetActive(false);
                TopDownCamera.SetActive(true);
                break;
        }
    }
}
