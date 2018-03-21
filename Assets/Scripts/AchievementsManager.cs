using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
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
        Debug.Log("From achievement manager");
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
                Social.ReportProgress(LudoEraResources.achievement_home_safe, 100.0f, (bool success) =>
                {
                    //sucess or failure
                });
            }
        }
    }

    public void EveryOneButMe()
    {
        ShowDebugText("achivement Unlocked Everyonebutme");
        Social.ReportProgress(LudoEraResources.achievement_everyone_but_me, 100.0f, (bool success) =>
        {
            //sucess or failure
        });
    }

    public void FirstSinglePlayerWin()
    {
        ShowDebugText("achivement Unlocked FirstSinglePlayerWin");
        Social.ReportProgress(LudoEraResources.achievement_first_sp_win, 100.0f, (bool success) =>
        {
            //sucess or failure
        });
    }

    public void FirstMultiPlayerWin()
    {
        ShowDebugText("achivement Unlocked FirstMultiPlayerWin");
       Social.ReportProgress(LudoEraResources.achievement_first_mp_win, 100.0f, (bool success) =>
       {
           //sucess or failure
       });
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
            Social.ReportProgress(LudoEraResources.achievement_unlucky, 100.0f, (bool success) =>
            {
                //sucess or failure
            });
        }

   
    }
    public void TasteOfVictory()
    {
        ShowDebugText("achivement Unlocked TasteOfVictory");

        Social.ReportProgress(LudoEraResources.achievement_taste_of_victory, 100.0f, (bool success) =>
        {
            //sucess or failure
        });
    }

    public void isFirstLoss()
    {
        if (PlayerPrefs.GetInt("isfirstloss", 0) == 0)
        {
            PlayerPrefs.SetInt("isfirstloss", 1);
            //Call Achievement google here, Debugging it for now
            ShowDebugText("Achievement Unlocked isfirstloss");
        }

        Social.ReportProgress(LudoEraResources.achievement_first_loss, 100.0f, (bool success) =>
        {
            //sucess or failure
        });
        //HomeSafeHome();
    }
    public void KillStreak()
    {
        if (PlayerPrefs.GetInt("KillStreak", 0) == 0)
        {
            PlayerPrefs.SetInt("KillStreak", 1);
            //Call Achievement google here
            Social.ReportProgress(LudoEraResources.achievement_kill_streak_1, 100.0f, (bool success) =>
            {
                //sucess or failure
            });
            ShowDebugText("Achievement Unlocked KillStreak");
        }
    }
    public void KillStreakOne()
    {
        if (PlayerPrefs.GetInt("KillStreakOne", 0) == 0)
        {
            PlayerPrefs.SetInt("KillStreakOne", 1);
            //Call Achievement google here
            Social.ReportProgress(LudoEraResources.achievement_kill_streak_2, 100.0f, (bool success) =>
            {
                //sucess or failure
            });

            ShowDebugText("Achievement Unlocked KillStreakOne");
        }
    }
    public void KillStreakTwo()
    {
        if (PlayerPrefs.GetInt("KillStreakTwo", 0) == 0)
        {
            PlayerPrefs.SetInt("KillStreakTwo", 1);
            //Call Achievement google here
            Social.ReportProgress(LudoEraResources.achievement_kill_streak_3, 100.0f, (bool success) =>
            {
                //sucess or failure
            });
            ShowDebugText("Achievement Unlocked KillStreakTwo");
        }
    }

    public void DeathStreak()
    {
        if (PlayerPrefs.GetInt("DeathStreak", 0) == 0)
        {
            PlayerPrefs.SetInt("DeathStreak", 1);
            //Call Achievement google here
            Social.ReportProgress(LudoEraResources.achievement_death_streak_1, 100.0f, (bool success) =>
            {
                //sucess or failure
            });

            ShowDebugText("Achievement Unlocked DeathStreak");
        }
    }

    public void DeathStreakOne()
    {
        if (PlayerPrefs.GetInt("DeathStreak1", 0) == 0)
        {
            PlayerPrefs.SetInt("DeathStreak1", 1);
            //Call Achievement google here
            Social.ReportProgress(LudoEraResources.achievement_death_streak_2, 100.0f, (bool success) =>
            {
                //sucess or failure
            });

            ShowDebugText("Achievement Unlocked DeathStreak");
        }
    }

    public void DeathStreakTwo()
    {
        if (PlayerPrefs.GetInt("DeathStreak2", 0) == 0)
        {
            PlayerPrefs.SetInt("DeathStreak2", 1);
            //Call Achievement google here
            Social.ReportProgress(LudoEraResources.achievement_death_streak_3, 100.0f, (bool success) =>
            {
                //sucess or failure
            });

            ShowDebugText("Achievement Unlocked DeathStreak");
        }
    }

    public void LuckyHand()
    {
        if (PlayerPrefs.GetInt("LuckyHand", 0) == 0)
        {
            PlayerPrefs.SetInt("LuckyHand", 1);
            //Call Achievement google here
            Social.ReportProgress(LudoEraResources.achievement_lucky_hand, 100.0f, (bool success) =>
            {
                //sucess or failure
            });
            ShowDebugText("Achievement Unlocked LuckyHand");
        }
    }
    public void PawnPyramid()
    {
        if (PlayerPrefs.GetInt("PawnPyramid", 0) == 0)
        {
            PlayerPrefs.SetInt("PawnPyramid", 1);
            //Call Achievement google here

            Social.ReportProgress(LudoEraResources.achievement_pawn_pyramid, 100.0f, (bool success) =>
            {
                //sucess or failure
            });

            ShowDebugText("Achievement Unlocked PawnPyramid");
        }
    } 
    public void DuckingForCover()
    {
        if (PlayerPrefs.GetInt("DuckingForCover", 0) == 0)
        {
            PlayerPrefs.SetInt("DuckingForCover", 1);
            //Call Achievement google here
            Social.ReportProgress(LudoEraResources.achievement_ducking_for_cover, 100.0f, (bool success) =>
            {
                //sucess or failure
            });

            ShowDebugText("Achievement Unlocked DuckingForCover");
        }
    }
    public void Statue()
    {
        if (PlayerPrefs.GetInt("Statue", 0) == 0)
        {
            PlayerPrefs.SetInt("Statue", 1);

            Social.ReportProgress(LudoEraResources.achievement_statue, 100.0f, (bool success) =>
            {
                //sucess or failure
            });
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
                Social.ReportProgress(LudoEraResources.achievement_godlike, 100.0f, (bool success) =>
                {
                    //sucess or failure
                });
                Debug.Log("Achievement Unlocked GodLike");
            }  
        }

    }
}
