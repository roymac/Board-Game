using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioClip[] GameMusic;

    public AudioSource BGSource;
    public AudioSource pawnAudioSource1;
    public AudioSource pawnAudioSource2;

    public AudioClip pawnMovement;
    public AudioClip pawnDeath;
    public AudioClip pawnSpawn;
    public AudioClip diceRoll;
    public AudioClip victroyAudio;
    public AudioClip clickObjectAudio;

    public AudioClip clickUI;

    public int songNumber = 0;

    public static bool mute = false;
    public bool dontDestroyOnLoad = false;

    public static AudioManager Instance = null;

    void Awake ()
    {
        SceneManager.sceneLoaded += SceneLoad;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


        if (dontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);
    }

    void SceneLoad(Scene scene, LoadSceneMode mode)
    {
        pawnAudioSource1.Stop();
        pawnAudioSource2.Stop();
        pawnAudioSource1.loop = false;
        pawnAudioSource2.loop = false;
    }


    void Start ()
    {
        Debug.Log("Playing on start");
        songNumber = 0;
        PlayBGMusic(songNumber);
		if (PlayerPrefs.HasKey ("MuteAudio"))
		{
			print ("Audio Setting : " + PlayerPrefs.GetInt("MuteAudio"));
			mute = (PlayerPrefs.GetInt ("MuteAudio") == 1);
		}
		MuteAudioSources (mute);
    }

    private void Update()
    {
        //MuteAudioSources();
    }

	public void MuteAudioSources(bool mute)
    {
		//print ("Time to mute stuff : " + mute);
        if (mute)
        {
            BGSource.volume = 0;
            pawnAudioSource1.volume = 0;
            pawnAudioSource2.volume = 0;
        }
        else
        {
            BGSource.volume = 0.6f;
            pawnAudioSource1.volume = 1f;
            pawnAudioSource2.volume = 1f;
        }
    }

    void PlayInAudioSource1(AudioClip clip)
    {
            pawnAudioSource1.Stop();
            pawnAudioSource1.clip = clip;
            pawnAudioSource1.Play();
    }

    void PlayInAudioSource2(AudioClip clip)
    {
            pawnAudioSource2.Stop();
            pawnAudioSource2.clip = clip;
            pawnAudioSource2.Play();
    }

    public void RollTheDice(bool value)
    {
        if (value)
        {
            pawnAudioSource1.Stop();
            pawnAudioSource1.loop = true;
            pawnAudioSource1.clip = diceRoll;
            pawnAudioSource1.Play();
        }
        else
        {
            pawnAudioSource1.Stop();
            pawnAudioSource1.loop = false;
        }
    }

    public void PlayBGMusic(int songNum)
    {
        BGSource.clip = GameMusic[songNum];
        BGSource.Play();
    }

    public void PawnMove(bool value) 
    {
        if (value)
        {
            pawnAudioSource1.Stop();
           // pawnAudioSource1.loop = true;
            pawnAudioSource1.clip = pawnMovement;
            pawnAudioSource1.Play();
        }
        else
        {
            pawnAudioSource1.Stop();
           // pawnAudioSource1.loop = false;
        }
    }

    public void PawnDeath()
    {
        PlayInAudioSource2(pawnDeath);
    }

    public void PawnSpawn()
    {
        PlayInAudioSource2(pawnSpawn);
    }

    public void ClickPawns()
    {
        PlayInAudioSource2(clickObjectAudio);
    }

    public void ReachedFinish()
    {
        PlayInAudioSource2(victroyAudio);
    }

    public void UIClick()
    {
        PlayInAudioSource1(clickUI);
    }

    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoad;
    }
}
