using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour 
{
    public UIManager ui_m;
    public DiceThrow dice;
    public List<PawnColor> totalPlayers;
    public List<string> playerNames;
    public GameObject currentPlayer;
    public PawnColor currentPlayerTurn;
    public int diceRollValue;
    public bool canSelectPlayer;
    public int redHomeCount, blueHomeCount, greenHomeCount, yellowHomeCount = -1;
    public int totalFinishCount;

    public GameObject RedGameObject, BlueGameObject, YellowGameObject, GreenGameObject,NormalDice,DicePosition;
    public NetworkManagerServer nm_server;
    public Texture[] boardTexture;
    public Material mat;

    public GameObject[] players;
    public int networkedPlayerDiceRollValues;
    int playerIndex = -1;
    List<int> DiceValues;
    AchievementsManager am;
    int allActivePlayerCount = 0;
    public int totalPlayersPlayingCount = 0;
    public int lockedplayers;
    public List<string> FinishList;
    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () 
    {
		Debug.Log ("player info : " + PlayerSelection.playerInfo);
        DiceValues = new List<int>();
        am = GetComponent<AchievementsManager>();
        players = GameObject.FindGameObjectsWithTag("Player");
        BlueGameObject.SetActive(false);
        GreenGameObject.SetActive(false);
        RedGameObject.SetActive(false);
        YellowGameObject.SetActive(false);
        redHomeCount = blueHomeCount = yellowHomeCount = greenHomeCount = -11;
        if (!PlayerSelection.isNetworkedGame)
        {
            GameObject obj = (GameObject)Instantiate(NormalDice, DicePosition.transform.position, Quaternion.identity);
            dice = obj.GetComponent<DiceThrow>();
            Invoke("InitializeGame", 1f);
        }
	}

    public void InitializeGame()
    {
		totalPlayers.Clear();
        playerNames.Clear();
        Debug.Log(SelectPlayField.whichBoard);
        mat.mainTexture = boardTexture[SelectPlayField.whichBoard];
        print("from manager : " + PlayerSelection.playerInfo.Count);
        for (int i = 0; i < PlayerSelection.playerInfo.Count; i++)
        {
            totalPlayers.Add(PlayerSelection.playerInfo[i].color);
            playerNames.Add(PlayerSelection.playerInfo[i].name);

			if (playerNames[i] == "" || playerNames[i] == null)
			{
				switch (totalPlayers[i])
				{
				case PawnColor.c_Blue:
					playerNames[i] = "Blue";
					break;
				case PawnColor.c_Green:
					playerNames[i] = "Green";
					break;
				case PawnColor.c_Red:
					playerNames[i] = "Red";
					break;
				case PawnColor.c_Yellow:
					playerNames[i] = "Yellow";
					break;
				}
			}
        }

        totalPlayersPlayingCount = totalPlayers.Count;

        for (int i = 0; i < totalPlayers.Count; i++)
        {
            switch (totalPlayers[i])
            {
                case PawnColor.c_Blue:
                    BlueGameObject.SetActive(true);
                    blueHomeCount = 0;
                    break;
                case PawnColor.c_Red:
                    RedGameObject.SetActive(true);
                    redHomeCount = 0;
                    break;
                case PawnColor.c_Green:
                    GreenGameObject.SetActive(true);
                    greenHomeCount = 0;
                    break;
                case PawnColor.c_Yellow:
                    YellowGameObject.SetActive(true);
                    yellowHomeCount = 0;
                    break;
            }
        }

        if (!PlayerSelection.isNetworkedGame)
        {
            int randIndex = Random.Range(0, totalPlayers.Count);
            currentPlayerTurn = totalPlayers[randIndex];
            if (!CheckAITurn())
            {
                ui_m.ActivateSpinButton();  
            }
            ui_m.ShowCurrentTurn(currentPlayerTurn, playerNames[randIndex]);
            Debug.Log("Non Networked");
        }
        else
        {
            
            Debug.Log("Networked");
            nm_server.GetStartingTurn(totalPlayers.Count);
        }

        ui_m.RemoveLoadingScreen();
            
		AnalyticsResult result = Analytics.CustomEvent ("GameStart");
		print ("game start analytics result : " + result);
    }

    public void StartNetworkedTurn(int num)
    {
        Debug.Log("Back from Network");
        Debug.Log(totalPlayers.Count + "\t" + num);
        currentPlayerTurn = totalPlayers[num];

        if (!CheckAITurn())
        {
            ui_m.ActivateSpinButton();  
        }
        ui_m.ShowCurrentTurn(currentPlayerTurn,playerNames[num]);
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    bool CheckAITurn()
    {
        if (PlayerSelection.playerColor != PawnColor.c_null && currentPlayerTurn != PlayerSelection.playerColor && !PlayerSelection.isNetworkedGame)
        {
           Debug.Log("Check");
           ThrowTheDice();
           return true;
        }
        else
        {
            if (!PlayerSelection.isNetworkedGame)
            {
                ui_m.DeactivateSpinButton();
                return false; 
            }
            else
            {
                if (currentPlayerTurn != PlayerSelection.playerColor)
                {
                    Debug.Log("My Turn");
                    ui_m.DeactivateSpinButton();
                    return true; 
                }
                else
                {
                    Debug.Log("Not My Turn");
                    ui_m.DeactivateSpinButton();
                    return false;
                }
            }
        }
    }

    public void SetNextTurn()
    {
        lockedplayers = 0;
        if (playerIndex < 0)
        {
            playerIndex = totalPlayers.IndexOf(currentPlayerTurn);
        }
        if (playerIndex < totalPlayers.Count - 1)
        {
            playerIndex++;
        }
        else
        {
            playerIndex = 0;
        }

        currentPlayerTurn = totalPlayers[playerIndex];
        if (!CheckAITurn())
        {
            ui_m.ActivateSpinButton();  
        }
        ui_m.ShowCurrentTurn(currentPlayerTurn,playerNames[playerIndex]);
    }

    public void SetLockedPlayers()
    {
        lockedplayers++;

    }

    public void SetActivePlayerDone()
    {
        allActivePlayerCount++;
        if (allActivePlayerCount == 4)
        {
            if (PlayerSelection.isNetworkedGame)
            {
                if (lockedplayers == 4)
                {
                    nm_server.SetNextTurn();
                    EventManager.PlayerSelected();
                }
            }
            else
            {
                if (lockedplayers == 4)
                {
//                    SetNextTurn();
//                    EventManager.PlayerSelected();
					Invoke("ChangeTurn", 1f);
                }
            }

            if (lockedplayers == 3)
            {
                EventManager.CallOnePlayerOut();
            }

            allActivePlayerCount = 0;
        }

    }

	void ChangeTurn()
	{
		SetNextTurn();
		EventManager.PlayerSelected();
	}

    public void CheckPlayersOnDisconnect(PawnColor col)
    {
        switch (col)
        {
            case PawnColor.c_Blue:
                if (blueHomeCount < 4)
                {
                    totalPlayersPlayingCount--;
                    blueHomeCount = -1;
                }
                break;
            case PawnColor.c_Red:
                if (redHomeCount < 4)
                {
                    totalPlayersPlayingCount--;
                    redHomeCount = -1;
                }
                break;
            case PawnColor.c_Yellow:
                if (yellowHomeCount< 4)
                {
                    totalPlayersPlayingCount--;
                    yellowHomeCount = -1;
                }
                break;
            case PawnColor.c_Green:
                if (greenHomeCount < 4)
                {
                    totalPlayersPlayingCount--;
                    greenHomeCount = -1;
                }
                break;
        }

        if (totalFinishCount == totalPlayersPlayingCount - 1 && totalFinishCount > 0)
        {
            if (greenHomeCount < 4)
            {
                if(totalPlayers.Contains(PawnColor.c_Green))
                {
                    int indexOfColor = totalPlayers.IndexOf(PawnColor.c_Green);
                    ui_m.FinishList.Add(playerNames[indexOfColor]);
                    ui_m.OnGameOver("Green Lost");
                    if (PlayerSelection.playerColor == PawnColor.c_Green)
                    {
                        am.isFirstLoss();
                    }
                    return;
                }
                
            }
            if (redHomeCount < 4)
            {
                if (totalPlayers.Contains(PawnColor.c_Red))
                {
                    int indexOfColor = totalPlayers.IndexOf(PawnColor.c_Red);
                    ui_m.FinishList.Add(playerNames[indexOfColor]);
                    ui_m.OnGameOver("Red Lost");
                    if (PlayerSelection.playerColor == PawnColor.c_Red)
                    {
                        am.isFirstLoss();
                    }
                    return;
                }
               
            }
            if (blueHomeCount < 4)
            {
                if (totalPlayers.Contains(PawnColor.c_Blue))
                {
                    int indexOfColor = totalPlayers.IndexOf(PawnColor.c_Blue);
                    ui_m.FinishList.Add(playerNames[indexOfColor]);
                    ui_m.OnGameOver("Blue Lost");
                    if (PlayerSelection.playerColor == PawnColor.c_Blue)
                    {
                        am.isFirstLoss();
                    }
                    return;
                }
                
            }
            if (yellowHomeCount < 4)
            {
                if (totalPlayers.Contains(PawnColor.c_Yellow))
                {
                    int indexOfColor = totalPlayers.IndexOf(PawnColor.c_Yellow);
                    ui_m.FinishList.Add(playerNames[indexOfColor]);
                    ui_m.OnGameOver("Yellow Lost");
                    if (PlayerSelection.playerColor == PawnColor.c_Yellow)
                    {
                        am.isFirstLoss();
                    }
                    return;
                }
                
            }
        }
        else
        {
            if (totalPlayersPlayingCount == 1)
            {
                ui_m.OnGameOver("connection lost");
            }
        }
    }

    public void OnLoosingServer()
    {
        ui_m.OnGameOver("connection lost");
    }

    void OnFinish()
    {
        //CHecking for Win Achievements;
        if (totalFinishCount == 1)
        {
            if (PlayerSelection.playerColor == currentPlayerTurn)
            {
                if (PlayerSelection.isNetworkedGame)
                {
                    am.FirstMultiPlayerWin();
                }
                else
                {
                    am.FirstSinglePlayerWin();
                    CoinManager.AwardCoins(10);

                }
            }
            else
            {
                am.Unlucky();
            }
        }

        if (totalFinishCount == totalPlayersPlayingCount - 1)
        {
            if (greenHomeCount < 4 && greenHomeCount >= 0)
            {
                int indexOfColor = totalPlayers.IndexOf(PawnColor.c_Green);
                ui_m.FinishList.Add(playerNames[indexOfColor]);
                ui_m.OnGameOver("Green Lost");
                if (PlayerSelection.playerColor == PawnColor.c_Green)
                {
                    am.isFirstLoss();
                }
                return;
            }
            if (redHomeCount < 4 && redHomeCount >= 0)
            {
                int indexOfColor = totalPlayers.IndexOf(PawnColor.c_Red);
                ui_m.FinishList.Add(playerNames[indexOfColor]);
                ui_m.OnGameOver("Red Lost");
                if (PlayerSelection.playerColor == PawnColor.c_Red)
                {
                    am.isFirstLoss();
                }
                return;
            }
            if (blueHomeCount < 4 && blueHomeCount >= 0)
            {
                int indexOfColor = totalPlayers.IndexOf(PawnColor.c_Blue);
                ui_m.FinishList.Add(playerNames[indexOfColor]);
                ui_m.OnGameOver("Blue Lost");
                if (PlayerSelection.playerColor == PawnColor.c_Blue)
                {
                    am.isFirstLoss();
                }
                return;
            }
            if (yellowHomeCount < 4 && yellowHomeCount >= 0)
            {
                int indexOfColor = totalPlayers.IndexOf(PawnColor.c_Yellow);
                ui_m.FinishList.Add(playerNames[indexOfColor]);
                ui_m.OnGameOver("Yellow Lost");
                if (PlayerSelection.playerColor == PawnColor.c_Yellow)
                {
                    am.isFirstLoss();
                }
                return;
            }
        }
        else
        {
            
            if (PlayerSelection.playerColor == currentPlayerTurn && !PlayerSelection.isNetworkedGame)
            {
				ui_m.OnGameOver("Game Over, Player was: " + totalFinishCount.ToString());
            }
            else
            {
                OnMoveFinished(null);
            }


        }
    }
    public void CountPawnsAtHome()
    {
        switch (currentPlayerTurn)
        {
            case PawnColor.c_Blue:
                blueHomeCount++;
                if (blueHomeCount == 4)
                {
                    int index = totalPlayers.IndexOf(currentPlayerTurn);
                    ui_m.FinishList.Add(playerNames[index]);
                    totalFinishCount++;
                    playerNames.Remove(playerNames[index]);
                    totalPlayers.Remove(PawnColor.c_Blue);
                    OnFinish();
                }
                else
                {
                    if (blueHomeCount < 4)
                    {
                        OnMoveFinished(this.gameObject);
                    }
                }
                break;
            case PawnColor.c_Red:
                redHomeCount++;
                if (redHomeCount == 4)
                {
                    int index = totalPlayers.IndexOf(currentPlayerTurn);
                    ui_m.FinishList.Add(playerNames[index]);
                    totalFinishCount++;
                    playerNames.Remove(playerNames[index]);
                    totalPlayers.Remove(PawnColor.c_Red);
                    OnFinish();
                }
                else
                {
                    if (redHomeCount < 4)
                    {
                        OnMoveFinished(this.gameObject);
                    }
                }
                break;
            case PawnColor.c_Yellow:
                yellowHomeCount++;
                if (yellowHomeCount == 4)
                {
                    int index = totalPlayers.IndexOf(currentPlayerTurn);
                    ui_m.FinishList.Add(playerNames[index]);
                    totalFinishCount++;
                    playerNames.Remove(playerNames[index]);
                    totalPlayers.Remove(PawnColor.c_Yellow);
                    OnFinish();
                }
                else
                {
                    if (yellowHomeCount < 4)
                    {
                        OnMoveFinished(this.gameObject);
                    }
                }
                break;
            case PawnColor.c_Green:
                greenHomeCount++;
                if (greenHomeCount == 4)
                {
                    int index = totalPlayers.IndexOf(currentPlayerTurn);
                    ui_m.FinishList.Add(playerNames[index]);
                    totalFinishCount++;
                    playerNames.Remove(playerNames[index]);
                    totalPlayers.Remove(PawnColor.c_Green);
                    OnFinish();
                }
                else
                {
                    if (greenHomeCount < 4)
                    {
                        OnMoveFinished(this.gameObject);
                    }
                }
                break;
        }

    }
    public void SetDiceRollValue(int in_Value)
    {
        HideDice();
        //ui_m.ShowDiceValues(currentPlayerTurn, in_Value.ToString());
        canSelectPlayer = true;
        diceRollValue = in_Value;
        switch (currentPlayerTurn)
        {
            case PawnColor.c_Blue:
                if (!PlayerSelection.isNetworkedGame)
                {
                    EventManager.PawnUnlockBlue(in_Value);
                }
                else
                {
                    if (PlayerSelection.playerColor == currentPlayerTurn)
                    {
                        EventManager.PawnUnlockBlue(in_Value);
                    }
                }
                break;
            case PawnColor.c_Red:
                if (!PlayerSelection.isNetworkedGame)
                {
                    EventManager.PawnUnlockRed(in_Value);
                }
                else
                {
                    if (PlayerSelection.playerColor == currentPlayerTurn)
                    {
                        EventManager.PawnUnlockRed(in_Value);
                    }
                }
                break;
            case PawnColor.c_Yellow:
                if (!PlayerSelection.isNetworkedGame)
                {
                    EventManager.PawnUnlockYellow(in_Value);
                }
                else
                {
                    if (PlayerSelection.playerColor == currentPlayerTurn)
                    {
                        EventManager.PawnUnlockYellow(in_Value);
                    }
                }
                break;
            case PawnColor.c_Green:
                if (!PlayerSelection.isNetworkedGame)
                {
                    EventManager.PawnUnlockGreen(in_Value);
                }
                else
                {
                    if (PlayerSelection.playerColor == currentPlayerTurn)
                    {
                        EventManager.PawnUnlockGreen(in_Value);
                    }
                }
                break;
        }
    }

    public void SelectPlayerNetwork(string name)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name.Equals(name))
            {
                currentPlayer = players[i];
                currentPlayer.GetComponent<PlayerMovement>().isLocked = false;
                currentPlayer.GetComponent<PlayerMovement>().diceRoll = networkedPlayerDiceRollValues;
                currentPlayer.GetComponent<PlayerMovement>().MoveCharacter();
                EventManager.PlayerSelected();
                canSelectPlayer = false;
               // AudioManager.Instance.PawnMove(true);
                break;
            }
        }
    }

    public void SetCurrentPlayer(GameObject obj,PawnColor pawn_col)
    {
        if (PlayerSelection.isNetworkedGame)
        {
            nm_server.UpdateDiceValue(obj.GetComponent<PlayerMovement>().diceRoll);
            nm_server.SetPlayerOverNetwork(obj.name);
        }
        else
        {
            if (canSelectPlayer)
            {
                if (currentPlayerTurn == pawn_col)
                {
                    currentPlayer = obj;
                    obj.GetComponent<PlayerMovement>().MoveCharacter();
                    EventManager.PlayerSelected();
                    //AudioManager.Instance.PawnMove(true);
                    AudioManager.Instance.ClickPawns();
                }
                canSelectPlayer = false;
            }  
        }
    }

    public void OnMoveFinished(GameObject player_killed)
    {
        lockedplayers = 0;
        currentPlayer = null;
        //AudioManager.Instance.PawnMove(false);
        if (diceRollValue == 6 || player_killed != null)
        {
            if (!CheckAITurn())
            {
                if (diceRollValue == 6)
                {
                    DiceValues.Add(6);
                    if (DiceValues.Count == 3)
                    {
                        am.LuckyHand();
                        DiceValues.Clear();
                    }
                }
                else
                {
                    DiceValues.Clear();
                }
                ui_m.ActivateSpinButton();  
            }
        }
        else
        {
//			print("")
            DiceValues.Clear();
            SetNextTurn();
        }
    }

    public void ThrowTheDice()
    {
        if (PlayerSelection.isNetworkedGame)
        {
            nm_server.ThrowDice();
        }
        else
        {
            ShowDice();
            dice.ThrowDice();
        }
    }

    public void UIDiceValueShowOnRollStop(int val)
    {
        if (PlayerSelection.isNetworkedGame)
        {
            nm_server.UIDice(val);
        }
        else
        {
            showUIDice(val);
        }
    }

    public void showUIDice(int in_Value)
    {
        ui_m.ShowDiceValues(currentPlayerTurn, in_Value.ToString());
    }

    public void SetDiceVale(int val)
    {
        if (PlayerSelection.isNetworkedGame)
        {
            nm_server.SetDiceValue(val);
        }
        else
        {
           SetDiceRollValue(val);
        }
    }

    public void ShowDice()
    {
        dice.ShowDice(false);
       // ui_m.HideDiceImage();
        AudioManager.Instance.RollTheDice(true);
    }
    public void HideDice()
    {
        dice.ShowDice(true);
        AudioManager.Instance.RollTheDice(false);
    }


    int killcount, deathcount;
    public void IncreaseKillCount()
    {
        killcount++;

        if (killcount == 5)
        {
            am.KillStreak();
        }
        if (killcount == 10)
        {
            am.KillStreakOne();
        }
        if (killcount == 15)
        {
            am.KillStreakTwo();
        }
    }

    public void IncreaseDeathCount()
    {
        killcount = 0;
        deathcount++;

        if (deathcount == 5)
        {
            am.DeathStreak();
        }
        if (deathcount == 10)
        {
            am.DeathStreakOne();
        }
        if (deathcount == 20)
        {
            am.DeathStreakTwo();
        }
    }
}
