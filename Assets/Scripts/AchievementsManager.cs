using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour 
{
    static int count;
    public Text debugText;
    public int homesafeCount;
    public List<GameObject> LockedPlayers;
	// Use this for initialization
	void Start () 
    {
        PlayerPrefs.DeleteAll();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void ShowDebugText(string text)
    {
        debugText.text = text;
        Invoke("removeText", 2f);
    }

    void removeText()
    {
        debugText.text = "";
    }

    public void HomeSafe()
    {
        if (PlayerPrefs.GetInt("HomeSafe", 0) == 0)
        {
            homesafeCount++;
            if (homesafeCount == 4)
            {
                ShowDebugText("Achovement Unlocked HomeSafe");
            }
        }
    }

    public void EveryOneButMe()
    {
        ShowDebugText("achivement Unlocked Everyonebutme");
    }

    public void FirstSinglePlayerWin()
    {
        ShowDebugText("achivement Unlocked FirstSinglePlayerWin");
    }

    public void FirstMultiPlayerWin()
    {
        ShowDebugText("achivement Unlocked FirstMultiPlayerWin");
    }
    public void HomeSafeHome()
    {
        if (LockedPlayers.Count == 4)
        {
            ShowDebugText("achivement Unlocked HomeSafeHome");
        }
    }
    public void Unlucky()
    {
        if (homesafeCount > 0 && homesafeCount < 4)
        {
            ShowDebugText("achivement Unlocked Unlucky");
        }
    }
    public void TasteOfVictory()
    {
        ShowDebugText("achivement Unlocked TasteOfVictory");
    }

    public void isFirstLoss()
    {
        if (PlayerPrefs.GetInt("isfirstloss", 0) == 0)
        {
            PlayerPrefs.SetInt("isfirstloss", 1);
            //Call Achievement google here, Debugging it for now
            ShowDebugText("Achievement Unlocked isfirstloss");
        }
        HomeSafeHome();
    }
    public void KillStreak()
    {
        if (PlayerPrefs.GetInt("KillStreak", 0) == 0)
        {
            PlayerPrefs.SetInt("KillStreak", 1);
            //Call Achievement google here
            ShowDebugText("Achievement Unlocked KillStreak");
        }
    }
    public void DeathStreak()
    {
        if (PlayerPrefs.GetInt("DeathStreak", 0) == 0)
        {
            PlayerPrefs.SetInt("DeathStreak", 1);
            //Call Achievement google here
            ShowDebugText("Achievement Unlocked DeathStreak");
        }
    }
    public void LuckyHand()
    {
        if (PlayerPrefs.GetInt("LuckyHand", 0) == 0)
        {
            PlayerPrefs.SetInt("LuckyHand", 1);
            //Call Achievement google here
            ShowDebugText("Achievement Unlocked LuckyHand");
        }
    }
    public void PawnPyramid()
    {
        if (PlayerPrefs.GetInt("PawnPyramid", 0) == 0)
        {
            PlayerPrefs.SetInt("PawnPyramid", 1);
            //Call Achievement google here
            ShowDebugText("Achievement Unlocked PawnPyramid");
        }
    } 
    public void DuckingForCover()
    {
        if (PlayerPrefs.GetInt("DuckingForCover", 0) == 0)
        {
            PlayerPrefs.SetInt("DuckingForCover", 1);
            //Call Achievement google here
            ShowDebugText("Achievement Unlocked DuckingForCover");
        }
    }
    public void Statue()
    {
        if (PlayerPrefs.GetInt("Statue", 0) == 0)
        {
            PlayerPrefs.SetInt("Statue", 1);
            //Call Achievement google here
            Debug.Log("Achievement Unlocked Statue");
        }
    }
    public void GodLike()
    {
        count++;
        if (count == 4)
        {
            if (PlayerPrefs.GetInt("GodLike", 0) == 0)
            {
                PlayerPrefs.SetInt("GodLike", 1);
                //Call Achievement google here
                Debug.Log("Achievement Unlocked GodLike");
            }  
        }

    }
}
