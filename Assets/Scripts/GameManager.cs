using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int redHomeCount, blueHomeCount, greenHomeCount, yellowHomeCount;
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
    public int lockedplayers;
    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () 
    {
        DiceValues = new List<int>();
        am = GetComponent<AchievementsManager>();
        players = GameObject.FindGameObjectsWithTag("Player");
        BlueGameObject.SetActive(false);
        GreenGameObject.SetActive(false);
        RedGameObject.SetActive(false);
        YellowGameObject.SetActive(false);
        if (!PlayerSelection.isNetworkedGame)
        {
            GameObject obj = (GameObject)Instantiate(NormalDice, DicePosition.transform.position, Quaternion.identity);
            dice = obj.GetComponent<DiceThrow>();
            InitializeGame();
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
        }

        for (int i = 0; i < totalPlayers.Count; i++)
        {
            switch (totalPlayers[i])
            {
                case PawnColor.c_Blue:
                    BlueGameObject.SetActive(true);
                    break;
                case PawnColor.c_Red:
                    RedGameObject.SetActive(true);
                    break;
                case PawnColor.c_Green:
                    GreenGameObject.SetActive(true);
                    break;
                case PawnColor.c_Yellow:
                    YellowGameObject.SetActive(true);
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
                    SetNextTurn();
                    EventManager.PlayerSelected();
                }
            }

            if (lockedplayers == 3)
            {
                EventManager.CallOnePlayerOut();
            }

            allActivePlayerCount = 0;
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
                    totalFinishCount++;
                    totalPlayers.Remove(PawnColor.c_Blue);
                }
                break;
            case PawnColor.c_Red:
                redHomeCount++;
                if (redHomeCount == 4)
                {
                    totalFinishCount++;
                    totalPlayers.Remove(PawnColor.c_Red);
                }
                break;
            case PawnColor.c_Yellow:
                yellowHomeCount++;
                if (yellowHomeCount == 4)
                {
                    totalFinishCount++;
                    totalPlayers.Remove(PawnColor.c_Yellow);
                }
                break;
            case PawnColor.c_Green:
                greenHomeCount++;
                if (greenHomeCount == 4)
                {
                    totalFinishCount++;
                    totalPlayers.Remove(PawnColor.c_Green);
                }
                break;
        }

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
                }
            }
            else
            {
                am.Unlucky();
            }
        }

        if (totalFinishCount == PlayerSelection.playerInfo.Count-1)
        {
            if (greenHomeCount < 4)
            {
                ui_m.OnGameOver("Green Lost");
                if (PlayerSelection.playerColor == PawnColor.c_Green)
                {
                    am.isFirstLoss();
                }
                return;
            }
            if (redHomeCount < 4)
            {
                ui_m.OnGameOver("Red Lost");
                if (PlayerSelection.playerColor == PawnColor.c_Red)
                {
                    am.isFirstLoss();
                }
                return;
            }
            if (blueHomeCount < 4)
            {
                ui_m.OnGameOver("Blue Lost");
                if (PlayerSelection.playerColor == PawnColor.c_Blue)
                {
                    am.isFirstLoss();
                }
                return;
            }
            if (yellowHomeCount < 4)
            {
                ui_m.OnGameOver("Yellow Lost");
                if (PlayerSelection.playerColor == PawnColor.c_Yellow)
                {
                    am.isFirstLoss();
                }
                return;
            }
        }
    }
    public void SetDiceRollValue(int in_Value)
    {
        HideDice();
        ui_m.ShowDiceValues(currentPlayerTurn, in_Value.ToString());
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
                AudioManager.Instance.PawnMove(true);
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
                    AudioManager.Instance.PawnMove(true);
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
        AudioManager.Instance.PawnMove(false);
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
            DiceValues.Clear();
            SetNextTurn();
        }
    }

    public void ThrowTheDice()
    {
        if (PlayerSelection.isNetworkedGame)
        {
            nm_server.ThrowDice();
            Debug.Log("Check");
        }
        else
        {
            ShowDice();
            dice.ThrowDice();
        }
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
        //AudioManager.Instance.RollTheDice(true);
    }
    public void HideDice()
    {
        dice.ShowDice(true);
        //AudioManager.Instance.RollTheDice(false);
    }


}
