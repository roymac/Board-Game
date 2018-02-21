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
        mat.SetFloat("_EmissionStrength", 0);
	}

    void OnDestroy()
    {
    }

    public void ShowSelectableAnimation()
    {
        if (canChangeMat)
        {
            StartCoroutine(FadeInOut(2f,false)); 
        }

    }

    public void StopSelectableAnimation()
    {
        StopAllCoroutines();
    }

    IEnumerator FadeInOut(float TimeToFade, bool isFadeIn)
    {
        if (!isFadeIn)
        {
            for (float i = TimeToFade; i >= 0; i -= Time.deltaTime * animespeed )
            {
                mat.SetFloat("_EmissionStrength", (i / TimeToFade) *3);
                yield return null;
            }
            StartCoroutine(FadeInOut(TimeToFade, true));
        }
        else
        {
            for (float i = 0 ; i <= TimeToFade; i += Time.deltaTime * animespeed)
            {
                mat.SetFloat("_EmissionStrength", (i / TimeToFade)*3);
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
