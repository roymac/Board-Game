using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LudoLoader : MonoBehaviour {

    public static bool isFirstload;
    public GameObject LevelLoadScreen;
    public static LudoLoader instance;
    public GameObject SplashPlayer;
    public RenderTexture tex;
    

	// Use this for initialization
	void Start () 
    {
        
        SceneManager.sceneLoaded += newSceneLoaded;
        if (instance == null)
        {
            instance = this;
        }
        if (!isFirstload)
        {
            if (SplashPlayer != null)
            {
                SplashPlayer.SetActive(true);
                // SplashPlayer.GetComponent<Animation>().Play();
                //Invoke("RemoveSplash", SplashPlayer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length*1.5f);
                Invoke("RemoveSplash", 3.5f);
            }
            isFirstload = false;
        }
        else
        {
            if (SplashPlayer != null)
            {
                SplashPlayer.SetActive(false);
            }
        }

	}

    public void RemoveSplash()
    {
        SplashPlayer.SetActive(false);
        
    }

    void newSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            LevelLoadScreen = GameObject.Find("Canvas").GetComponent<UIScript>().loadingScreen;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= newSceneLoaded;
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void LevelLoaderCall(string level)
    {
        StartCoroutine(Loader(level));
    }

    public void LevelLoaderCallNormal(string Level)
    {
        LevelLoadScreen.SetActive(true);
        SceneManager.LoadScene(Level);
    }

    IEnumerator Loader(string level_name)
    {
        LevelLoadScreen.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(level_name);

        async.allowSceneActivation = false;
     
		while (async.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        async.allowSceneActivation = true;
    }


}
