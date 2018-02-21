using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QualityManager : MonoBehaviour 
{

    Resolution nativeRes;
    public Text debugtest;
	// Use this for initialization
	void Start () 
    {
        Application.targetFrameRate = 60;
        if (PlayerPrefs.GetInt("NativeRes", 0) > 0)
        {
            nativeRes.width = PlayerPrefs.GetInt("NativeResWidth");
            nativeRes.height = PlayerPrefs.GetInt("NativeResHeight");
        }
        else
        {
            nativeRes = Screen.currentResolution;
            PlayerPrefs.SetInt("NativeResWidth", nativeRes.width);
            PlayerPrefs.SetInt("NativeResHeight", nativeRes.height);
            PlayerPrefs.SetInt("NativeRes", 1);
        }



	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void setLowResolution()
    {
        Screen.SetResolution(1280,720, true);
        QualitySettings.SetQualityLevel((int)QualityLevel.Fast,true);
    }

    public void SetHighResolution()
    {
        Screen.SetResolution(nativeRes.width, nativeRes.height,true);
        QualitySettings.SetQualityLevel((int)QualityLevel.Beautiful,true);
    }
}
