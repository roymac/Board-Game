using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoaderScreenScript : MonoBehaviour 
{
    public Text pulseText;
    float animespeed = 0.5f;
    public static LoaderScreenScript instance;
	// Use this for initialization
	void Start () 
    {
        
	}

    void OnEnable()
    {
        StartCoroutine(Pulse(1f,false));
    }

    void OnDisable()
    {
        StopAllCoroutines();
        pulseText.color = Color.white;
        Debug.Log(SceneManager.GetActiveScene().name + "Level name for loadingScreen Deactivate");
    }
	
	// Update is called once per frame
	void Update () 
    {
	}

    IEnumerator Pulse(float TimeToFade, bool isFadeIn)
    {
        if (!isFadeIn)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime * animespeed)
            {
                Color col = pulseText.color;
                col.a = i;
                pulseText.color = col;
                yield return null;
            }
            StartCoroutine(Pulse(TimeToFade, true));
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime * animespeed)
            {
                Color col = pulseText.color;
                col.a = i;
                pulseText.color = col;
                yield return null;
            } 
            StartCoroutine(Pulse(TimeToFade, false));
        }
    }

}
