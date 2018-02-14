using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointVisuals : MonoBehaviour 
{
    public Material mat;
    public PawnColor col;
    public float animespeed = 3;
    public bool canChangeMat;
	// Use this for initialization
	void Start () 
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.SetFloat("_EmissionStrength", 10);
        switch (col)
        {
            case PawnColor.c_Blue:
                EventManager.UnlockBluePawns += ShowUnlockAnimation;
                break;
            case PawnColor.c_Red:
                EventManager.UnlockRedPawns += ShowUnlockAnimation;
                break;
            case PawnColor.c_Yellow:
                EventManager.UnlockYellowPawns += ShowUnlockAnimation;
                break;
            case PawnColor.c_Green:
                EventManager.UnlockGreenPawns += ShowUnlockAnimation;
                break;
        }
        EventManager.onSelected += OnPlayerSelect;
	}

    void OnDestroy()
    {
        switch (col)
        {
            case PawnColor.c_Blue:
                EventManager.UnlockBluePawns -= ShowUnlockAnimation;
                break;
            case PawnColor.c_Red:
                EventManager.UnlockRedPawns -= ShowUnlockAnimation;
                break;
            case PawnColor.c_Yellow:
                EventManager.UnlockYellowPawns -= ShowUnlockAnimation;
                break;
            case PawnColor.c_Green:
                EventManager.UnlockGreenPawns -= ShowUnlockAnimation;
                break;
        }
        EventManager.onSelected -= OnPlayerSelect;
    }

    void ShowUnlockAnimation(int num)
    {
        if (canChangeMat)
        {
            StartCoroutine(FadeInOut(2f,false));
            //Debug.Log("Called" + gameObject.name);  
        }

    }

    void OnPlayerSelect()
    {
        StopAllCoroutines();
        //Debug.Log("Called" + gameObject.name);
    }

    IEnumerator FadeInOut(float TimeToFade, bool isFadeIn)
    {
        if (!isFadeIn)
        {
            for (float i = TimeToFade; i >= 0; i -= Time.deltaTime * animespeed )
            {
                mat.SetFloat("_EmissionStrength", (i / TimeToFade) * 10);
                //Debug.Log(mat.GetFloat("_EmissionStrength"));
                yield return null;
            }
            StartCoroutine(FadeInOut(TimeToFade, true));
        }
        else
        {
            for (float i = 0 ; i <= TimeToFade; i += Time.deltaTime * animespeed)
            {
                mat.SetFloat("_EmissionStrength", (i / TimeToFade)*10);
                //Debug.Log(mat.GetFloat("_EmissionStrength"));
                yield return null;
            } 
            StartCoroutine(FadeInOut(TimeToFade, false));
        }


    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
